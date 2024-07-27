using MusicApplicationAPI.Models.DbModels;
using MusicApplicationAPI.Models.DTOs.OtherDTO;
using MusicApplicationAPI.Models.DTOs.UserDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicApplicationAPI.Interfaces.Service
{
    public interface IUserService
    {
        Task<UserReturnDTO> UpdateUserProfile(UserUpdateDTO user, int userId);
        Task<UserReturnDTO> GetUserById(int userId);
        Task<UserReturnDTO> GetUserByEmail(string email);
        Task<IEnumerable<UserReturnDTO>> GetAllUsers();
        Task<UserReturnDTO> DeleteUser(int userId);
        Task<IEnumerable<UserReturnDTO>> GetAllAdminUsers();

        Task<bool> ChangePassword(ChangePasswordRequestDTO requestDTO, int userId);
        Task<PremiumUser> UpgradeUserToPremium(int userId, PremiumRequestDTO premiumRequestDTO);

        Task<bool> DowngradePremiumUser(int userId);

        Task<UserRegisterReturnDTO> AddAdmin(UserRegisterDTO adminRegisterDTO);

        Task<UserReturnDTO> GetAdminById(int adminId);
    }
}
