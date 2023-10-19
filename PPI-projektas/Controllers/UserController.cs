using Microsoft.AspNetCore.Mvc;
using PPI_projektas.Exceptions;
using PPI_projektas.Services;

namespace PPI_projektas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {

        [HttpGet("{name}")]
        public IActionResult Get(string name)
        {
            var userService = new UserService();
            if (!userService.ValidateData(name)) return BadRequest("Invalid Data");

            var users = userService.GetUsersByName(name);
            if (!userService.ValidateData(users)) return BadRequest("No users found");

            return Ok(users);
        }

        [HttpPost("createuser")]
        public IActionResult CreateUser([FromBody] UserCreateData userData)
        {
            var userService = new UserService();
            if (!userService.ValidateData(userData)) return BadRequest("Invalid Data");

            var userGuidId = userService.CreateUser(userData);

            return CreatedAtAction("CreateUser", userGuidId);
        }

        [HttpDelete("delete/{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var userService = new UserService();
            try
            {
                var userGuidId = userService.DeleteUser(id);
                return NoContent();
            }
            catch (ObjectDoesNotExistException)
            {
                return NotFound();
            }  
        }
    }
}


