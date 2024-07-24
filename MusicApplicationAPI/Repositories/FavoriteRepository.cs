using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicApplicationAPI.Contexts;
using MusicApplicationAPI.Exceptions.FavoriteExceptions;
using MusicApplicationAPI.Interfaces.Repository;
using MusicApplicationAPI.Models.DbModels;

namespace MusicApplicationAPI.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        #region Fields
        private readonly MusicManagementContext _context;
        #endregion

        #region Constructor
        public FavoriteRepository(MusicManagementContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds a new favorite to the database.
        /// </summary>
        /// <param name="favorite">The favorite to be added.</param>
        /// <returns>The added favorite.</returns>
        /// <exception cref="UnableToAddFavoriteException">Thrown when the favorite could not be added to the database.</exception>
        public async Task<Favorite> Add(Favorite favorite)
        {
            await _context.Favorites.AddAsync(favorite);
            int noOfRowsUpdated = await _context.SaveChangesAsync();
            if (noOfRowsUpdated <= 0)
                throw new UnableToAddFavoriteException("Something went wrong while adding the favorite");
            return favorite;
        }

        /// <summary>
        /// Deletes a favorite from the database by its ID.
        /// </summary>
        /// <param name="favoriteId">The ID of the favorite to be deleted.</param>
        /// <returns>The deleted favorite.</returns>
        /// <exception cref="NoSuchFavoriteExistException">Thrown when no favorite with the specified ID exists.</exception>
        /// <exception cref="UnableToDeleteFavoriteException">Thrown when the favorite could not be deleted from the database.</exception>
        public async Task<Favorite> Delete(int favoriteId)
        {
            var favorite = await GetById(favoriteId);

            _context.Favorites.Remove(favorite);
            int noOfRowsUpdated = await _context.SaveChangesAsync();

            return (noOfRowsUpdated <= 0)
                    ? throw new UnableToDeleteFavoriteException("Something went wrong while deleting the favorite")
                    : favorite;
        }

        /// <summary>
        /// Retrieves all favorites from the database.
        /// </summary>
        /// <returns>A list of all favorites.</returns>
        public async Task<IEnumerable<Favorite>> GetAll()
        {
            return await _context.Favorites.ToListAsync();
        }

        /// <summary>
        /// Retrieves a favorite from the database by its ID.
        /// </summary>
        /// <param name="favoriteId">The ID of the favorite to be retrieved.</param>
        /// <returns>The favorite with the specified ID.</param>
        /// <exception cref="NoSuchFavoriteExistException">Thrown when no favorite with the specified ID exists.</exception>
        public async Task<Favorite> GetById(int favoriteId)
        {
            var favorite = await _context.Favorites.FirstOrDefaultAsync(f => f.FavoriteId == favoriteId);
            return favorite == null
                ? throw new NoSuchFavoriteExistException($"Favorite with Id {favoriteId} doesn't exist!")
                : favorite;
        }

        /// <summary>
        /// Updates an existing favorite in the database.
        /// </summary>
        /// <param name="item">The favorite object with updated information.</param>
        /// <returns>The updated favorite.</returns>
        /// <exception cref="UnableToUpdateFavoriteException">Thrown when the favorite could not be updated in the database.</exception>
        /// <exception cref="NoSuchFavoriteExistException">Thrown when no favorite with the specified ID exists.</exception>
        public async Task<Favorite> Update(Favorite item)
        {
            var favorite = await GetById(item.FavoriteId);
            _context.Favorites.Update(favorite);
            int noOfRowsUpdated = await _context.SaveChangesAsync();

            return (noOfRowsUpdated <= 0)
                ? throw new UnableToUpdateFavoriteException("Something went wrong while updating the favorite")
                : favorite;
        }
        #endregion
    }
}
