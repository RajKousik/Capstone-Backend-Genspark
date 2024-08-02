using MusicApplicationAPI.Models.DbModels;

namespace MusicApplicationAPI.Interfaces.Repository
{
    public interface IAlbumRepository : IRepository<int, Album>
    {
        Task<bool> DeleteRange(IList<int> key);
    }
}
