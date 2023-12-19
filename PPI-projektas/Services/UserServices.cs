using PPI_projektas.Exceptions;
using PPI_projektas.objects.Factories;
using PPI_projektas.Services.Interfaces;
using PPI_projektas.Services.Response;
using PPI_projektas.Utils;

namespace PPI_projektas.Services;

public class UserService : IUserService
{
    private readonly IObjectDataItemFactory _objectDataItemFactory;
    private readonly IUserFactory _userFactory;

    public UserService(IObjectDataItemFactory objectDataItemFactory, IUserFactory userFactory)
    {
        _objectDataItemFactory = objectDataItemFactory;
        _userFactory = userFactory;
    }
    
    public bool ValidateData(string data)
    {
        return !String.IsNullOrEmpty(data);
    }
    public bool ValidateData<T>(List<T>? data)
    {
        if (data == null) return false;

        return data.Any();
    }
    public bool ValidateData(UserCreateData data)
    {
        return !data.Equals(default(UserCreateData));
    }

    public List<ObjectDataItem> GetUsersByName(string name)
    {
        var users = DataHandler.Instance.AllUsers
            .Where(user => user.GetUsername().ContainsCaseInsensitive(name))
            .OrderByDescending(user => user.GetUsername().ToCharArray().Intersect(name.ToCharArray()).ToList().Count)
            .Select(user => new ObjectDataItem(user.Id, user.GetUsername()))
            .ToList();

        return users;
    }
    
    public List<ObjectDataItem> GetGroupsFromUser(Guid userId)
    {
        var user = DataHandler.FindObjectById(userId, DataHandler.Instance.AllUsers);

        if (user == null)
            throw new ObjectDoesNotExistException();

        var groups = user.Groups.Select(group => _objectDataItemFactory.Create(group.Id, group.Name)).ToList();
        
        return groups;
    }

    public Guid CreateUser(UserCreateData userData)
    {
        var newUser = _userFactory.Create(userData.Username, userData.Password, userData.Email);
        DataHandler.Create(newUser);

        return newUser.Id;
    }

    public void DeleteUser(Guid userId)
    {
        var user = DataHandler.FindObjectById(userId, DataHandler.Instance.AllUsers);

        foreach (var group in user.Groups)
            group.RemoveUser(user);
        
        DataHandler.Delete(user);
    }
}
public struct UserCreateData
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public Guid UserId { get; set; }
}
