using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CT.Models.ServerObject
{
    /// <summary>
    /// Dữ liệu trả về result
    /// </summary>
    /// phamkhanhhand 14.04.2023
    public class ServiceData
    {

        /// <summary>
        /// Trạng thái kết quả trả về
        /// </summary>
        public CT.Models.Enumeration.ServiceResult Success { get; set; }
        /// <summary>
        /// Thông báo kết quả kèm theo
        /// </summary>
        public string Messenger { get; set; }

        private object data;
        /// <summary>
        /// Dữ liệu trả về
        /// </summary>
        public object Data
        {
            get
            {
                string dataString = String.Empty;
                if (data != null && data.GetType() == typeof(string))
                {
                    dataString = (string)data;
                }
                else
                {
                   //return data;
                    dataString = JsonSerializer.Serialize(data);//mã hóa nó ở đây
                }


                return dataString;

            }
            set
            {
                data = value;
            }
        }

        /// <summary>
        /// Mã lỗi
        /// </summary>
        public string Code { get; set; }


        public List<String> Error { get; set; }
    }
}
