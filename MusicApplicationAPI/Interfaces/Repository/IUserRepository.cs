using MusicApplicationAPI.Models.DbModels;

namespace MusicApplicationAPI.Interfaces.Repository
{
    public interface IUserRepository : IRepository<int, User>
    {
        Task<User> GetUserByEmail(string email);
    }
}
