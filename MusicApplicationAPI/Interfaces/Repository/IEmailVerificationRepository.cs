using MusicApplicationAPI.Models.DbModels;

namespace MusicApplicationAPI.Interfaces.Repository
{
    public interface IEmailVerificationRepository : IRepository<int, EmailVerification>
    {
        Task<EmailVerification> GetByUserId(int userId);
    }
}
