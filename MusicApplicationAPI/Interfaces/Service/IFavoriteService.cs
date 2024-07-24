using MusicApplicationAPI.Models.DTOs.FavoriteDTO;
using MusicApplicationAPI.Models.DTOs.PlaylistDTO;
using MusicApplicationAPI.Models.DTOs.SongDTO;

namespace MusicApplicationAPI.Interfaces.Service
{
    public interface IFavoriteService
    {
        Task MarkSongAsFavorite(FavoriteDTO favoriteDTO);
        Task RemoveSongFromFavorites(FavoriteDTO favoriteDTO);
        Task MarkPlaylistAsFavorite(int userId, int playlistId);
        Task RemovePlaylistFromFavorites(int userId, int playlistId);
        Task<IEnumerable<SongReturnDTO>> GetFavoriteSongsByUserId(int userId);
        Task<IEnumerable<PlaylistReturnDTO>> GetFavoritePlaylistsByUserId(int userId);
    }
}
