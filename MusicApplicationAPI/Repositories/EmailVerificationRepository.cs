using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicApplicationAPI.Contexts;
using MusicApplicationAPI.Interfaces.Repository;
using MusicApplicationAPI.Models.DbModels;

namespace MusicApplicationAPI.Repositories
{
    public class EmailVerificationRepository : IEmailVerificationRepository
    {
        private readonly MusicManagementContext _context;

        public EmailVerificationRepository(MusicManagementContext context)
        {
            _context = context;
        }

        public async Task<EmailVerification> Add(EmailVerification item)
        {
            await _context.EmailVerifications.AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<EmailVerification> Delete(int id)
        {
            var emailVerification = await _context.EmailVerifications.FindAsync(id);
            if (emailVerification == null) return null;

            _context.EmailVerifications.Remove(emailVerification);
            await _context.SaveChangesAsync();
            return emailVerification;
        }

        public async Task<IEnumerable<EmailVerification>> GetAll()
        {
            return await _context.EmailVerifications.ToListAsync();
        }

        public async Task<EmailVerification> GetById(int id)
        {
            return await _context.EmailVerifications.FindAsync(id);
        }

        public async Task<EmailVerification> Update(EmailVerification item)
        {
            _context.EmailVerifications.Update(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<EmailVerification> GetByUserId(int userId)
        {
            var result = await _context.EmailVerifications
                                 .FirstOrDefaultAsync(ev => ev.UserId == userId) ;

            return result;
        }
    }
}
