using Microsoft.AspNetCore.Mvc;
using MusicApplicationAPI.Models.DTOs.FavoriteDTO;
using MusicApplicationAPI.Models.DTOs.PlaylistDTO;
using MusicApplicationAPI.Models.DTOs.SongDTO;

namespace MusicApplicationAPI.Interfaces.Service
{
    public interface IFavoriteService
    {
        Task MarkSongAsFavorite(FavoriteSongDTO favoriteSongDTO);
        Task RemoveSongFromFavorites(FavoriteSongDTO favoriteSongDTO);
        Task MarkPlaylistAsFavorite(FavoritePlaylistDTO favoritePlaylistDTO);
        Task RemovePlaylistFromFavorites(FavoritePlaylistDTO favoritePlaylistDTO);
        Task<IEnumerable<SongReturnDTO>> GetFavoriteSongsByUserId(int userId);
        Task<IEnumerable<PlaylistReturnDTO>> GetFavoritePlaylistsByUserId(int userId);
    }
}
