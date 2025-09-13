using CT.BL; 
using CT.Models.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {


        // Phương thức để lấy tất cả và phân trang
        //testok
        [HttpGet("GetAllMenuByCurrentUser")]//thêm list gọi qua gateway cho dễ
        public virtual IActionResult GetAllMenuByCurrentUser(int page = 1, int pageSize = 10)
        {

             var menuBL = new MenuBL();

            var rs = menuBL.GetAllMenuByCurrentUser();
            return Ok(rs);
        }

        // Phương thức để lấy tất cả và phân trang
        //testok
        [HttpGet("GetAllPermisionByFeatureByCurrentUser")]//thêm list gọi qua gateway cho dễ
        public virtual IActionResult GetAllPermisionByFeatureByCurrentUser(string featureCode)
        {

            var menuBL = new MenuBL();

            var rs = menuBL.GetAllPermisionByFeatureByCurrentUser(featureCode);
            return Ok(rs);
        }
    }
}
