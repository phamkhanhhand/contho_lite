using CT.Auth; 
using Microsoft.AspNetCore.Authentication.JwtBearer; 
using Microsoft.IdentityModel.Tokens; 
using System.Security.Cryptography; 
using System.IdentityModel.Tokens.Jwt;
using CT.UserContext.CurrentContext;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();


//AddSingleton--all app, only one instance
//AddScoped --each request (the same ssession)
//AddTransient--each inject
//builder.Services.AddScoped<IUserContext, UserContext>();
//builder.Services.AddScoped<IPermissionService, CachedPermissionService>();


//IHttpClientFactory singleton, but HttpClient -Transient
builder.Services.AddHttpClient();

//Singleton special by micosoft
builder.Services.AddMemoryCache();


//IHttpClientFactory singleton, but HttpContext -scoped
builder.Services.AddHttpContextAccessor();


//controller phải đặt [Authorize] thì mới authen cái này
// Cấu hình xác thực với JWT Token 
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var pem = File.ReadAllText("Keys/public.pem");
        var rsa = RSA.Create();
        rsa.ImportFromPem(pem.ToCharArray());

        //options.Authority = "https://your-identity-server";  // URL của Identity Server
        //options.Audience = "your_api";  // Audience của API
        options.TokenValidationParameters = new TokenValidationParameters
        {
            //ValidateIssuerSigningKey = false
            ValidateIssuer = false,     // Bỏ qua việc kiểm tra Issuer
            ValidateAudience = false,   // Bỏ qua việc kiểm tra Audience
            ValidateLifetime = true,    // Kiểm tra thời gian hết hạn của token
            //ClockSkew = TimeSpan.Zero,   // Không có độ trễ cho thời gian hết hạn
            ValidateIssuerSigningKey = true, // Bỏ qua kiểm tra chữ ký của token

            IssuerSigningKey = new RsaSecurityKey(rsa),

        };




#if DEBUG

        // Nếu bạn cần xử lý sự kiện, ví dụ log thông tin token hoặc kiểm tra thêm
        options.Events = new JwtBearerEvents
        {

            //đoạn này dành cho self site, nhưng nếu ở chỗ khác mà không có Cookies thì k bị, nhưng nên xóa đi
            OnMessageReceived = context =>
            {
                // Đọc token từ cookie thay vì header
                var accessToken = context.Request.Cookies["access_token"];
                if (!string.IsNullOrEmpty(accessToken))
                    context.Token = accessToken;

                return Task.CompletedTask;
            },

            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine($"Token validated: {context.SecurityToken}");
                return Task.CompletedTask;
            }

        };

#endif

    });

var app = builder.Build();


var httpContextAccessor = app.Services.GetRequiredService<IHttpContextAccessor>();
CurrentUserHelper.Configure(httpContextAccessor);



//Middleware để tự động refresh token nếu access token hết hạn
//đoạn này dành cho self site, nhưng nếu ở chỗ khác mà không có Cookies thì k bị, nhưng nên xóa đi
app.Use(async (context, next) =>
{
    var accessToken = context.Request.Cookies["access_token"];
    var refreshToken = context.Request.Cookies["refresh_token"];

    var tokenHandler = new JwtSecurityTokenHandler();

    if (!string.IsNullOrEmpty(accessToken))
    {
        var jwtToken = tokenHandler.ReadJwtToken(accessToken);
        var exp = jwtToken.ValidTo;

        // Nếu access token sắp hết hạn (hoặc đã hết)
        if (exp < DateTime.UtcNow.AddMinutes(1) && !string.IsNullOrEmpty(refreshToken))
        {
            var username = CTAuthService.GetUsernameByToken(refreshToken);
            if (username != null)
            {
                // Token hợp lệ, cấp token mới
                var newAccessToken = CTJwtHelper.GenerateAccessToken(username);
                context.Response.Cookies.Append("access_token", newAccessToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(30)
                });
            }
        }
    }

    await next.Invoke();
});



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();   // API

app.Run();
