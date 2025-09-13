
using Microsoft.AspNetCore.Mvc.Filters;

namespace CT.Auth
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

 