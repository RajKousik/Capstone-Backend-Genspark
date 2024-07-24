using MusicApplicationAPI.Models.DTOs.UserDTO;

namespace MusicApplicationAPI.Interfaces.Service.AuthService
{
    public interface IAuthLoginService<T, K> where T : class where K : class
    {
        public Task<T> Login(K dto);
    }
}
