
using CT.Models.Entity;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CT.Auth
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class PermissionAttribute : Attribute, IFilterFactory
    {
        public string[] ListScope { get; }
        public string Uri { get; }

        public bool IsReusable => false;


        public PermissionAttribute( string uri, params string[] listScope)
        { 
            Uri = uri;
            ListScope = listScope ?? new string[0];
        }
         
        // Đây là chỗ tạo instance PermissionFilter, ASP.NET Core sẽ gọi
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            // Lấy IPermissionService và IUserContext từ DI container
            //var permissionService = (IPermissionService)serviceProvider.GetService(typeof(IPermissionService));
            //var userContext = (IUserContext)serviceProvider.GetService(typeof(IUserContext));

            // Trả về filter mới với các dependency đã được inject
            //return new PermissionFilter(ListScope, Uri, permissionService, userContext);
            return new PermissionFilter(ListScope, Uri);
        }
    }

     
}

 