using PPI_projektas.Exceptions;
using PPI_projektas.objects;
using PPI_projektas.Utils;

namespace PPI_projektas.Services;

public class GroupService
{
    
    public GroupData GetGroupsByOwner(Guid ownerId)
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

        return groupData;
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

    public void DeleteGroup(Guid groupId)
    {
        var group = DataHandler.Instance.AllGroups.Find(group => group.Id == groupId);
        if (group == null) throw new ObjectDoesNotExistException(groupId);

        DataHandler.Delete(group);
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
}