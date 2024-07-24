using MusicApplicationAPI.Models.Enums;

namespace MusicApplicationAPI.Interfaces.Service.AuthService
{
    public interface IAuthRegisterService<T, K> where T : class where K : class
    {
        public Task<T> Register(K dto);
    }
}
