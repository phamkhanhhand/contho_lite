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
    public class FlexValueSetsController : CTBaseController<adm_flex_value_sets>
    { 

    }
}
