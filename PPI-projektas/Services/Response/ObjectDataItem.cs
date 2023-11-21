namespace PPI_projektas.Services.Response;

public interface IObjectDataItemFactory
{
    ObjectDataItem Create(Guid id, string name);
}

public class ObjectDataItemFactory : IObjectDataItemFactory
{
    public ObjectDataItem Create(Guid id, string name)
    { 
        return new ObjectDataItem(id, name);
    }
}

public record ObjectDataItem(Guid Id, string Name);