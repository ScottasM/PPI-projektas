using PPI_projektas.Services.Response;

namespace PPI_projektas.Services.Interfaces;

public interface IUserService
{
    public bool ValidateData(string data);
    public bool ValidateData<T>(List<T>? data);
    public bool ValidateData(UserCreateData data);
    public List<ObjectDataItem> GetUsersByName(string name);
    public List<GroupIconData> GetGroupsFromUser(Guid userId);
    public Guid CreateUser(UserCreateData userData);
    public void DeleteUser(Guid userId);
}