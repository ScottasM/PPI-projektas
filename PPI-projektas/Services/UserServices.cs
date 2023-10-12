using PPI_projektas.Exceptions;
using PPI_projektas.objects;
using PPI_projektas.Utils;

namespace PPI_projektas.Services;

public class UserService
{
    public bool ValidateData(string data)
    {
        return !String.IsNullOrEmpty(data);
    }
    public bool ValidateData<T>(List<T> data)
    {
        if (data == null) return false;
        else return true;
    }
    public bool ValidateData(UserCreateData data)
    {
        return !data.Equals(default(UserCreateData));
    }

    public List<SimpleUserData> GetUsersByName(string name)
    {
        var users = DataHandler.Instance.AllUsers
            .Where(user => user.GetUsername().Contains(name))
            .Select(user => new SimpleUserData(user.Id, user.GetUsername()))
            .ToList();

        return users;
    }

    public Guid CreateUser(UserCreateData userData)
    {
        var newUser = new User(userData.Username, userData.Password, userData.Email);
        DataHandler.Create(newUser);

        return newUser.Id;
    }

    public Guid DeleteUser(Guid userId)
    {
        var user = DataHandler.Instance.AllUsers.Find(user => user.Id == userId);
        if(user == null) throw new ObjectDoesNotExistException(userId);
        DataHandler.Delete(user);

        return userId;
    }

    public struct SimpleUserData
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public SimpleUserData(Guid id, string name)
        {
            Id = id;
            Username = name;
        }
    }
}
public struct UserCreateData
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public Guid UserId { get; set; }
}
