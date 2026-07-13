using System;
using System.Security.Cryptography;
using System.Text;
namespace Shared
{
    public static class clsSecurityHelper
    {
        public static string ComputeHash(string password, string salt, int iterations = 10000)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltBytes = Convert.FromBase64String(salt);

            // New static method for PBKDF2 hashing in .NET 10
            byte[] hashBytes = Rfc2898DeriveBytes.Pbkdf2(passwordBytes, saltBytes, iterations, HashAlgorithmName.SHA256, 32);
    
            return Convert.ToBase64String(hashBytes);
        }

        public static string GenerateSalt(int size = 16)
        {
            byte[] saltBytes = new byte[size];
            using (var provider = RandomNumberGenerator.Create())
            {
                provider.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }
    }
}