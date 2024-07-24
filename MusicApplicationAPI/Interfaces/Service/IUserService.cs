using MusicApplicationAPI.Models.DbModels;
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
    }
}
