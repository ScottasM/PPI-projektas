using Microsoft.AspNetCore.Mvc;
using PPI_projektas.objects;
using PPI_projektas.Utils;

namespace PPI_projektas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        [HttpGet]
        public IActionResult Get(string name)
        {
            if (name == null) return BadRequest("Invalid Data");
            var users = DataHandler.Instance._allUsers
                .Where(user => user._username.Contains(name));

            return Ok(users);
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] UserCreateData userData)
        {
            if(userData == null) return BadRequest("Invalid Data");
            //if (DataHandler.Instance._allUsers.Find(user => user.Id == userData.UserId) == null) return BadRequest("User already exists");
           
            var user = new User(userData.Username, userData.Password, userData.Email);
            DataHandler.Create(user);

            Guid UserId = user.Id;

            return CreatedAtAction(nameof(userData), UserId);
        }

        [HttpDelete("delete/{userId:guid}")]
        public IActionResult Delete(Guid userId)
        {
            var user = DataHandler.Instance._allUsers.Find(user => user.Id == userId);
            if (user == null) return BadRequest("User does not exist");
            DataHandler.Delete(user);

            return NoContent();
        }
    }

    public record UserCreateData
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public Guid UserId { get; set; }
    }
}
