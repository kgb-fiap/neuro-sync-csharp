using System;
using System.Security.Cryptography;
using System.Text;

namespace NeuroSync.Application.Common
{
    public static class PasswordHasher
    {
        public static string Hash(string plain)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(plain));
            return Convert.ToHexString(bytes);
        }
    }
}
