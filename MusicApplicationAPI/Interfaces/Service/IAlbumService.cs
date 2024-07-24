using MusicApplicationAPI.Models.DTOs.AlbumDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicApplicationAPI.Interfaces.Service
{
    public interface IAlbumService
    {
        Task<AlbumReturnDTO> AddAlbum(AlbumAddDTO albumAddDTO);
        Task<AlbumReturnDTO> UpdateAlbum(int albumId, AlbumUpdateDTO albumUpdateDTO);
        Task<AlbumReturnDTO> GetAlbumById(int albumId);
        Task<IEnumerable<AlbumReturnDTO>> GetAllAlbums();
        Task<IEnumerable<AlbumReturnDTO>> GetAlbumsByArtistId(int artistId);
        Task<AlbumReturnDTO> DeleteAlbum(int albumId);
    }
}
