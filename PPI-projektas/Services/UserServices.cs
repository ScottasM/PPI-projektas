using PPI_projektas.Exceptions;
using PPI_projektas.objects;
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
        return data != null;
    }
    public bool ValidateData(UserCreateData data)
    {
        return !data.Equals(default(UserCreateData));
    }

    public List<ObjectDataItem> GetUsersByName(string name)
    {
        var users = DataHandler.Instance.AllUsers
            .Where(user => user.GetUsername().Contains(name))
            .Select(user => _objectDataItemFactory.Create(user.Id, user.GetUsername()))
            .ToList();

        return users;
    }

    public Guid CreateUser(UserCreateData userData)
    {
        var newUser = _userFactory.Create(userData.Username, userData.Password, userData.Email);
        DataHandler.Create(newUser);

        return newUser.Id;
    }

    public void DeleteUser(Guid userId)
    {
        var user = DataHandler.Instance.AllUsers.Find(user => user.Id == userId);
        if(user == null) throw new ObjectDoesNotExistException(userId);
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
