using MusicApplicationAPI.Models.DTOs.PlaylistDTO;
using MusicApplicationAPI.Models.DTOs.SongDTO;

namespace MusicApplicationAPI.Interfaces.Service
{
    public interface IPlaylistService
    {
        Task<PlaylistReturnDTO> AddPlaylist(PlaylistAddDTO playlistCreateDTO);
        Task<PlaylistReturnDTO> UpdatePlaylist(int playlistId, PlaylistUpdateDTO playlistUpdateDTO);
        Task<PlaylistReturnDTO> GetPlaylistById(int playlistId);
        Task<IEnumerable<PlaylistReturnDTO>> GetAllPlaylists();
        Task<PlaylistReturnDTO> DeletePlaylist(int playlistId);
        Task<IEnumerable<PlaylistReturnDTO>> GetPlaylistsByUserId(int userId);
        Task<IEnumerable<PlaylistReturnDTO>> GetPublicPlaylists();
    }
}
