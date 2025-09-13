using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
 
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CT.Auth;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity.Data;
using Azure.Core;

namespace CT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthSameSiteController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] CTLoginRequest request)
        {
            if (request.Username != "admin" || request.Password != "123456")
                return Unauthorized();

            var accessToken = CTJwtHelper.GenerateAccessToken(request.Username);
            var refreshToken = CTJwtHelper.GenerateSecureRefreshToken();

            // Lưu refresh token vào DB (ràng buộc user, device, IP, v.v.)
            CTAuthService.Save(refreshToken, request.Username);

            // Set cookie access token (30 phút)
            Response.Cookies.Append("access_token", accessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(30)
            });

            // Set refresh token cookie (7 ngày)
            Response.Cookies.Append("refresh_token", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

            return Ok(new { message = "Login successful" });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Xoá cookie
            Response.Cookies.Delete("access_token");
            Response.Cookies.Delete("refresh_token");

            // Tùy chọn: revoke refresh token khỏi DB
            return Ok(new { message = "Logged out" });
        }


         


    }

}