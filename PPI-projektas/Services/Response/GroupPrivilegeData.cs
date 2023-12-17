namespace PPI_projektas.Services.Response;


public interface IGroupPrivilegeDataFactory
{
    GroupPrivilegeData Create(Guid id, string name, bool isOwner, bool isAdministrator);
}

public class GroupPrivilegeDataFactory : IGroupPrivilegeDataFactory
{
    public GroupPrivilegeData Create(Guid id, string name, bool isOwner, bool isAdministrator)
    {
        return new GroupPrivilegeData(id, name, isOwner, isAdministrator);
    }
}

public class GroupPrivilegeData
{
    public Guid Id { get; set; }

    public string Name { get; set; } = "";
        
    public bool IsOwner { get; set; }
        
    public bool IsAdministrator { get; set; }

    public GroupPrivilegeData(Guid id, string name, bool isOwner, bool isAdministrator)
    {
        Id = id;
        Name = name;
        IsOwner = isOwner;
        IsAdministrator = isAdministrator;
    }
}