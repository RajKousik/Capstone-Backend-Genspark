using MusicApplicationAPI.Models.DbModels;

namespace MusicApplicationAPI.Interfaces.Service.TokenService
{
    public interface ITokenService
    {
        public string GenerateToken(User user);

        public string GenerateShortLivedToken(User user);
    }
}
