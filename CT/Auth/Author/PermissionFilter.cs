using Microsoft.AspNetCore.Mvc; 
using Microsoft.AspNetCore.Mvc.Filters; 
using System.IdentityModel.Tokens.Jwt; 
using System.Security.Claims; 

namespace CT.Auth
{

    public class PermissionFilter : IAsyncAuthorizationFilter
    {
        private readonly string _scope;
        private readonly string _apiCode;
        private readonly IPermissionService _permissionService;
        private readonly IUserContext _userContext;

        public PermissionFilter(string scope, string apiCode, IPermissionService permissionService, IUserContext userContext)
        {
            _scope = scope;
            _apiCode = apiCode;
            _permissionService = permissionService;
            _userContext = userContext;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var authHeader = context.HttpContext.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var token = authHeader["Bearer ".Length..].Trim();


            #region Không cần thiết vì lấy ở midware rồi

            // Parse token (không validate chữ ký ở đây nữa, vì midware ở program rồi, ở đây bổ sung vào context và author thôi)
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            var userId = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var username = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var email = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var roles = jwt.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToArray();
            var scopes = jwt.Claims.Where(c => c.Type == "scope").Select(c => c.Value).ToArray();

            #endregion

            #region Kiểm tra quyền api

            // Gán vào UserContext, đúng ra dựa vào token token
            _userContext.UserId = userId ?? "";
            _userContext.Username = username ?? "";
            _userContext.Email = email ?? "";
            _userContext.Roles = roles;
            _userContext.Scopes = scopes;
            _userContext.AccessToken = token;
            #endregion

            // Kiểm tra quyền qua service
            var permissions = await _permissionService.GetPermissionsAsync(token);

            //todo
            var daa_userContext = await _permissionService.GetDataContext(token, username);

            if (!permissions.Contains($"{_scope}:{_apiCode}"))
            {
                context.Result = new ForbidResult();
            }
        }
    }


}

