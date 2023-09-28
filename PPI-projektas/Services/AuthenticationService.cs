using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using PPI_projektas.objects;
using PPI_projektas.Utils;
using System.Linq;

namespace PPI_projektas.Services
{
    public class AuthReturn
    {
        public User? user;
        public bool success;
        public string? errorMessage;

        public AuthReturn(User? user, bool success = true, string errorMessage = "")
        {
            this.user = user;
            this.success = success;
            this.errorMessage = errorMessage;
        }
    }
    public class AuthenticationService
    {
        public AuthReturn TryRegister(string name, string password, string email)
        {
            string hashedPassword = hash(password);

            User userExists = DataHandler.Instance.All


            Regex validateGuidRegex = new Regex("^(?=.*?[a-zA-Z])(?=.*?[0-9]).{8,}$"); // atleast one letter, atleast one number and atleast 8 characters long
            Console.WriteLine(validateGuidRegex.IsMatch("-Secr3t."));  


            User newUser = new User(name,password,email);
            DataHandler.Create(newUser);

            return new AuthReturn(newUser,true);
        }


        private string hash(string str)
        {

            byte[] inputBytes = Encoding.UTF8.GetBytes(str);

            using (SHA256 sha256 = SHA256.Create()) {
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                string hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                return hashString;
            }
        }

    }
}
