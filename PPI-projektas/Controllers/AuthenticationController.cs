using Microsoft.AspNetCore.Mvc;
using PPI_projektas.Utils;
using PPI_projektas.Services;
using PPI_projektas.Services.Interfaces;


namespace PPI_projektas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ICustomAuthenticationService _authenticationService;

        public AuthenticationController(ICustomAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        
        [HttpPost("tryregister")]
        public IActionResult Register([FromBody]AuthData authData)
        {
            var authReturn = _authenticationService.TryRegister(authData.Username, authData.Password);
            if(authReturn == null) {
                return BadRequest("Register failed. Please try again later");
            }

            if (!authReturn.Success) {
                return BadRequest(authReturn.ErrorMessage);
            }

            return CreatedAtAction("Register", new { id = authReturn.User.Id, username = authReturn.User.GetUsername() }, authReturn.User.Id);
        }


        [HttpPost("trylogin")]
        public IActionResult Login([FromBody]AuthData authData)
        {
            var authReturn = _authenticationService.TryLogin(authData.Username, authData.Password);
            if (authReturn == null) {
                return BadRequest("Login failed. Please try again later");
            }

            if (!authReturn.Success) {
                return BadRequest(authReturn.ErrorMessage);
            }

            return CreatedAtAction("Login", new { id = authReturn.User.Id, username = authReturn.User.GetUsername() }, authReturn.User.Id);
        }
    }

    public class AuthData
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
