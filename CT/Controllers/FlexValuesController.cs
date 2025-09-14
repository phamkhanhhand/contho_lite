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
using CT.Models.DTO;

namespace CT.Controllers
{

    [Authorize]
    [Route("api/[controller]")]  
    [ApiController]
    public class FlexValuesController : CTBaseController<adm_flex_values>
    {


        //Save link set (for parent, for children): value/set
        //select All parent/children by id
        //select all flexvalue may be parent/children 



        /// <summary>
        /// add link children, parent to flex value set
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Permission(listScope: [API_SCOPES.UPDATE, API_SCOPES.CREATE])]
        [HttpPost("save-link")]
        public virtual IActionResult AddLink(ParentChildDTO linkset)
        {

            var username = CurrentUserHelper.GetCurrentProfileEmployee();


            adm_flex_valuesBL bl = new adm_flex_valuesBL();

            var rs = bl.AddLink(linkset);

            return Ok(rs);

        }


    }
}
