using PPI_projektas.objects;

namespace PPI_projektas.Services.Response;

public interface IAuthReturnFactory
{
    public AuthReturn Create(User? user, bool success = true, string errorMessage = "");
}

public class AuthReturnFactory : IAuthReturnFactory
{
    public AuthReturn Create(User? user, bool success = true, string errorMessage = "")
    {
        return new AuthReturn(user, success, errorMessage);
    }
}

public class AuthReturn
{
    public User? User;
    public bool Success;
    public string? ErrorMessage;

    public AuthReturn(User? user, bool success = true, string errorMessage = "")
    {
        User = user;
        Success = success;
        ErrorMessage = errorMessage;
    }
}