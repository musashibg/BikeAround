using System;
using System.Security.Cryptography;
using System.Text;

namespace BikeAround.Service.Impl
{
    public static class AuthenticationHelper
    {
        private const int SaltLength = 32;

        public static void HashPassword(string password, out string passwordSalt, out string passwordHash)
        {
            // Generate password salt
            var random = new RNGCryptoServiceProvider();
            byte[] saltBytes = new byte[SaltLength];
            random.GetNonZeroBytes(saltBytes);

            passwordSalt = Convert.ToBase64String(saltBytes);
            passwordHash = ComputePasswordHash(password, saltBytes);
        }

        public static string ComputePasswordHash(string password, byte[] saltBytes)
        {
            // Get password bytes with appended salt bytes
            int passwordBytesLength = Encoding.Unicode.GetByteCount(password);
            byte[] saltedPasswordBytes = new byte[passwordBytesLength + SaltLength];
            Encoding.Unicode.GetBytes(password, 0, password.Length, saltedPasswordBytes, 0);
            Array.Copy(saltBytes, 0, saltedPasswordBytes, passwordBytesLength, SaltLength);

            // Hash the salted password bytes
            SHA1 hashAlgorithm = SHA1.Create();
            byte[] passwordHashBytes = hashAlgorithm.ComputeHash(saltedPasswordBytes);

            return Convert.ToBase64String(passwordHashBytes);
        }
    }
}