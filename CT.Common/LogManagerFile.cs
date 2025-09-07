
//using CT.Models.Enumeration;
//using CT.Utils;


using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.Common
{
    public class LogManagerFile
    {


        /// <summary>
        /// Ghi log vào bảng log
        /// </summary> 
        /// phamkhanhhand 13.05.2023
        public static void Log(string content, string title)
        {
            Task.Run(() =>
            {
                LogNoThread(content, title);
            });

        }
        // Phương thức ghi log vào file
        public static void LogNoThread(string mess, string title, string fileName = "my_log.txt")
        {
            try
            {

                var logFolder = "kh_log";

                if (!Directory.Exists(logFolder))
                {
                    Directory.CreateDirectory(logFolder);

                }

                // Đường dẫn tới file log
                string logFilePath = logFolder + "\\" + fileName;

                // Mở file để ghi vào cuối file nếu đã tồn tại, nếu không sẽ tạo mới
                using (StreamWriter writer = new StreamWriter(logFilePath, append: true))
                {
                    // Ghi tiêu đề và thời gian vào đầu mỗi log
                    writer.WriteLine("------------ " + title + " ------------");
                    writer.WriteLine("Thời gian: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    writer.WriteLine("Thông điệp: " + mess);
                    writer.WriteLine("---------------------------------------");
                    writer.WriteLine(); // Dòng trống giữa các log
                    writer.Close();
                }

                Console.WriteLine("Log đã được ghi vào file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi ghi log: " + ex.Message);
            }
        }

    }
}
