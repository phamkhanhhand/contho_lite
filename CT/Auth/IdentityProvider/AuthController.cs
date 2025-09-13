using Microsoft.AspNetCore.Mvc;
using CT.Auth;
using Microsoft.AspNetCore.Authorization;

namespace CT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
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

            var tokenString = CTJwtHelper.GenerateAccessToken(request.Username);

            var refreshToken = CTJwtHelper.GenerateSecureRefreshToken();
            //todo lưu refreshToken vào db

            // Lưu refresh token vào memory hoặc DB (demo)
            CTAuthService.Save(refreshToken, request.Username);

            return Ok(new
            {
                access_token = tokenString,
                refresh_token = refreshToken
            });
        }


        [HttpPost("refresh")]
        public IActionResult RefreshToken([FromBody] CTRefreshRequest model)
        {
            var username = CTAuthService.GetUsernameByToken(model.RefreshToken);
            if (username == null) return Unauthorized();

            var newAccessToken = CTJwtHelper.GenerateAccessToken(username);
            return Ok(new { access_token = newAccessToken });
        }

        [HttpPost("logout")]
        [Authorize] // Nếu bạn yêu cầu token hợp lệ để logout
        public IActionResult Logout([FromBody] CTLogoutRequest request)
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                return Unauthorized();

            // Xoá refresh token khỏi DB hoặc đánh dấu là revoked
            var token = request.RefreshToken;
            CTAuthService.Revoke(token);

            return Ok(new { message = "Logged out successfully" });
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