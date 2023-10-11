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
            
            AuthReturn authReturn = service.TryRegister(authData.Username, authData.Password);
            if(authReturn == null) {
                return BadRequest("Register failed. Please try again later");
            }

            if (!authReturn.success) {
                return BadRequest(authReturn.errorMessage);
            }

            return CreatedAtAction("Register", new { id = authReturn.user.Id, username = authReturn.user.GetUsername() }, authReturn.user);
        }


        [HttpPost("trylogin")]
        public IActionResult Login([FromBody]AuthData authData)
        {
            service = LazySingleton<AuthenticationService>.Instance;

            AuthReturn authReturn = service.TryLogin(authData.Username, authData.Password);
            if (authReturn == null) {
                return BadRequest("Login failed. Please try again later");
            }

            if (!authReturn.success) {
                return BadRequest(authReturn.errorMessage);
            }

            return CreatedAtAction("Login", new { id = authReturn.user.Id, username = authReturn.user.GetUsername() }, authReturn.user);
        }
    }

    public class AuthData
    {
        public string Username;
        public string Password;
    }
}
