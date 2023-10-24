using Microsoft.AspNetCore.Mvc;
using PPI_projektas.Exceptions;
using PPI_projektas.Services;
using PPI_projektas.Services.Interfaces;

namespace PPI_projektas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        
        [HttpGet("{name}")]
        public IActionResult Get(string name)
        {
            if (!_userService.ValidateData(name)) return BadRequest("Invalid Data");

            var users = _userService.GetUsersByName(name);
            if (!_userService.ValidateData(users)) return BadRequest("No users found");

            return Ok(users);
        }

        [HttpPost("createuser")]
        public IActionResult CreateUser([FromBody] UserCreateData userData)
        {
            if (!_userService.ValidateData(userData)) return BadRequest("Invalid Data");

            var userGuidId = _userService.CreateUser(userData);

            return CreatedAtAction("CreateUser", userGuidId);
        }

        [HttpDelete("delete/{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                _userService.DeleteUser(id);
                return NoContent();
            }
            catch (ObjectDoesNotExistException)
            {
                return NotFound();
            }  
        }
    }
}


