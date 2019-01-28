namespace LocalJsonStore
{
    public interface IAuthService
    {
        string GenerateSalt(int saltLengthMin = 6, int saltLengthMax = 10, string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789");
        string HashPassword(string passwordAndSalt);
        string HashPassword(string password, string salt);
    }
}
