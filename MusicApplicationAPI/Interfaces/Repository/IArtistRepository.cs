using MusicApplicationAPI.Models.DbModels;

namespace MusicApplicationAPI.Interfaces.Repository
{
    public interface IArtistRepository : IRepository<int, Artist>
    {
        Task<Artist> GetArtistByEmail(string email);

        Task<Artist> GetArtistByName(string name);
    }
}
