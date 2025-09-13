
 
using CT.Models.Entity; 
using CT.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace CT.UserContext.CurrentContext
{
    public  static class CurrentUserHelper
    {
        private static IHttpContextAccessor _accessor;
        //private readonly IMemoryCache _cache;//không dùng như này được vì nó depency injection vào constructor => không static, tạo mới thì phải truyền
         
        private static readonly MemoryCacheHelper _memoryCache = new MemoryCacheHelper(new MemoryCache(new MemoryCacheOptions()));

        public static void Configure(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public static string UserName =>
            _accessor?.HttpContext?.User?.Identity?.Name;

        public static string UserId =>
            _accessor?.HttpContext?.User?.FindFirst("sub")?.Value;

         
        public static MyProfile GetCurrentProfileEmployee()
        {
            var username = UserName;


            MyProfile profile = _memoryCache.Get<MyProfile>(username);

            // Kiểm tra xem thông tin của người dùng đã được cache chưa
            if (profile == null)
            {

                profile = new MyProfile();

                // Nếu chưa có trong cache, cần lấy thông tin từ database hoặc service
                var employee = GetEmployeeByUsername(username);

                // Lưu thông tin vào cache để lần sau sử dụng
                //_memoryCache.Set(userId, employeeId, TimeSpan.FromMinutes(30)); // Cache trong 30 phút


                if (employee != null)
                {
                    profile.Employee = employee;
                    profile.EmployeeID = employee.employee_id;
                }

                _memoryCache.Set<MyProfile>(username, profile);

            }



            return profile;
        }
         
        private static adm_Employee GetEmployeeByUsername(string username)
        {

            EmployeeDL dl = new EmployeeDL();

            return dl.GetEmployeeByUsername(username);

        }



    }


}
