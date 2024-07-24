using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicApplicationAPI.Contexts;
using MusicApplicationAPI.Exceptions.RatingExceptions;
using MusicApplicationAPI.Interfaces.Repository;
using MusicApplicationAPI.Models.DbModels;

namespace MusicApplicationAPI.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        #region Fields
        private readonly MusicManagementContext _context;
        #endregion

        #region Constructor
        public RatingRepository(MusicManagementContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds a new rating to the database.
        /// </summary>
        /// <param name="rating">The rating to be added.</param>
        /// <returns>The added rating.</returns>
        /// <exception cref="UnableToAddRatingException">Thrown when the rating could not be added to the database.</exception>
        public async Task<Rating> Add(Rating rating)
        {
            await _context.Ratings.AddAsync(rating);
            int noOfRowsUpdated = await _context.SaveChangesAsync();
            if (noOfRowsUpdated <= 0)
                throw new UnableToAddRatingException("Something went wrong while adding the rating");
            return rating;
        }

        /// <summary>
        /// Deletes a rating from the database by its ID.
        /// </summary>
        /// <param name="ratingId">The ID of the rating to be deleted.</param>
        /// <returns>The deleted rating.</returns>
        /// <exception cref="NoSuchRatingExistException">Thrown when no rating with the specified ID exists.</exception>
        /// <exception cref="UnableToDeleteRatingException">Thrown when the rating could not be deleted from the database.</exception>
        public async Task<Rating> Delete(int ratingId)
        {
            var rating = await GetById(ratingId);

            _context.Ratings.Remove(rating);
            int noOfRowsUpdated = await _context.SaveChangesAsync();

            return (noOfRowsUpdated <= 0)
                    ? throw new UnableToDeleteRatingException("Something went wrong while deleting the rating")
                    : rating;
        }

        /// <summary>
        /// Retrieves all ratings from the database.
        /// </summary>
        /// <returns>A list of all ratings.</returns>
        public async Task<IEnumerable<Rating>> GetAll()
        {
            return await _context.Ratings.ToListAsync();
        }

        /// <summary>
        /// Retrieves a rating from the database by its ID.
        /// </summary>
        /// <param name="ratingId">The ID of the rating to be retrieved.</param>
        /// <returns>The rating with the specified ID.</returns>
        /// <exception cref="NoSuchRatingExistException">Thrown when no rating with the specified ID exists.</exception>
        public async Task<Rating> GetById(int ratingId)
        {
            var rating = await _context.Ratings.FirstOrDefaultAsync(r => r.RatingId == ratingId);
            return rating == null
                ? throw new NoSuchRatingExistException($"Rating with Id {ratingId} doesn't exist!")
                : rating;
        }

        /// <summary>
        /// Updates an existing rating in the database.
        /// </summary>
        /// <param name="item">The rating object with updated information.</param>
        /// <returns>The updated rating.</returns>
        /// <exception cref="UnableToUpdateRatingException">Thrown when the rating could not be updated in the database.</exception>
        /// <exception cref="NoSuchRatingExistException">Thrown when no rating with the specified ID exists.</exception>
        public async Task<Rating> Update(Rating item)
        {
            var rating = await GetById(item.RatingId);
            _context.Ratings.Update(rating);
            int noOfRowsUpdated = await _context.SaveChangesAsync();

            return (noOfRowsUpdated <= 0)
                ? throw new UnableToUpdateRatingException("Something went wrong while updating the rating")
                : rating;
        }
        #endregion
    }
}
