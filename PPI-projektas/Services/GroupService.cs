using PPI_projektas.Exceptions;
using PPI_projektas.objects;
using PPI_projektas.objects.abstractions;
using PPI_projektas.Utils;

namespace PPI_projektas.Services;

public class GroupService
{
    
    public List<ObjectDataItem> GetGroupsByOwner(Guid ownerId)
    {
        var data = DataHandler.Instance.AllGroups
            //.Where(group => group.OwnerGuid == ownerId) Will be uncommented when user is associated on the frontend
            .Select(group => new ObjectDataItem(group.Id, group.Name))
            .ToList();
        
        return data;
    }

    public List<ObjectDataItem> GetUsersInGroup(Guid groupId)
    {
        var group = FindObjectById(groupId, DataHandler.Instance.AllGroups);

        var users = group.Members
            .Select(user => new ObjectDataItem(user.Id, user.GetUsername()))
            .ToList();

        return users;
    }
    
    public Guid CreateGroup(Guid ownerId, string groupName)
    {

        // var owner = FindObjectById(ownerId, DataHandler.Instance.AllUsers);

        var members = new List<User>();
        for (var i = 0; i < 3; i++)
        {
            var user = new User($"User + {i}", " ", " ");
            DataHandler.Create(user);
            members.Add(user);
        }
        
        // var group = new Group(groupName, owner);
        var group = new Group(groupName, new User(), members);
        DataHandler.Create(group);

        return group.Id;
    }

    public Guid CreateGroup(Guid ownerId, string groupName, List<Guid> groupMemberIds)
    {
        // var owner = FindObjectById(ownerId, DataHandler.Instance.AllUsers);

        var groupMembers = groupMemberIds.Select(id => FindObjectById(id, DataHandler.Instance.AllUsers)).ToList();

        // var group = new Group(groupName, owner, groupMembers);
        var group = new Group(groupName, new User(), groupMembers);
        DataHandler.Create(group);

        return group.Id;
    }
    

    public void EditGroup(Guid groupId, string? optionalNewName = null, List<User>? optionalNewUsers = null)
    {
        var group = FindObjectById(groupId, DataHandler.Instance.AllGroups);
        
        if (optionalNewName != null) group.Name = optionalNewName;
        if (optionalNewUsers != null)
        {
            foreach (var user in optionalNewUsers.Where(user => !group.Members.Contains(user)))
                group.AddUser(user);
        }
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

    public struct ObjectDataItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ObjectDataItem(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}