using CT.Utils;
using MathNet.Numerics.Distributions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;

namespace CT.Usermanager
{
    //class này để phân tách chuỗi thôi vì .net chỉ hỗ trợ vài thuộc tính policy, role..
    public class CustomAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        private readonly AuthorizationOptions _options;

        public CustomAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
            : base(options)
        {
            _options = options.Value;
        }

        public override Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            // Ví dụ: policyName = "scope:read;api:USER_GET"
            var parts = policyName.Split(';');
            var scope = parts.FirstOrDefault(p => p.StartsWith("scope:"))?.Replace("scope:", "");
            var uri = parts.FirstOrDefault(p => p.StartsWith("uri:"))?.Replace("api:", "");


            //scope, uri nếu dùng custom thì cái PermissionRequirement có thể có contructor nhiều param và ko cần đăng ký policy trên propram.cs
            var requirement = new PermissionRequirement();

            var policy = new AuthorizationPolicyBuilder()
                .AddRequirements(requirement)
                .Build();

            return Task.FromResult(policy);
        }
    }



}
