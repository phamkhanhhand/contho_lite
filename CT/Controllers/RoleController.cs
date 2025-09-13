using CT.BL; 
using CT.Models.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : CTBaseController<adm_Role>
    {



        // Phương thức để lấy theo ID (có thể được override)
        [HttpGet("GetPermisionByFeatureID/{id}")]
        public IActionResult GetPermisionByFeatureID(int id)
        {

            adm_RoleBL bl = new adm_RoleBL();


            var rs = bl.GetPermisionByFeatureID(id);
            return Ok(rs);
        }
        // Phương thức để lấy theo ID 
        [HttpGet("GetPermisionFeatureRole/{id}")]
        public  IActionResult GetPermissionFeatureRole(int id)
        {

            adm_RoleBL bl = new adm_RoleBL();
             

            var rs = bl.GetPermissionFeatureRole(id);
            return Ok(rs);
        }
    }
}
