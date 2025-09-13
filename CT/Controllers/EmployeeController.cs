using CT.BL;
using CT.Auth;
using CT.Models.Entity; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using System.Security.Claims;
using CT.Models.Enumeration;
using CT.UserContext.CurrentContext;

namespace CT.Controllers
{

    [Authorize]
    [Route("api/[controller]")]  
    [ApiController]
    public class EmployeeController : CTBaseController<adm_Employee>
    {



        // Phương thức để lấy theo ID (có thể được override)
        //testok
        [HttpGet("GetEmployeeByUsername")]
        [Permission(uri: "/api/employee/GetEmployeeByUsername", API_SCOPES.VIEW, API_SCOPES.CREATE)]
        public IActionResult GetEmployeeByUsername()
        {
            var username = CurrentUserHelper.GetCurrentProfileEmployee(); 


            adm_EmployeeBL bl = new adm_EmployeeBL();

            var rs = bl.GetEmployeeByUsername("phamha");

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
