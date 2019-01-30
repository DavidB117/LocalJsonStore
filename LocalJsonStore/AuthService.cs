using System;
using System.Text;
using System.Security.Cryptography;

namespace LocalJsonStore
{
    public class AuthService : IAuthService
    {
        private const int _defaultSaltLengthMin = 6;
        private const int _defaultSaltLengthMax = 10;
        private const string _defaultCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private const string _upperHexFormat = "x2";

        public int DEFAULT_SALT_LENGTH_MIN => _defaultSaltLengthMin;
        public int DEFAULT_SALT_LENGTH_MAX => _defaultSaltLengthMax;
        public string DEFAULT_CHARACTERS => _defaultCharacters;

        public string GenerateSalt(int saltLengthMin = _defaultSaltLengthMin, int saltLengthMax = _defaultSaltLengthMax, string characters = _defaultCharacters)
        {
            if (saltLengthMin <= 0) throw new ArgumentException(nameof(saltLengthMin) + " is less than or equal to zero");
            if (saltLengthMax <= 0) throw new ArgumentException(nameof(saltLengthMax) + " is less than or equal to zero");
            if (saltLengthMin > saltLengthMax) throw new ArgumentException(nameof(saltLengthMin) + " is greater than " + nameof(saltLengthMax));
            if (string.IsNullOrWhiteSpace(characters)) throw new ArgumentNullException(nameof(characters) + " is null or white space");

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
            return _sha256Hash(password);
        }

        public string HashPassword(string password, string salt)
        {
            return _sha256Hash(password + salt);
        }

        private string _sha256Hash(string str)
        {
            if (str == null) throw new ArgumentNullException(nameof(str) + " is null");

            using (var sha256 = SHA256.Create())
            {
                var sb = new StringBuilder();
                foreach (var b in sha256.ComputeHash(Encoding.UTF8.GetBytes(str)))
                {
                    sb.Append(b.ToString(_upperHexFormat));
                }
                return sb.ToString();
            }
        }
    }
}
