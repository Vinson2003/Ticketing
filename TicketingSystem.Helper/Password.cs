using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace TicketingSystem.Helper
{
    public static class PasswordHelper
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100000;

        // Hash password menggunakan PBKDF2
        public static string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256,
                HashSize
            );

            return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        public static bool IsHashedPassword(string password)
        {
            var parts = password.Split('.');

            return parts.Length == 3 &&
                   int.TryParse(parts[0], out _);
        }

        // Verifikasi password dengan membandingkan hash yang disimpan
        public static bool VerifyPassword(string password, string storedPassword)
        {
            var parts = storedPassword.Split('.');

            if (parts.Length != 3)
            {
                return false;
            }

            int iterations = int.Parse(parts[0]);
            byte[] salt = Convert.FromBase64String(parts[1]);
            byte[] storedHash = Convert.FromBase64String(parts[2]);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations,
                HashAlgorithmName.SHA256,
                storedHash.Length
            );

            return CryptographicOperations.FixedTimeEquals(hash, storedHash);
        }
    }
}
