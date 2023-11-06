using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using PPI_projektas.objects;
using PPI_projektas.objects.Factories;
using PPI_projektas.Services.Interfaces;
using PPI_projektas.Services.Response;
using PPI_projektas.Utils;

namespace PPI_projektas.Services
{
    public class AuthenticationService : ICustomAuthenticationService
    {
        private readonly IAuthReturnFactory _authReturnFactory;
        private readonly IUserFactory _userFactory;

        public AuthenticationService(IAuthReturnFactory authReturnFactory, IUserFactory userFactory)
        {
            _authReturnFactory = authReturnFactory;
            _userFactory = userFactory;
        }
        
        public AuthReturn TryRegister(string name, string password)
        {
            var hashedPassword = Hash(password);

            if (DataHandler.userExists(name))
                return _authReturnFactory.Create(null, false, "User already exists.");

            var validateGuidRegex = new Regex("^(?=.*?[a-zA-Z])(?=.*?[0-9]).{8,}$"); // at least one letter, at least one number and at least 8 characters long
            if (!validateGuidRegex.IsMatch("-Secr3t."))
                return _authReturnFactory.Create(null, false, "Invalid password format. Ensure at least 1 letter, 1 number and total length of at least 8 characters");

            var newUser = _userFactory.Create(name, hashedPassword);
            DataHandler.Create(newUser);

            return _authReturnFactory.Create(newUser);
        }

        public AuthReturn TryLogin(string name,string password)
        {
            var user = DataHandler.userExistsObject(name);
            if (user == null)
                return _authReturnFactory.Create(null, false, "User with such username not found.");

            var hashedPassword = Hash(password);
            return hashedPassword != user.GetPassword() ? 
                _authReturnFactory.Create(null, false, "Password is incorrect") 
                : _authReturnFactory.Create(user, true);
        }


        private string Hash(string str)
        {

            var inputBytes = Encoding.UTF8.GetBytes(str);

            using (var sha256 = SHA256.Create()) {
                var hashBytes = sha256.ComputeHash(inputBytes);

                var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                return hashString;
            }
        }

    }
}
