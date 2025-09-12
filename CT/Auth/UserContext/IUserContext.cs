using CT.Models.Entity;
using CT.Utils;
using MathNet.Numerics.Distributions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Reflection.Emit;
using System.Security.Claims;
using System.Text;

namespace CT.Usermanager
{
    public interface IUserContext
    {
        string UserId { get; set; }
        string Username { get; set; }
        string Email { get; set; }
        string[] Roles { get; set; }
        string[] Scopes { get; set; }
        string AccessToken { get; set; }
    }


}

