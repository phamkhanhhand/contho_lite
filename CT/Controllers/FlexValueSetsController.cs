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
using CT.Models.ServerObject;
using CT.Models.DTO;

namespace CT.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FlexValueSetsController : CTBaseController<adm_flex_value_sets>
    {

        //1.save link set (for parent, for children) 
        //2.select All parent/children by id 
        //3.select all sets 
        //4.select set (has listchildren, list parent set, list flexvalue)

         
        /// <summary>
        /// add link children, parent to flex value set
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Permission(listScope: [ API_SCOPES.UPDATE, API_SCOPES.CREATE])]
        [HttpPost("save-link")]
        public virtual IActionResult AddLink(ParentChildDTO parentChildDTO)
        {

            var username = CurrentUserHelper.GetCurrentProfileEmployee();


            adm_flex_value_setsBL bl = new adm_flex_value_setsBL();

            var rs = bl.AddLink(parentChildDTO);

            return Ok(rs);

        }
    }
}
