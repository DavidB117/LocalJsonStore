using System;
using System.Security.Cryptography;
using System.Text;

namespace LocalJsonStore
{
    public class AuthService : IAuthService
    {
        #region Constants
        private const int DEFAULT_SALT_LENGTH_MIN = 6;
        private const int DEFAULT_SALT_LENGTH_MAX = 10;
        private const string CHARACTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        #endregion

        public string GenerateSalt(int saltLengthMin = DEFAULT_SALT_LENGTH_MIN, int saltLengthMax = DEFAULT_SALT_LENGTH_MAX, string characters = CHARACTERS)
        {
            var random = new Random();
            var saltLength = (saltLengthMin != saltLengthMax) ? random.Next(saltLengthMin, saltLengthMax) : saltLengthMin;
            var salt = string.Empty;
            for (var i = 0; i < saltLength; i++)
            {
                salt += characters[random.Next(characters.Length)];
            }
            return salt;
        }

        public string HashPassword(string password)
        {
            return Sha256Hash(password);
        }

        public string HashPassword(string password, string salt)
        {
            return Sha256Hash(password + salt);
        }

        private string Sha256Hash(string str)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(str));
                var sb = new StringBuilder();
                foreach (var b in bytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
