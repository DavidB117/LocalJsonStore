using System;
using System.Security.Cryptography;
using System.Text;

namespace LocalJsonStore
{
    public class AuthService : IAuthService
    {
        protected const int _defaultSaltLengthMin = 6;
        protected const int _defaultSaltLengthMax = 10;
        protected const string _defaultCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public int DEFAULT_SALT_LENGTH_MIN => _defaultSaltLengthMin;
        public int DEFAULT_SALT_LENGTH_MAX => _defaultSaltLengthMax;
        public string DEFAULT_CHARACTERS => _defaultCharacters;

        public string GenerateSalt(int saltLengthMin = _defaultSaltLengthMin, int saltLengthMax = _defaultSaltLengthMax, string characters = _defaultCharacters)
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

        public string HashPassword(string passwordAndSalt)
        {
            return Sha256Hash(passwordAndSalt);
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
