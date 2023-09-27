using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using PPI_projektas.objects;
using PPI_projektas.Utils;

namespace PPI_projektas.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var saveHandler = new SaveHandler();
            var groups = saveHandler.LoadList<Group>();
            return Ok(groups);
        }

        [HttpPost("creategroup")]
        public IActionResult CreateGroup([FromBody] GroupData groupData)
        {
            if (groupData == null) return BadRequest("Invalid Data");

            if (string.IsNullOrEmpty(groupData.OwnerId.ToString())) return BadRequest("OwnerId is empty or null");

            Console.WriteLine("HttpPost " + groupData.GroupName);

            var owner = new User("test", " ", " ");
            
            var group = new Group(groupData.GroupName, owner);
            DataHandler.Create(group);
            
            return CreatedAtAction("CreateGroup", new { id = group.Id }, group);
        }
    }

    public class GroupData
    {
        public string GroupName { get; set; }
        public Guid OwnerId { get; set; }
    }
}