using MusicApplicationAPI.Interfaces.Repository;
using MusicApplicationAPI.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using MusicApplicationAPI.Contexts;

namespace MusicApplicationAPI.Repositories
{
    public class PremiumUserRepository : IPremiumUserRepository
    {
        private readonly MusicManagementContext _context;

        public PremiumUserRepository(MusicManagementContext context)
        {
            _context = context;
        }

        public async Task<PremiumUser> Add(PremiumUser item)
        {
            _context.PremiumUsers.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<PremiumUser> Delete(int id)
        {
            var item = await _context.PremiumUsers.FindAsync(id);
            if (item != null)
            {
                _context.PremiumUsers.Remove(item);
                await _context.SaveChangesAsync();
            }
            return item;
        }

        public async Task<PremiumUser> Update(PremiumUser item)
        {
            _context.PremiumUsers.Update(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<PremiumUser> GetById(int id)
        {
            return await _context.PremiumUsers.FindAsync(id);
        }

        public async Task<PremiumUser> GetByUserId(int userId)
        {
            return await _context.PremiumUsers.FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task<IEnumerable<PremiumUser>> GetAll()
        {
            return await _context.PremiumUsers.ToListAsync();
        }
    }
}
