using MusicApplicationAPI.Models.DbModels;

namespace MusicApplicationAPI.Interfaces.Repository
{
    public interface IPremiumUserRepository : IRepository<int, PremiumUser>
    {
        Task<PremiumUser> GetByUserId(int userId);

        Task<IEnumerable<PremiumUser>> GetUsersWithExpiringSubscriptions(DateTime twoDaysBeforeThreshold, DateTime oneHourBeforeThreshold);
    }
}
