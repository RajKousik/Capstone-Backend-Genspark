using MusicApplicationAPI.Models.DbModels;
using MusicApplicationAPI.Models.DTOs.PlaylistDTO;

namespace MusicApplicationAPI.Interfaces.Repository
{
    public interface IPlaylistRepository : IRepository<int, Playlist>
    {
        public Task<IEnumerable<Playlist>> GetPlaylistsByUserId(int userId);
    }
}
