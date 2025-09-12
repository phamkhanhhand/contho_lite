using CT.Usermanager;
using Microsoft.AspNetCore.Authorization;
using System.Net.NetworkInformation; 
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();

builder.Services.AddScoped<IUserContext, UserContext>();

builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IPermissionService, CachedPermissionService>();

 

// Cấu hình xác thực với JWT Token, không cần chữ ký luôn vì gateway đã có chữ ký rồi
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
