using PPI_projektas.objects;
using PPI_projektas.Utils;

namespace PPI_projektas.Services.Response;

public interface IPrivilegeDataFactory
{
    PrivilegeData Create(Guid userId, string username, Privilege privilege);
}

public class PrivilegeDataFactory : IPrivilegeDataFactory
{
    public PrivilegeData Create(Guid userId, string username, Privilege privilege)
    {
        return new PrivilegeData(userId, username, privilege);
    }
}

public class PrivilegeData
{
    public User User { get; set; }

    public string Username { get; set; }
    
    public Privilege Privilege { get; set; }

    public PrivilegeData(Guid userId, string username, Privilege privilege)
    {
        User = DataHandler.FindObjectById(userId, DataHandler.Instance.AllUsers);
        Username = username;
        Privilege = privilege;
    }
}

public enum Privilege
{
    Author,
    Administrator,
    Editor,
    Viewer
}