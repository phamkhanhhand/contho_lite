using CT.Utils;
using MathNet.Numerics.Distributions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Reflection.Emit;
using System.Security.Claims;
using System.Text;

namespace CT.Usermanager
{
    //1 policy- có nhiều require ment
    //1 requirement - có nhiều handle (nhiều hành động kiểm tra)
    //Đại diện cho điều kiện cần kiểm tra
    public class PermissionRequirement : IAuthorizationRequirement
    {

        public PermissionRequirement()
        {

        }
    }


    //Thực hiện logic kiểm tra requirement
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {

            //TODO dựa theo cấu hình để bật tắt xác thực 

            var skipAuthen = XmlConfigReader.GetAppSettingValue("SkipAuth");
            if (skipAuthen=="1")
            {
                context.Succeed(requirement);

                return Task.CompletedTask;
            }



            //chưa authen thì trả về luôn
            if (!context.User.Identity.IsAuthenticated)
            {
                // Người dùng chưa xác thực
                context.Fail(); // Đánh dấu là thất bại
                return Task.CompletedTask;

            }

            //không cần validate sâu xa nữa, lấy kết quả từ gateway thôi


            var httpContext = context.Resource as HttpContext;
            if (httpContext != null)
            {
                if (httpContext.Request.Headers["ITFamilyPassAuthor"] == (false).ToString())
                {

                    context.Fail(); // Đánh dấu là thất bại
                    httpContext.Response.Headers.Add("X-Authorization-Error", "No author acces data");


                }
                else
                {

                    context.Succeed(requirement);
                }

            }


            return Task.CompletedTask;


        }


        public static string ConvertToPublickey(string keyText)
        {

            // Chuyển chuỗi tiếng Việt thành mảng byte
            byte[] byteArray = Encoding.UTF8.GetBytes(keyText);

            // Mã hóa mảng byte thành chuỗi Base64
            string base64String = Convert.ToBase64String(byteArray);

            // Chuyển đổi Base64 thành định dạng PEM giả lập
            string pemString = ConvertToPem(base64String);

            return pemString;
        }


        // Hàm chuyển đổi Base64 thành định dạng PEM giả lập
        static string ConvertToPem(string base64String)
        {
            // Định dạng PEM bắt đầu và kết thúc với các dòng đặc biệt
            string pemHeader = "-----BEGIN PUBLIC KEY-----\n";
            string pemFooter = "\n-----END PUBLIC KEY-----";

            // Tạo chuỗi PEM với dữ liệu Base64
            string pemContent = base64String;

            // Đảm bảo chuỗi Base64 có độ dài đúng, chia theo các đoạn 64 ký tự
            StringBuilder pemBuilder = new StringBuilder();
            int lineLength = 64;
            for (int i = 0; i < pemContent.Length; i += lineLength)
            {
                pemBuilder.AppendLine(pemContent.Substring(i, Math.Min(lineLength, pemContent.Length - i)));
            }

            // Kết hợp Header, nội dung Base64, và Footer thành định dạng PEM
            //return     pemBuilder.ToString()  ;
            return pemHeader + pemBuilder.ToString() + pemFooter;
        }

    }


}
