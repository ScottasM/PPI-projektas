using PPI_projektas.Services.Response;

namespace PPI_projektas.Services.Interfaces;

public interface IGroupService
{
    public List<ObjectDataItem> GetGroupsByOwner(Guid ownerId);
    public List<ObjectDataItem> GetUsersInGroup(Guid groupId);
    public Guid CreateGroup(Guid ownerId, string groupName, IEnumerable<Guid> groupMemberIds);
    public void EditGroup(Guid groupId, string newName, IEnumerable<Guid> newMemberIds);
    public void DeleteGroup(Guid groupId);
}
