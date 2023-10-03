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
                
                
            var groups = DataHandler.Instance.AllGroups;
            var groupNames = groups
                //.Where(group => group.OwnerGuid == ownerId) Will be uncommented when user is associated on the frontend
                .Select(group => group.Name)
                .ToList();
            var groupIds = groups
                //.Where(group => group.OwnerGuid == ownerId) Will be uncommented when user is associated on the frontend
                .Select(group => group.Id)
                .ToList();
            
            var groupData = new GroupData(groupNames, groupIds);
            
            return Ok(groupData);
        }

        [HttpPost("creategroup")]
        public IActionResult CreateGroup([FromBody] GroupCreateData groupData)
        {
            if (groupData == null) return BadRequest("Invalid Data");

            if (string.IsNullOrEmpty(groupData.OwnerId.ToString())) return BadRequest("OwnerId is empty or null");

            var owner = new User("test", " ", " "); // temporary user for all groups
            DataHandler.Create(owner);
            
            var group = new Group(groupData.GroupName, owner);
            DataHandler.Create(group);
            
            return CreatedAtAction("CreateGroup", new { id = group.Id }, group);
        }
    }

    public class GroupDataItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class GroupData
    {
        public List<GroupDataItem> Groups { get; set; }

        public GroupData(List<string> groupNames, List<Guid> groupIds)
        {
            Groups = new List<GroupDataItem>();

            for (var i = 0; i < Math.Min(groupNames.Count, groupIds.Count); i++)
            {
                Groups.Add(new GroupDataItem
                {
                    Id = groupIds[i],
                    Name = groupNames[i]
                });
            }
        }
    }
    
    public record GroupCreateData
    {
        public string GroupName { get; set; }
        public Guid OwnerId { get; set; }
    }
}