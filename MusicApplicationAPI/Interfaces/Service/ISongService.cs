using MusicApplicationAPI.Models.DTOs.SongDTO;

namespace MusicApplicationAPI.Interfaces.Service
{
    /// <summary>
    /// Interface to define methods for song-related operations.
    /// </summary>
    public interface ISongService
    {
        Task<SongReturnDTO> AddSong(SongAddDTO songCreateDTO);
        Task<SongReturnDTO> UpdateSong(int songId, SongUpdateDTO songUpdateDTO);
        Task<SongReturnDTO> GetSongById(int songId);
        Task<IEnumerable<SongReturnDTO>> GetAllSongs();
        Task<SongReturnDTO> DeleteSong(int songId);

        Task<IEnumerable<SongReturnDTO>> GetSongsByArtistId(int artistId);
        Task<IEnumerable<SongReturnDTO>> GetSongsByAlbumId(int albumId);
        Task<IEnumerable<SongReturnDTO>> GetAlbumSongs();
        Task<IEnumerable<SongReturnDTO>> GetSongsByGenre(string genre);
    }
}
