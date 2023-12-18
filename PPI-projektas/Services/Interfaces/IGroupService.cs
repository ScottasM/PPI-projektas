using PPI_projektas.Services.Response;

namespace PPI_projektas.Services.Interfaces;

public interface IGroupService
{
    public List<GroupIconData> GetGroupsByOwner(Guid ownerId);
    public GroupUserData GetUsersInGroup(Guid groupId);
    public Guid CreateGroup(Guid ownerId, string groupName, IEnumerable<Guid> groupMemberIds, IEnumerable<Guid> groupAdministratorIds);
    public void EditGroup(Guid groupId, string newName, IEnumerable<Guid> newMemberIds, IEnumerable<Guid> newAdministratorIds, Guid userId);
    public void DeleteGroup(Guid groupId);
}
