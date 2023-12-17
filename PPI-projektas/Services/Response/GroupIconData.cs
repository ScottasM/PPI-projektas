namespace PPI_projektas.Services.Response;


public interface IGroupIconDataFactory
{
    GroupIconData Create(Guid id, string name, bool isOwner, bool isAdministrator);
}

public class GroupIconDataFactory : IGroupIconDataFactory
{
    public GroupIconData Create(Guid id, string name, bool isOwner, bool isAdministrator)
    {
        return new GroupIconData(id, name, isOwner, isAdministrator);
    }
}

public class GroupIconData
{
    public Guid Id { get; set; }

    public string Name { get; set; } = "";
        
    public bool IsOwner { get; set; }
        
    public bool IsAdministrator { get; set; }

    public GroupIconData(Guid id, string name, bool isOwner, bool isAdministrator)
    {
        Id = id;
        Name = name;
        IsOwner = isOwner;
        IsAdministrator = isAdministrator;
    }
}