using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using PPI_projektas.Exceptions;
using PPI_projektas.objects;
using PPI_projektas.Services;
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
            if (ownerId == null) return BadRequest("USER-ERROR");
            
            var groupService = new GroupService();

            var groupData = groupService.GetGroupsByOwner(ownerId);
            
            return Ok(groupData);
        }

        [HttpPost("creategroup")]
        public IActionResult CreateGroup([FromBody] GroupCreateData groupData)
        {
            if (groupData == null) return BadRequest("Invalid Data");
            
            Guid groupId;
            try
            {
                var groupService = new GroupService();
                groupId = groupService.CreateGroup(groupData.GroupName, groupData.OwnerId);
            }
            catch (UserDoesNotExistException e)
            {
                return BadRequest("USER-ERROR");
            }
            catch (Exception e)
            {
                return BadRequest();
            }
            
            return CreatedAtAction("CreateGroup", groupId);
        }
    }
    
    public record GroupCreateData
    {
        public string GroupName { get; set; }
        public Guid OwnerId { get; set; }
    }
}