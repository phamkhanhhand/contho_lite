using CT.Utils;
using MathNet.Numerics.Distributions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Reflection.Emit;
using System.Security.Claims;
using System.Text;

namespace CT.Usermanager
{


    /// <summary>
    /// Chỉ là attribute bình thường,không liên quan đến policy của .net core
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class PermissionAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public string Scope { get; }
        public List<string> ListScope { get; }
        public string Uri { get; }

        public PermissionAttribute(string uri, string scope = "", List<string> listScope = null)
        {
            Scope = scope;
            Uri = uri;
            this.ListScope = listScope;
        }

        //do ke thua IAsyncAuthorizationFilter
        //Middleware → Routing → MVC → Filters (Authorization → Resource → Action → Exception → Result)

        //ASP.NET Core tự động quét các attributes có interface filter như:
        //IAuthorizationFilter / IAsyncAuthorizationFilter
        //IActionFilter / IAsyncActionFilter
        //IExceptionFilter, etc.


        //vì không qua middleware, nên phải tự check token
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var httpContext = context.HttpContext;

            // Lấy header Authorization
            var authHeader = httpContext.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Cắt ra token
            var token = authHeader["Bearer ".Length..].Trim();

            // TODO: xử lý token ở đây
            // Ví dụ: gọi API kiểm tra quyền, decode JWT, lấy claim từ token, v.v.

            // token có thể online hay offline tùy ý
            var isValid = await ValidateTokenAsync(token);
            if (!isValid)
            {
                context.Result = new ForbidResult();
                return;
            }

            // Cho phép qua
            await Task.CompletedTask;
        }


        public async Task<bool> ValidateTokenAsync(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = "your-issuer",         // issuer từ hệ thống cấp token

                ValidateAudience = true,
                ValidAudience = "your-audience",     // audience hợp lệ

                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(1), // cho phép lệch giờ chút xíu

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-secret-or-key"))
            };

            try
            {
                // Nếu token hợp lệ thì nó sẽ không ném exception
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                // Bạn có thể dùng `principal` để lấy claim:
                var scope = principal.FindFirst("scope")?.Value;

                // TODO: Kiểm tra các claim logic tại đây nếu cần

                return true;
            }
            catch
            {
                // Token không hợp lệ
                return false;
            }
        }


        public async Task<bool> ValidateTokenAsyncOnline(string token)
        {
            var httpClient = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Get, "https://auth-server.com/api/check-permission");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

    }
}

 