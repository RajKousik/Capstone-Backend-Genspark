using MusicApplicationAPI.Interfaces.Repository;
using MusicApplicationAPI.Models.DbModels;

namespace MusicApplicationAPI.Interfaces.Service
{
    public interface IArtistRepository : IRepository<int, Artist>
    {
    }
}
