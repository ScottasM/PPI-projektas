namespace PPI_projektas.objects.Factories;

public interface IUserFactory
{
    User Create(string name, string password);
    User Create(string name, string password, string email);
}

public class UserFactory : IUserFactory
{
    public User Create(string name, string password)
    {
        return new User(name, password);
    }

    public User Create(string name, string password, string email)
    {
        return new User(name, password, email);
    }
}