using System;

namespace Hmj.Common.Utils
{
    public static class SHA256ManagedUtils
    {
        public static string GetHashedPassword(string salt, string unHashedPassword)
        {
            byte[] passwordAndSaltBytes = System.Text.Encoding.UTF8.GetBytes(unHashedPassword + salt);
            byte[] hashBytes = new System.Security.Cryptography.SHA256Managed().ComputeHash(passwordAndSaltBytes);
            string hashString = Convert.ToBase64String(hashBytes);
            return hashString;
        }
    }
}
