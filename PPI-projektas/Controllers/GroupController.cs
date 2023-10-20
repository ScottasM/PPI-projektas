using Microsoft.AspNetCore.Mvc;
using PPI_projektas.Exceptions;
using PPI_projektas.Services;
using PPI_projektas.Services.Response;

namespace PPI_projektas.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupController : ControllerBase
    {
        
        [HttpGet]
        public IActionResult Get(Guid? ownerId)
        {
            if (ownerId == null) return BadRequest("InvalidData");
            
            return Ok(new GroupService().GetGroupsByOwner((Guid) ownerId));
        }

        [HttpGet("group-members/{groupId:guid}")]
        public IActionResult GetGroupMembers(Guid? groupId)
        {
            if (groupId == null) return BadRequest("InvalidData");
            
            try
            {
                return Ok(new GroupService().GetUsersInGroup((Guid)groupId));
            }
            catch (ObjectDoesNotExistException)
            {
                return BadRequest("GROUP-ERROR");
            }
        }

        [HttpPost("creategroup")]
        public IActionResult CreateGroup([FromBody] GroupCreateData? groupData)
        {
            if (groupData == null) return BadRequest("InvalidData");
            
            try
            {
                var groupId = new GroupService().CreateGroup(groupData.OwnerId, groupData.GroupName, groupData.MemberIds);
                return CreatedAtAction("CreateGroup", groupId);
            }
            catch (ObjectDoesNotExistException)
            {
                return BadRequest("USER-ERROR");
            }
        }
        
        //TODO: group edit POST with route "editgroup"

        [HttpDelete("delete/{groupId:guid}")]
        public IActionResult Delete(Guid groupId)
        {
            var groupService = new GroupService();

            try
            {
                groupService.DeleteGroup(groupId);
                return NoContent();
            }
            catch (ObjectDoesNotExistException)
            {
                return NotFound();
            }
        }
    }
    
    public record GroupCreateData
    {
        public string GroupName { get; set; }
        public Guid OwnerId { get; set; }
        public List<Guid> MemberIds { get; set; }
    }
}