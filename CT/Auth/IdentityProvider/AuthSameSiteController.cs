using Microsoft.AspNetCore.Mvc;
using CT.Auth; 

namespace CT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthSameSiteController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] CTLoginRequest request)
        {
            // 1. Kiểm tra user/pass
            var isPassLogin = CTAuthService.Login(request.Username, request.Password);

            if (!isPassLogin)
            {
                return Unauthorized();
            }

            var accessToken = CTJwtHelper.GenerateAccessToken(request.Username);
            var refreshToken = CTJwtHelper.GenerateSecureRefreshToken();
             

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
                
            // Lưu refresh token vào memory hoặc DB (demo)
            CTAuthService.Save(refreshToken, request.Username);

            return Ok(new
            {
                access_token = accessToken,
                refresh_token = refreshToken
            });

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



        [HttpPost("signin")] 
        public IActionResult Signin([FromBody] CTLoginRequest request)
        {
            var isPassLogin = CTAuthService.Signin(request.Username, request.Password);

            if (isPassLogin)
            {

                return Ok(new { message = request.Username });
            }
            else
            {
                return Unauthorized();
            }
        }


    }

}