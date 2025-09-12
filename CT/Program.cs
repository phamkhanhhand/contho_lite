using CT.Usermanager;
using Microsoft.AspNetCore.Authorization;
using System.Net.NetworkInformation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();

builder.Services.AddScoped<IUserContext, UserContext>();

builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IPermissionService, CachedPermissionService>();


//bắt buộc phải có 2 cái này đi với nhau

builder.Services.AddAuthorization(options =>
{ 

    //policy này 1 là fix cứng, 2 là không truyền tham số, nên ít dùng

    //định nghĩa quy tắc, luật chơi
    options.AddPolicy("DynamicPolicy", policy => policy.Requirements.Add(new PermissionRequirement()));
    //PermissionRequirement Là điều kiện cụ thể bên trong quy tắc. Giống như "Bạn phải đủ 18 tuổi"
});

//như là cảnh sát/bảo vệ kiểm tra điều kiện
builder.Services.AddSingleton<IAuthorizationHandler, PermissionHandler>();


//Để phân tách, chặt chém api từ controller truyền vào, để truyền dạng chuỗi
builder.Services.AddSingleton<IAuthorizationPolicyProvider,CustomAuthorizationPolicyProvider>();



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
