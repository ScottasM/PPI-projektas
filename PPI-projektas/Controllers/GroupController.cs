using Microsoft.AspNetCore.Mvc;
using PPI_projektas.objects;
using PPI_projektas.Utils;

namespace PPI_projektas.Controllers

{
    [ApiController]
    [Route("[controller]")]
    public class GroupController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetGroups()
        {
            var groups = DataHandler.LoadList<Group>();
            return Ok(groups);
        }

        [HttpPost]
        public IActionResult CreateGroup(string name)
        {
            // if (owner == null)
            // {
            //     return BadRequest("No creating User");
            // }

            Console.WriteLine("HttpPost");
            
            // var group = new Group(name, owner);
            //
            // var groups = DataHandler.LoadList<Group>();
            // groups.Add(group);
            // DataHandler.SaveList(groups);
            //
            // return CreatedAtAction("GetGroups", new { id = group.Id }, group);
            return CreatedAtAction("GetGroups", 0, 1);
        }
    }
}