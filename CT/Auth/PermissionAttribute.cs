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

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class PermissionAttribute : Attribute, IFilterFactory
    {
        public string Scope { get; }
        public string Uri { get; }

        public bool IsReusable => false;

        public PermissionAttribute(string scope, string uri)
        {
            Scope = scope;
            Uri = uri;
        }

        // Đây là chỗ tạo instance PermissionFilter, ASP.NET Core sẽ gọi
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            // Lấy IPermissionService và IUserContext từ DI container
            var permissionService = (IPermissionService)serviceProvider.GetService(typeof(IPermissionService));
            var userContext = (IUserContext)serviceProvider.GetService(typeof(IUserContext));

            // Trả về filter mới với các dependency đã được inject
            return new PermissionFilter(Scope, Uri, permissionService, userContext);
        }
    }

     
}

 