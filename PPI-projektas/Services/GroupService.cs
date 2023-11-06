using PPI_projektas.Exceptions;
using PPI_projektas.objects;
using PPI_projektas.objects.abstractions;
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
        var data = DataHandler.Instance.AllGroups
            //.Where(group => group.OwnerGuid == ownerId) Will be uncommented when user is associated on the frontend
            .Select(group => _objectDataItemFactory.Create(group.Id, group.Name))
            .ToList();
        
        return data;
    }

    public List<ObjectDataItem> GetUsersInGroup(Guid groupId)
    {
        var group = FindObjectById(groupId, DataHandler.Instance.AllGroups);

        var users = group.Members
            .Select(user => _objectDataItemFactory.Create(user.Id, user.GetUsername()))
            .ToList();

        return users;
    }

    public Guid CreateGroup(Guid ownerId, string groupName, IEnumerable<Guid> groupMemberIds)
    {
        // var owner = FindObjectById(ownerId, DataHandler.Instance.AllUsers);

        var groupMembers = groupMemberIds.Select(id => FindObjectById(id, DataHandler.Instance.AllUsers)).ToList();

        // var group = _groupFactory.Create(groupName, owner, groupMembers);
        var group = _groupFactory.Create(groupName, new User(), groupMembers);
        DataHandler.Create(group);

        return group.Id;
    }
    

    public void EditGroup(Guid groupId, string newName, IEnumerable<Guid> newMemberIds)
    {
        var group = FindObjectById(groupId, DataHandler.Instance.AllGroups);

        group.Name = newName;
        
        var newMembers = newMemberIds.Select(id => FindObjectById(id, DataHandler.Instance.AllUsers)).ToList();

        var membersToAdd = newMembers.Where(user => !group.Members.Contains(user)).ToList();
        foreach (var member in membersToAdd)
            group.AddUser(member);

        var membersToRemove = group.Members.Where(member => !newMembers.Contains(member)).ToList();
        foreach (var member in membersToRemove)
            group.RemoveUser(member);
        
        new SaveHandler().SaveObject(group);
    }

    public void DeleteGroup(Guid groupId)
    {
        var group = FindObjectById(groupId, DataHandler.Instance.AllGroups);

        DataHandler.Delete(group);
    }
    
    private T FindObjectById<T>(Guid objectId, List<T> objectList) where T : Entity
    {
        var obj = objectList.Find(obj => obj.Id == objectId);
        if (obj == null) throw new ObjectDoesNotExistException(objectId);

        return obj;
    }
}