
 
using CT.Models.Entity; 
using CT.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;


namespace CT.UserContext.CurrentContext
{
    public static class CurrentUserService
    {
        private static readonly MemoryCacheHelper _memoryCache = new MemoryCacheHelper( new MemoryCache( new MemoryCacheOptions()));

        // AsyncLocal sẽ đảm bảo rằng mỗi request sẽ có một HttpContext riêng biệt trong phạm vi luồng xử lý đó
        private static readonly AsyncLocal<IHttpContextAccessor> _httpContextAccessor = new AsyncLocal<IHttpContextAccessor>();


        /// <summary>
        /// Phương thức này được gọi trong middleware để thiết lập HttpContext cho request hiện tại
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public static void SetAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor.Value = httpContextAccessor;
        }





        // Truy cập HttpContext, mỗi request sẽ có giá trị riêng biệt
        public static IHttpContextAccessor Current_IHttpContextAccessor => _httpContextAccessor.Value;





        public static int GetCurrentEmployeeID()
        {
            return 1;
        }
        public static MyProfile GetCurrentProfileEmployee()
        {
            var username = Current_IHttpContextAccessor.HttpContext.User.Identity.Name;


            MyProfile profile =  _memoryCache.Get<MyProfile>(username);

            // Kiểm tra xem thông tin của người dùng đã được cache chưa
            if (profile==null)
            {

                profile = new MyProfile();

                // Nếu chưa có trong cache, cần lấy thông tin từ database hoặc service
                var employee = GetEmployeeByUsername(username);

                // Lưu thông tin vào cache để lần sau sử dụng
                //_memoryCache.Set(userId, employeeId, TimeSpan.FromMinutes(30)); // Cache trong 30 phút


                if (employee != null)
                {
                    profile.Employee = employee;
                    profile.EmployeeID = employee.EmployeeID;
                }

                _memoryCache.Set<MyProfile>(username, profile);

            }



            return profile;
        }

        //private static Employee GetEmployeeByID(int employeeID)
        //{

        //    EmployeeBL bl = new EmployeeBL();

        //    return bl.GetDataByID<Employee>(employeeID); // Giả sử lấy từ DB
        //}

        private static adm_Employee GetEmployeeByUsername(string username)
        {

            EmployeeDL dl = new EmployeeDL();

            return dl.GetEmployeeByUsername(username);
             
        }

    }

}
