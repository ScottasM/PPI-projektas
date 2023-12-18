using PPI_projektas.Services.Response;

namespace PPI_projektas.Services.Interfaces;

public interface ICustomAuthenticationService
{
    public AuthReturn TryRegister(string name, string password);
    public AuthReturn TryLogin(string name, string password);
}