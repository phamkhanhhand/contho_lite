using CT.BL;
////using CT.Invoice;
using CT.Models.Entity;
using CT.Usermanager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using System.Security.Claims;

namespace KH.Usermanager.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "DynamicPolicy")] 

    //[Authorize(Policy = "RequireEmail")]
    //[Authorize(Roles = "RequireAdministratorRole")] 
    [ApiController]
    public class EmployeeController : BaseController<adm_Employee>
    {



        // Phương thức để lấy theo ID (có thể được override)
        //testok
        [HttpGet("GetEmployeeByUsername")]
        [Permission(scope: "write", uri: "USER_UPDATE")]
        public IActionResult GetEmployeeByUsername()
        {

            adm_EmployeeBL  bl = new adm_EmployeeBL();

            var rs = bl.GetEmployeeByUsername("phamkhanhhand");

            return Ok(rs);
        }


        

        #region Demo thêm mới

        //post (them moi)
        //http://localhost:5194/api/Employee
        //    {
        //    "Fullname": "kdffksdfks", 
        //}

        #endregion


        #region Demo sửa

        //PUT (them moi)
        //http://localhost:5194/api/Employee
        //        {
        //    "Fullname": "suaaa", 
        //    "employeeid": 4
        //}
        #endregion

        #region Xóa
        //DELETE
        //http://localhost:5194/api/Employee/5
        #endregion

        #region lấy chi tiết
        //GET
        //http://localhost:5194/api/Employee/1
        #endregion


        #region lấy danh sách
        //GET
        //http://localhost:5194/api/Employee/list
        #endregion

    }
}
