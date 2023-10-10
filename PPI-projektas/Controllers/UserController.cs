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

            var users = DataHandler.Instance.AllUsers
                .Where(user => user.GetUsername().Contains(name))
                .Select(user => new SimpleUserData(user.Id, user.GetUsername()))
                .ToList();

            if (users == null) return BadRequest("No users found");

            return Ok(users);
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] UserCreateData userData)
        {
            if(userData == null) return BadRequest("Invalid Data");
           
            var user = new User(userData.Username, userData.Password);
            DataHandler.Create(user);

            return CreatedAtAction("CreateUser", user.Id);
        }

        [HttpDelete("delete/{userId:guid}")]
        public IActionResult Delete(Guid userId)
        {
            var user = DataHandler.Instance.AllUsers.Find(user => user.Id == userId);
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

    //to be moved to user services
    public struct SimpleUserData
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public SimpleUserData(Guid id, string name)
        {
            Id = id;
            Username = name;
        }
    }
}
