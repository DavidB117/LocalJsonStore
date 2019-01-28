namespace LocalJsonStore
{
    public interface IAuthService
    {
        string GenerateSalt(int saltLengthMin, int saltLengthMax, string characters);
        string HashPassword(string passwordAndSalt);
        string HashPassword(string password, string salt);
    }
}
