using System;
using System.Security.Cryptography;
using System.Text;

namespace SGM_LOYALTY.Utilities
{
    internal static class Helper
    {
        public static string EncodeString(string originalPassword)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();

            byte[] inputBytes = (new UnicodeEncoding()).GetBytes(originalPassword);
            byte[] hash = sha1.ComputeHash(inputBytes);

            return Convert.ToBase64String(hash);
        }
    }
}