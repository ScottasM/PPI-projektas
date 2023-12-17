using PPI_projektas.objects.Factories;
using PPI_projektas.Services.Interfaces;
using PPI_projektas.Services.Response;
using PPI_projektas.Utils;

namespace PPI_projektas.Services;

public class GroupService : IGroupService
{
    private readonly IObjectDataItemFactory _objectDataItemFactory;
    private readonly IGroupFactory _groupFactory;
    private readonly IGroupPrivilegeDataFactory _groupPrivilegeDataFactory;

    public GroupService(IObjectDataItemFactory objectDataItemFactory, IGroupFactory groupFactory, IGroupPrivilegeDataFactory groupPrivilegeDataFactory)
    {
        _objectDataItemFactory = objectDataItemFactory;
        _groupFactory = groupFactory;
        _groupPrivilegeDataFactory = groupPrivilegeDataFactory;
    }
    
    public List<GroupPrivilegeData> GetGroupsByOwner(Guid ownerId)
    {
        var data = DataHandler.Instance.AllGroups.Values
            //.Where(group => group.OwnerGuid == ownerId) Will be uncommented when user is associated on the frontend
            .Select(group => _groupPrivilegeDataFactory.Create(group.Id, group.Name, group.Owner.Id == ownerId, group.Administrators.Select(admin => admin.Id).Contains(ownerId)))
            .ToList();
        
        return data;
    }

    public List<ObjectDataItem> GetUsersInGroup(Guid groupId)
    {
        var group = DataHandler.FindObjectById(groupId, DataHandler.Instance.AllGroups);

        var users = group.Members
            .Where(user => user != group.Owner)
            .Select(user => _objectDataItemFactory.Create(user.Id, user.GetUsername()))
            .ToList();

        return users;
    }

    public Guid CreateGroup(Guid ownerId, string groupName, IEnumerable<Guid> groupMemberIds, IEnumerable<Guid> groupAdministratorIds)
    {
        var owner = DataHandler.FindObjectById(ownerId, DataHandler.Instance.AllUsers);

        var groupMembers = groupMemberIds.Select(id => DataHandler.FindObjectById(id, DataHandler.Instance.AllUsers)).ToList();

        var group = _groupFactory.Create(groupName, owner, groupMembers);

        foreach (var user in groupMembers)
            user.AddGroup(group);
        
        group.Administrators = groupAdministratorIds
            .Select(id => DataHandler.FindObjectById(id, DataHandler.Instance.AllUsers))
            .ToList();
        
        owner.AddGroup(group);
        
        DataHandler.Create(group);

        return group.Id;
    }
    

    public void EditGroup(Guid groupId, string newName, IEnumerable<Guid> newMemberIds, IEnumerable<Guid> newAdministratorIds, Guid userId)
    {
        var group = DataHandler.FindObjectById(groupId, DataHandler.Instance.AllGroups);

        if (group.Owner.Id != userId && !group.Administrators.Select(user => user.Id).Contains(userId))
            throw new UnauthorizedAccessException();
        
        group.Name = newName;
        
        var newMembers = newMemberIds.Select(id => DataHandler.FindObjectById(id, DataHandler.Instance.AllUsers)).ToList();

        var membersToAdd = newMembers.Where(user => !group.Members.Contains(user)).ToList();
        foreach (var member in membersToAdd)
        {
            group.AddUser(member);
            member.AddGroup(group);
        }
        
        var membersToRemove = group.Members.Where(member => !newMembers.Contains(member) && member != group.Owner).ToList();
        foreach (var member in membersToRemove)
        {
            group.RemoveUser(member);
            member.RemoveGroup(group);
        }

        if (group.Owner.Id == userId)
            group.Administrators = newAdministratorIds
                .Select(id => DataHandler.FindObjectById(id, DataHandler.Instance.AllUsers))
                .ToList();
        
        DataHandler.Instance.SaveChanges();
    }

    public void DeleteGroup(Guid groupId)
    {
        var group = DataHandler.FindObjectById(groupId, DataHandler.Instance.AllGroups);

        foreach (var member in group.Members)
            member.RemoveGroup(group);

        for (int i = group.Notes.Count-1;i>=0; i--)
        {
            var note = group.Notes[i];
            var user = DataHandler.FindObjectById(note.UserId, DataHandler.Instance.AllUsers);
            user.RemoveCreatedNote(note);
            DataHandler.Delete(note);
        }
        
        DataHandler.Delete(group);
    }
}