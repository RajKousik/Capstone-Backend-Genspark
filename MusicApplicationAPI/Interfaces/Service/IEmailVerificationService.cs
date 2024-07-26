namespace MusicApplicationAPI.Interfaces.Service
{
    public interface IEmailVerificationService
    {
        Task CreateEmailVerification(int userId);
        Task VerifyEmail(int userId, string verificationCode);
    }
}
