namespace LocalJsonStore
{
    public interface ICryptService
    {
        string Encrypt(string str, string encryptionKey);
        string Decrypt(string encryptedStr, string encryptionKey);
    }
}
