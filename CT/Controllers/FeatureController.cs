using CT.BL;
using CT.Models.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeatureController : CTBaseController<adm_Feature>
    {
        [HttpGet("GetPermisionByFeatureID/{id}")]

        //hển thị các tính năng
        public IActionResult GetPermisionByID(int id)
        {
            adm_FeatureBL feature = new adm_FeatureBL();
            var rs = feature.GetPermisionByFeatureID(id);
            return Ok(rs);
        }
      
    }
}
