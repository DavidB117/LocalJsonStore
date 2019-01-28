namespace LocalJsonStore
{
    public interface IAuthService
    {
        int DEFAULT_SALT_LENGTH_MIN { get; }
        int DEFAULT_SALT_LENGTH_MAX { get; }
        string DEFAULT_CHARACTERS { get; }
        string GenerateSalt(int saltLengthMin = 6, int saltLengthMax = 10, string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789");
        string HashPassword(string passwordAndSalt);
        string HashPassword(string password, string salt);
    }
}
