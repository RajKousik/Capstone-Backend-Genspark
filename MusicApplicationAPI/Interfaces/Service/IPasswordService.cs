namespace MusicApplicationAPI.Interfaces.Service
{
    public interface IPasswordService
    {
        byte[] HashPassword(string password, out byte[] key);
        bool VerifyPassword(string password, byte[] passwordHash, byte[] key);

    }
}
