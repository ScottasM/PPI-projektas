using PPI_projektas.Exceptions;
using PPI_projektas.objects;
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
        var group = DataHandler.Instance.AllGroups.Find(group => group.Id == groupId);
        if (group == null) throw new ObjectDoesNotExistException(groupId);

        var users = group.Members
            .Select(user => new ObjectDataItem(user.Id, user.GetUsername()))
            .ToList();

        return users;
    }
    
    public Guid CreateGroup(string groupName, Guid ownerId)
    {

        // var owner = DataHandler.Instance.AllUsers.Find(user => user.Id == ownerId);
        // if (owner == null) throw new ObjectDoesNotExistException(ownerId);
        
        // var group = new Group(groupName, owner);
        var group = new Group(groupName, new User());
        DataHandler.Create(group);

        return group.Id;
    }
    

    public void EditGroup(Guid groupId, string? optionalNewName = null, List<User>? optionalNewUsers = null)
    {
        var group = DataHandler.Instance.AllGroups.Find(group => group.Id == groupId);
        if (group == null) throw new ObjectDoesNotExistException(groupId);
        
        if (optionalNewName != null) group.Name = optionalNewName;
        if (optionalNewUsers != null)
        {
            foreach (var user in optionalNewUsers.Where(user => !group.Members.Contains(user)))
                group.AddUser(user);
        }
    }

    public void DeleteGroup(Guid groupId)
    {
        var group = DataHandler.Instance.AllGroups.Find(group => group.Id == groupId);
        if (group == null) throw new ObjectDoesNotExistException(groupId);

        DataHandler.Delete(group);
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