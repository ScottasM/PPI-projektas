using Microsoft.AspNetCore.Mvc;
using PPI_projektas.Exceptions;
using PPI_projektas.Services.Interfaces;

namespace PPI_projektas.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }
        
        [HttpGet]
        public IActionResult Get(Guid? ownerId)
        {
            if (ownerId == null) return BadRequest("InvalidData");
            
            return Ok(_groupService.GetGroupsByOwner((Guid) ownerId));
        }

        [HttpGet("group-members/{groupId:guid}")]
        public IActionResult GetGroupMembers(Guid? groupId)
        {
            if (groupId == null) return BadRequest("InvalidData");
            
            try
            {
                return Ok(_groupService.GetUsersInGroup((Guid)groupId));
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
                var groupId = _groupService.CreateGroup(groupData.Id, groupData.GroupName, groupData.MemberIds);
                return CreatedAtAction("CreateGroup", groupId);
            }
            catch (ObjectDoesNotExistException)
            {
                return BadRequest("USER-ERROR");
            }
        }
        
        [HttpPut("editgroup")]
        public IActionResult EditGroup([FromBody] GroupCreateData? groupData)
        {
            if (groupData == null) return BadRequest("InvalidData");
            
            try
            {
                _groupService.EditGroup(groupData.Id, groupData.GroupName, groupData.MemberIds);
                return Ok();
            }
            catch (ObjectDoesNotExistException)
            {
                return BadRequest("GROUP-ERROR");
            }
        }

        [HttpDelete("delete/{groupId:guid}")]
        public IActionResult Delete(Guid groupId)
        {
            try
            {
                _groupService.DeleteGroup(groupId);
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
        public Guid Id { get; set; }
        public List<Guid> MemberIds { get; set; }
    }
}