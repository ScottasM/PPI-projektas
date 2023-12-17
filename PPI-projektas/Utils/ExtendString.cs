using System.Text.RegularExpressions;

namespace PPI_projektas.Utils
{
    public static class ExtendString
    {

        private static List<string> profanityFilter = new List<string>() {
            "idiot",
            "ass",
            "fuck",
            "nazi" // write some more later
        };

        private static List<string> commonPasswords = new List<string>() {
            "password",
            "123456",
            "qwerty",
            "abc123",
            "letmein",
            "admin",
            "welcome",
            "123123",
            "password1",
            "123qwe",
            "qwerty123",
            "admin123",
            "test123",
            "login",
            "changeme",
            "iloveyou",
            "sunshine"
            // Add more common passwords here
        };

        private static string pattern = @"[^a-zA-Z0-9]";


        // returns null if the string is good to go, and error message if it is not.

        // example : password.ValidateString(checkProfanity:true, minLength:8, checkCommonPasswords:true);
        public static string? ValidateString(this string str, 
            bool checkProfanity = true, 
            bool checkSpecialCharacters = false, 
            int minLength = 0, 
            int maxLength = 0, 
            bool checkCommonPasswords = false) 
        {


            
            if (checkCommonPasswords) 
                if (commonPasswords.Any(s => str.Contains(s)))
                    return "Really? This? Too easy to guess";
            
            if (checkSpecialCharacters) 
                if (Regex.IsMatch(str, pattern))
                    return "You can't use special characters here.";
            
            if(minLength > 0) 
                if(str.Length <  minLength) 
                    return $"Minimum length is : {minLength}";
                
            if(maxLength > 0) 
                if(str.Length > maxLength) 
                    return $"Maximum length is : {maxLength}";
            return null;
        }
    }
}
