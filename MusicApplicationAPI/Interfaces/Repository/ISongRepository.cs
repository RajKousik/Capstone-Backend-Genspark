using MusicApplicationAPI.Models.DbModels;
using MusicApplicationAPI.Models.DTOs.SongDTO;

namespace MusicApplicationAPI.Interfaces.Repository
{
    public interface ISongRepository : IRepository<int, Song>
    {
        public Task<IEnumerable<Song>> GetSongsByArtistId(int artistId);
        public Task<IEnumerable<Song>> GetSongsByAlbumId(int albumId);

        public Task<bool> DeleteRange(IList<int> key);
    }
}
