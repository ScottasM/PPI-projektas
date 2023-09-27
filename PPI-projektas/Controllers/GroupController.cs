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
        public IActionResult Get(Guid ownerId)
        {
            var saveHandler = new SaveHandler();
            var groups = saveHandler.LoadList<Group>();
            var groupNames = groups
                //.Where(group => group.OwnerGuid == ownerId) Will be uncommented then user is associated on the frontend
                .Select(group => group.Name)
                .ToList();
            return Ok(groupNames);
        }

        [HttpPost("creategroup")]
        public IActionResult CreateGroup([FromBody] GroupData groupData)
        {
            if (groupData == null) return BadRequest("Invalid Data");

            if (string.IsNullOrEmpty(groupData.OwnerId.ToString())) return BadRequest("OwnerId is empty or null");

            Console.WriteLine("HttpPost " + groupData.GroupName);

            var owner = new User("test", " ", " "); // temporary user for all groups
            DataHandler.Create(owner);
            
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