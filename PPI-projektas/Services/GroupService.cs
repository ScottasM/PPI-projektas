using PPI_projektas.objects.Factories;
using PPI_projektas.Services.Interfaces;
using PPI_projektas.Services.Response;
using PPI_projektas.Utils;

namespace PPI_projektas.Services;

public class GroupService : IGroupService
{
    private readonly IObjectDataItemFactory _objectDataItemFactory;
    private readonly IGroupFactory _groupFactory;

    public GroupService(IObjectDataItemFactory objectDataItemFactory, IGroupFactory groupFactory)
    {
        _objectDataItemFactory = objectDataItemFactory;
        _groupFactory = groupFactory;
    }
    
    public List<ObjectDataItem> GetGroupsByOwner(Guid ownerId)
    {
        var data = DataHandler.Instance.AllGroups.Values
            //.Where(group => group.OwnerGuid == ownerId) Will be uncommented when user is associated on the frontend
            .Select(group => _objectDataItemFactory.Create(group.Id, group.Name))
            .ToList();
        
        return data;
    }

    public List<ObjectDataItem> GetUsersInGroup(Guid groupId)
    {
        var group = DataHandler.FindObjectById(groupId, DataHandler.Instance.AllGroups);

        var users = group.Members
            .Select(user => _objectDataItemFactory.Create(user.Id, user.GetUsername()))
            .ToList();

        return users;
    }

    public Guid CreateGroup(Guid ownerId, string groupName, IEnumerable<Guid> groupMemberIds)
    {
        var owner = DataHandler.FindObjectById(ownerId, DataHandler.Instance.AllUsers);

        var groupMembers = groupMemberIds.Select(id => DataHandler.FindObjectById(id, DataHandler.Instance.AllUsers)).ToList();

        var group = _groupFactory.Create(groupName, owner, groupMembers);

        foreach (var user in groupMembers)
            user.AddGroup(group);
        
        owner.AddGroup(group);
        
        DataHandler.Create(group);

        return group.Id;
    }
    

    public void EditGroup(Guid groupId, string newName, IEnumerable<Guid> newMemberIds, Guid userId)
    {
        var group = DataHandler.FindObjectById(groupId, DataHandler.Instance.AllGroups);

        if (group.OwnerGuid != userId)
            throw new UnauthorizedAccessException();
        
        group.Name = newName;
        
        var newMembers = newMemberIds.Select(id => DataHandler.FindObjectById(id, DataHandler.Instance.AllUsers)).ToList();

        var membersToAdd = newMembers.Where(user => !group.Members.Contains(user)).ToList();
        foreach (var member in membersToAdd)
        {
            group.AddUser(member);
            member.AddGroup(group);
        }

        var membersToRemove = group.Members.Where(member => !newMembers.Contains(member)).ToList();
        foreach (var member in membersToRemove)
        {
            group.RemoveUser(member);
            member.RemoveGroup(group);
        }
        
    }

    public void DeleteGroup(Guid groupId)
    {
        var group = DataHandler.FindObjectById(groupId, DataHandler.Instance.AllGroups);

        foreach (var member in group.Members)
            member.RemoveGroup(group);

        foreach (var note in group.Notes)
        {
            var user = DataHandler.FindObjectById(note.UserId, DataHandler.Instance.AllUsers);
            user.RemoveCreatedNote(note);
            DataHandler.Delete(note);
        }
        
        DataHandler.Delete(group);
    }
}