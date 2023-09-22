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
            var groups = DataHandler.LoadList<Group>();
            return Ok(groups);
        }

        [HttpPost("creategroup")]
        public IActionResult CreateGroup(string data)
        {
            // if (owner == null)
            // {
            //     return BadRequest("No creating User");
            // }
            
            var obj = JsonSerializer.Deserialize<GroupData>(data);

            Console.WriteLine("HttpPost");
            
            // var group = new Group(name, owner);
            //
            // var groups = DataHandler.LoadList<Group>();
            // groups.Add(group);
            // DataHandler.SaveList(groups);
            //
            // return CreatedAtAction("GetGroups", new { id = group.Id }, group);
            return Ok();
        }
    }

    public class GroupData
    {
        public string GroupName { get; set; }
    }
}