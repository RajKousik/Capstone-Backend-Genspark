using MusicApplicationAPI.Models.DTOs.ArtistDTO;
using MusicApplicationAPI.Models.DTOs.OtherDTO;
using MusicApplicationAPI.Models.DTOs.SongDTO;

namespace MusicApplicationAPI.Interfaces.Service
{
    public interface IArtistService
    {
        Task<ArtistReturnDTO> AddArtist(ArtistAddDTO artistAddDTO);

        Task<ArtistLoginReturnDTO> Login(ArtistLoginDTO artistLoginDTO);

        Task<ArtistReturnDTO> Register(ArtistAddDTO artistAddDTO);

        Task<ArtistReturnDTO> UpdateArtist(int artistId, ArtistUpdateDTO artistUpdateDTO);
        Task<ArtistReturnDTO> GetArtistById(int artistId);
        Task<IEnumerable<ArtistReturnDTO>> GetAllArtists();
        Task<ArtistReturnDTO> DeleteArtist(int artistId);
        Task<IEnumerable<SongReturnDTO>> GetSongsByArtist(int artistId);

        Task<bool> ChangePassword(ChangePasswordRequestDTO requestDTO, int artistId);
    }
}
