using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicApplicationAPI.Contexts;
using MusicApplicationAPI.Exceptions.UserExceptions;
using MusicApplicationAPI.Interfaces.Repository;
using MusicApplicationAPI.Models.DbModels;

namespace MusicApplicationAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        #region Fields
        private readonly MusicManagementContext _context;
        #endregion

        #region Constructor
        public UserRepository(MusicManagementContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Adds a new user to the database.
        /// </summary>
        /// <param name="user">The user to be added.</param>
        /// <returns>The added user.</returns>
        /// <exception cref="UnableToAddUserException">Thrown when the user could not be added to the database.</exception>
        public async Task<User> Add(User user)
        {
            await _context.Users.AddAsync(user);
            int noOfRowsUpdated = await _context.SaveChangesAsync();
            if (noOfRowsUpdated <= 0)
                throw new UnableToAddUserException($"Something went wrong while adding a user");
            return user;
        }

        /// <summary>
        /// Deletes a user from the database by their ID.
        /// </summary>
        /// <param name="userId">The ID of the user to be deleted.</param>
        /// <returns>The deleted user.</returns>
        /// <exception cref="UnableToDeleteUserException">Thrown when the user could not be deleted from the database.</exception>
        /// <exception cref="NoSuchUserExistException">Thrown when no user with the specified ID exists.</exception>
        public async Task<User> Delete(int userId)
        {
            var user = await GetById(userId);

            _context.Users.Remove(user);
            int noOfRowsUpdated = await _context.SaveChangesAsync();

            return (noOfRowsUpdated <= 0)
                    ?
                    throw new UnableToDeleteUserException($"Something went wrong while deleting a user")
                    :
                    user;
        }

        /// <summary>
        /// Retrieves all users from the database.
        /// </summary>
        /// <returns>A list of all users.</returns>
        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        /// <summary>
        /// Retrieves a user from the database by their ID.
        /// </summary>
        /// <param name="userId">The ID of the user to be retrieved.</param>
        /// <returns>The user with the specified ID.</returns>
        /// <exception cref="NoSuchUserExistException">Thrown when no user with the specified ID exists.</exception>
        public async Task<User> GetById(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            return user == null
            ?
                    throw new NoSuchUserExistException($"User with Id {userId} doesn't exist!")
                    :
                    user;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }

        /// <summary>
        /// Updates an existing user in the database.
        /// </summary>
        /// <param name="item">The user object with updated information.</param>
        /// <returns>The updated user.</returns>
        /// <exception cref="UnableToUpdateUserException">Thrown when the user could not be updated in the database.</exception>
        /// <exception cref="NoSuchUserExistException">Thrown when no user with the specified ID exists.</exception>
        public async Task<User> Update(User item)
        {
            var user = await GetById(item.UserId);
            _context.Users.Update(user);
            int noOfRowsUpdated = await _context.SaveChangesAsync();

            return (noOfRowsUpdated <= 0)
                    ?
                    throw new UnableToUpdateUserException($"Something went wrong while updating a user")
                    :
                    user;
        }
        
        #endregion
    }
}
