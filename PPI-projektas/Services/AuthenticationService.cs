using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using PPI_projektas.objects;
using PPI_projektas.Utils;

namespace PPI_projektas.Services
{
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
    public class AuthenticationService
    {
        public AuthReturn TryRegister(string name, string password)
        {
            var hashedPassword = Hash(password);

            if (DataHandler.userExists(name))
                return new AuthReturn(null, false, "User already exists.");

            var validateGuidRegex = new Regex("^(?=.*?[a-zA-Z])(?=.*?[0-9]).{8,}$"); // at least one letter, at least one number and at least 8 characters long
            if (!validateGuidRegex.IsMatch("-Secr3t."))
                return new AuthReturn(null, false, "Invalid password format. Ensure at least 1 letter, 1 number and total length of at least 8 characters");

            var newUser = new User(name,password);
            DataHandler.Create(newUser);

            return new AuthReturn(newUser,true);
        }

        public AuthReturn TryLogin(string name,string password)
        {
            var user = DataHandler.userExistsObject(name);
            if (user == null)
                return new AuthReturn(null, false, "User with such username not found.");

            var hashedPassword = Hash(password);
            return hashedPassword != user.GetPassword() ? 
                new AuthReturn(null, false, "Password is incorrect") 
                : new AuthReturn(user, true);
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
