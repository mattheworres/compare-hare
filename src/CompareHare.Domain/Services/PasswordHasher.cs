using System;
using System.Security.Cryptography;

namespace CompareHare.Domain.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltLength = 16;
        private const int HashLength = 32;

        public void SetPassword(string password, IPasswordContainer passwordContainer)
        {
            using (var hasher = new Rfc2898DeriveBytes(password, SaltLength))
            {
                passwordContainer.PasswordSalt = Convert.ToBase64String(hasher.Salt);
                passwordContainer.PasswordHash = Convert.ToBase64String(hasher.GetBytes(HashLength));
            }
        }

        public string HashPassword(string password, IPasswordContainer passwordContainer)
        {
            return ComputeHash(password, passwordContainer.PasswordSalt);
        }

        private string ComputeHash(string password, string salt)
        {
            using (var hasher = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt)))
            {
                return Convert.ToBase64String(hasher.GetBytes(HashLength));
            }
        }
    }
}
