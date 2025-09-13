using CT.BL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CT.Auth
{

    public class PermissionFilter : IAsyncAuthorizationFilter
    {
        private readonly string[] listScope;
        private readonly string _apiUri; 

        public PermissionFilter(string[] listScope, string apiCode  )
        {
            this.listScope = listScope;
            _apiUri = apiCode; 
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

             
            #region Check Permission from database

            adm_PermissionBL adm_PermissionBL = new adm_PermissionBL();

            var allowProcess = adm_PermissionBL.CheckPermision(username, listScope.ToList<string>(), this._apiUri);

            #endregion 

            
            if (!allowProcess)
            {
                context.Result = new ForbidResult();
            } 
        }
    }


}

