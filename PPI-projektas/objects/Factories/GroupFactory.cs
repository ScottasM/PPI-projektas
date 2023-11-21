namespace PPI_projektas.objects.Factories;

public interface IGroupFactory
{
    Group Create(string name, User owner, List<User> members);
}

public class GroupFactory : IGroupFactory
{
    public Group Create(string name, User owner, List<User> members)
    {
        return new Group(name, owner, members);
    }
}