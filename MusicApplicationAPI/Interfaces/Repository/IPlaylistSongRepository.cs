using MusicApplicationAPI.Models.DbModels;
using MusicApplicationAPI.Models.DTOs.SongDTO;

namespace MusicApplicationAPI.Interfaces.Repository
{
    public interface IPlaylistSongRepository : IRepository<int, PlaylistSong>
    {
        public Task<IEnumerable<PlaylistSong>> GetPlaylistSongsByPlaylistId(int playlistId);
    }
}
