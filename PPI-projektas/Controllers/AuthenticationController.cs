using Microsoft.AspNetCore.Mvc;
using PPI_projektas.objects;
using PPI_projektas.Utils;
using PPI_projektas.Services;

namespace PPI_projektas.Controllers
{
    
    public class AuthenticationController
    {
        AuthenticationService? service;

        public void Register()
        {
            service = LazySingleton<AuthenticationService>.Instance;
        }
    }
}
