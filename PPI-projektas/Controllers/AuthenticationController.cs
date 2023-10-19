using Microsoft.AspNetCore.Mvc;
using PPI_projektas.objects;
using PPI_projektas.Utils;
using PPI_projektas.Services;


namespace PPI_projektas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        AuthenticationService? service;


        [HttpPost("tryregister")]
        public IActionResult Register([FromBody]AuthData authData)
        {
            service = LazySingleton<AuthenticationService>.Instance;
            
            var authReturn = service.TryRegister(authData.Username, authData.Password);
            if(authReturn == null) {
                return BadRequest("Register failed. Please try again later");
            }

            if (!authReturn.Success) {
                return BadRequest(authReturn.ErrorMessage);
            }

            return CreatedAtAction("Register", new { id = authReturn.User.Id, username = authReturn.User.GetUsername() }, authReturn.User);
        }


        [HttpPost("trylogin")]
        public IActionResult Login([FromBody]AuthData authData)
        {
            service = LazySingleton<AuthenticationService>.Instance;

            var authReturn = service.TryLogin(authData.Username, authData.Password);
            if (authReturn == null) {
                return BadRequest("Login failed. Please try again later");
            }

            if (!authReturn.Success) {
                return BadRequest(authReturn.ErrorMessage);
            }

            return CreatedAtAction("Login", new { id = authReturn.User.Id, username = authReturn.User.GetUsername() }, authReturn.User);
        }
    }

    public class AuthData
    {
        public string Username;
        public string Password;
    }
}
