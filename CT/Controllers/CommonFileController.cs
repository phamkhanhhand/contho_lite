using CT.BL;
//using CT.Integrate.FileCenter;
//using CT.Invoice;
using CT.Models.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using System.Security.Claims;

namespace KH.Usermanager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonFileController : ControllerBase
    {


        // Phương thức để lấy theo ID (có thể được override)
        //testok
        [HttpGet("test")]
        public IActionResult test()
        {
            //DigitalXmlHelper.Test();

            return Ok(1);
        }

        [HttpGet("download/{scid}")]
        public async Task<IActionResult> DownloadFile(string scid)
        {
            //FileCenterManager fileCenterManager = new FileCenterManager();
            //var fileBites= await fileCenterManager.Download(scid,"","","" );


            //// Tạo thông tin phản hồi về file
            //var response = new
            //{
            //    FileContent = fileBites, // Nội dung file sẽ được gửi dưới dạng binary
            //    Status = "1"  // Trạng thái
            //};

            //// Trả về file cùng thông tin bổ sung
            //return Ok(response);
            return Ok(null);
        }
    }
}
