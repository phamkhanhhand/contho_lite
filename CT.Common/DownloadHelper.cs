 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CT.Common
{
    /// <summary>
    /// Thông tin người đăng nhập hiện tại, có thể gọi ở bất cứ BL, DL, Controller nào
    /// (Cái này lấy thông tin Headers của request để lấy token)
    /// </summary>
    /// phamkhanhhand 13.05.2023
    public class DownloadHelper
    { 
        public const string CONTENT_TYPE_XLS = "application/vnd.ms-excel";
        public const string CONTENT_TYPE_XLSX = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        public const string CONTENT_TYPE_IMAGE= "application/image/jpeg";
        public const string CONTENT_TYPE_PDF= "application/pdf";
        public const string CONTENT_TYPE_BINARY= "application/octet-stream";
        public const string CONTENT_TYPE_TEXT= "text/plain";
        public const string CONTENT_TYPE_DOCX= "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        public const string CONTENT_TYPE_DOC = "application/msword";
        public const string CONTENT_TYPE_ZIP = "application/x-zip-compressed";


        /// <summary>
        /// Push file content to download
        /// </summary>
        /// <param name="contentType">vd: DownloadHelper.CONTENT_TYPE_XLS</param>
        /// <param name="fileContent"></param>
        /// <param name="displayName"></param>
        public  static void PushToDisplay(HttpResponse response, string contentType, byte[] fileContent, string displayName)
        { 
            // Cấu hình các header HTTP cho file Excel
        
            response.ContentType = contentType;
            response.Headers.Add("Content-Disposition", "attachment;  filename=\"" + displayName.Replace(" ", "_") + "\"");

            // Gửi byte array ra response
            response.Body.Write(fileContent, 0, fileContent.Length);
            response.Body.Flush();
            response.Body.Close(); 
             
            //response.ClearHeaders();
            //response.HeaderEncoding = System.Text.Encoding.UTF8;
            //response.Clear();
            //response.Buffer = true;
            ////response.ContentType = contentType;
            //response.AddHeader("content-disposition", "inline; filename=\"" + displayName.Replace(" ", "_") + "\"");
            //response.AddHeader("Content-Length", fileContent.Length.ToString());
            //response.BinaryWrite(fileContent);
            //response.Flush();
            //response.Close();
        }


        /// <summary>
        /// Push file content to download
        /// </summary>
        /// <param name="contentType">vd: DownloadHelper.CONTENT_TYPE_XLS</param>
        /// <param name="fileContent"></param>
        /// <param name="displayName"></param>
        public static FileContentResult GenerateExcelFile(string contentType, byte[] fileContent, string displayName)
        {

            // Trả về file dưới dạng FileContentResult
            return new FileContentResult(fileContent, contentType)
            {
                FileDownloadName = displayName.Replace(" ", "_")
            };
             

        }
    }
}