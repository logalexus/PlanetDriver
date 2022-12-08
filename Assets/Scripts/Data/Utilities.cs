using System;
using System.Security.Cryptography;
using System.Text;

namespace Data
{
    public static class Utilities
    {
        public static string GetHash(string text)  
        {  
            using(var sha256 = SHA256.Create())  
            {  
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));  
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();  
            }  
        }  
    }
}