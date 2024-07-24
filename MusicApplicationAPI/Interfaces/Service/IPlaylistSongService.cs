using MusicApplicationAPI.Models.DTOs.PlaylistSongDTO;
using MusicApplicationAPI.Models.DTOs.SongDTO;

namespace MusicApplicationAPI.Interfaces.Service
{
    public interface IPlaylistSongService
    {
        Task<PlaylistSongReturnDTO> AddSongToPlaylist(PlaylistSongDTO playlistSongDTO);
        Task<PlaylistSongReturnDTO> RemoveSongFromPlaylist(PlaylistSongDTO playlistSongDTO);
        Task<IEnumerable<SongReturnDTO>> GetSongsInPlaylist(int playlistId);
        Task ClearPlaylist(int playlistId);
        Task<int> GetSongCountInPlaylist(int playlistId);
    }

}
