using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Linq;
using System.Collections.Generic;

namespace LocalJsonStore
{
    public class CryptService : ICryptService
    {
        private const string _padding = "00000000000000000000000000000000";
        private byte[] _processEncryptionKey(string s)
        {
            return _getSubArray(Encoding.UTF8.GetBytes(s + _padding), 0, KEY_BYTE_LENGTH);
        }
        private T[] _getSubArray<T>(T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        protected const CipherMode CIPHER_MODE = CipherMode.CBC;
        protected const int BLOCK_SIZE = 128;
        protected const int KEY_BYTE_LENGTH = 32;
        protected const int IV_BYTE_LENGTH = 16;

        public string Encrypt(string str, string encryptionKey)
        {
            if (string.IsNullOrWhiteSpace(str)) throw new ArgumentNullException(nameof(str) + " is null or white space");
            if (string.IsNullOrWhiteSpace(encryptionKey)) throw new ArgumentNullException(nameof(encryptionKey) + " is null or white space");

            byte[] encrypted;
            using (var rm = new RijndaelManaged())
            {
                rm.BlockSize = BLOCK_SIZE;
                rm.Mode = CIPHER_MODE;
                rm.Key = _processEncryptionKey(encryptionKey);
                rm.GenerateIV();
                encrypted = rm.IV;
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, rm.CreateEncryptor(rm.Key, rm.IV), CryptoStreamMode.Write))
                    {
                        using (var sw = new StreamWriter(cs))
                        {
                            sw.Write(str);
                        }
                        encrypted = encrypted.Concat(ms.ToArray()).ToArray();
                    }
                }
            }
            return Convert.ToBase64String(encrypted);
        }

        public string Decrypt(string encryptedStr, string encryptionKey)
        {
            if (string.IsNullOrWhiteSpace(encryptedStr)) throw new ArgumentNullException(nameof(encryptedStr) + " is null or white space");
            if (string.IsNullOrWhiteSpace(encryptionKey)) throw new ArgumentNullException(nameof(encryptionKey) + " is null or white space");

            var temp = Convert.FromBase64String(encryptedStr);

            var iv = new List<byte>();
            var data = new List<byte>();
            for (var i = 0; i < temp.Length; i++)
            {
                if (i < IV_BYTE_LENGTH) iv.Add(temp[i]);
                else data.Add(temp[i]);
            }

            var decryptedString = string.Empty;
            using (var rm = new RijndaelManaged())
            {
                rm.BlockSize = BLOCK_SIZE;
                rm.Mode = CIPHER_MODE;
                rm.Key = _processEncryptionKey(encryptionKey);
                rm.IV = iv.ToArray();
                using (var ms = new MemoryStream(data.ToArray()))
                {
                    using (var cs = new CryptoStream(ms, rm.CreateDecryptor(rm.Key, rm.IV), CryptoStreamMode.Read))
                    {
                        using (var sr = new StreamReader(cs))
                        {
                            decryptedString = sr.ReadToEnd();
                        }
                    }
                }
            }
            return decryptedString;
        }
    }
}
