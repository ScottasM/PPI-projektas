using System;
using System.Security.Cryptography;
using System.Text;
using PPI_projektas.objects;
using PPI_projektas.Utils;

namespace PPI_projektas.Services
{
    public class AuthenticationService
    {
        public User TryRegister(string name, string password, string email)
        {
            string hashedPassword = hash(password);

            User newUser = new User(name,password,email);
            DataHandler.Create(newUser);

            return newUser;
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
