using CT.BL.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleEmployeeController : ControllerBase
    {
        [HttpGet("GetRoleByEmployeeID/{id}")]
        [HttpGet("GetEmployeeRole/{id}")]
        [HttpGet("GetEmployeeAllRole/{id}")]

        // hiển thị vai trò của nhân viên qua mã nhân viên
        public IActionResult GetRoleByEmployeeID(int id)
        {
            adm_RoleEmployeeBL roleEmployeeBL = new adm_RoleEmployeeBL();
            var rs = roleEmployeeBL.GetRoleByEmployeeID(id);
            return Ok(rs);
        }

        // hiển thị nhân viên sau khi gán vai trò 
        public IActionResult GetEmployeeRole(int id)
        {
            adm_RoleEmployeeBL roleEmployeeBL = new adm_RoleEmployeeBL();
            var rs = roleEmployeeBL.GetEmployeeRole(id);
            return Ok(rs);
        }

        // hiển thị nhân viên với tất cả vai trò và vai trò tương ứng
        public IActionResult GetEmployeeAllRole(int id)
        {
            adm_RoleEmployeeBL roleEmployeeBL = new adm_RoleEmployeeBL();
            var rs = roleEmployeeBL.GetEmployeeAllRole(id);
            return Ok(rs);
        }
    }
}
