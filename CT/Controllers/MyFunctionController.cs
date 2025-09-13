using CT.BL;
using CT.Models.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyFunctionController : CTBaseController<adm_Function>
    {
        [HttpGet("GetPermissionByFunctionID/{id}")]
        public IActionResult GetPermissionByID(int id)
        {
            adm_FunctionBL function = new adm_FunctionBL();
            var rs = function.GetPermisionByMyFunctionID(id);
            return Ok(rs);
        }
    }
}
