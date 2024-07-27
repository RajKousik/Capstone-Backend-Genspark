using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicApplicationAPI.Contexts;
using MusicApplicationAPI.Exceptions.ArtistExceptions;
using MusicApplicationAPI.Interfaces.Repository;
using MusicApplicationAPI.Models.DbModels;

namespace MusicApplicationAPI.Repositories
{
    public class ArtistRepository : IArtistRepository
    {
        #region Fields
        private readonly MusicManagementContext _context;
        #endregion

        #region Constructor
        public ArtistRepository(MusicManagementContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds a new artist to the database.
        /// </summary>
        /// <param name="artist">The artist to be added.</param>
        /// <returns>The added artist.</returns>
        /// <exception cref="UnableToAddArtistException">Thrown when the artist could not be added to the database.</exception>
        public async Task<Artist> Add(Artist artist)
        {
            await _context.Artists.AddAsync(artist);
            int noOfRowsUpdated = await _context.SaveChangesAsync();
            if (noOfRowsUpdated <= 0)
                throw new UnableToAddArtistException("Something went wrong while adding the artist");
            return artist;
        }

        /// <summary>
        /// Deletes an artist from the database by its ID.
        /// </summary>
        /// <param name="artistId">The ID of the artist to be deleted.</param>
        /// <returns>The deleted artist.</returns>
        /// <exception cref="NoSuchArtistExistException">Thrown when no artist with the specified ID exists.</exception>
        /// <exception cref="UnableToDeleteArtistException">Thrown when the artist could not be deleted from the database.</exception>
        public async Task<Artist> Delete(int artistId)
        {
            var artist = await GetById(artistId);

            _context.Artists.Remove(artist);
            int noOfRowsUpdated = await _context.SaveChangesAsync();

            return (noOfRowsUpdated <= 0)
                    ? throw new UnableToDeleteArtistException("Something went wrong while deleting the artist")
                    : artist;
        }

        /// <summary>
        /// Retrieves all artists from the database.
        /// </summary>
        /// <returns>A list of all artists.</returns>
        /// <exception cref="NoArtistsExistsException">Thrown when no artists exist in the database.</exception>
        public async Task<IEnumerable<Artist>> GetAll()
        {
            var artists = await _context.Artists.ToListAsync();
            return artists;
        }

        public async Task<Artist> GetArtistByEmail(string email)
        {
            var artist = await _context.Artists.FirstOrDefaultAsync(u => u.Email == email);
            return artist;
        }


        public async Task<Artist> GetArtistByName(string name)
        {
            var artist = await _context.Artists.FirstOrDefaultAsync(u => u.Name == name);
            return artist;
        }

        /// <summary>
        /// Retrieves an artist from the database by its ID.
        /// </summary>
        /// <param name="artistId">The ID of the artist to be retrieved.</param>
        /// <returns>The artist with the specified ID.</returns>
        /// <exception cref="NoSuchArtistExistException">Thrown when no artist with the specified ID exists.</exception>
        public async Task<Artist> GetById(int artistId)
        {
            var artist = await _context.Artists.FirstOrDefaultAsync(a => a.ArtistId == artistId);
            return artist == null
                ? throw new NoSuchArtistExistException($"Artist with Id {artistId} doesn't exist!")
                : artist;
        }

        /// <summary>
        /// Updates an existing artist in the database.
        /// </summary>
        /// <param name="item">The artist object with updated information.</param>
        /// <returns>The updated artist.</returns>
        /// <exception cref="UnableToUpdateArtistException">Thrown when the artist could not be updated in the database.</exception>
        /// <exception cref="NoSuchArtistExistException">Thrown when no artist with the specified ID exists.</exception>
        public async Task<Artist> Update(Artist item)
        {
            var artist = await GetById(item.ArtistId);
            _context.Artists.Update(artist);
            int noOfRowsUpdated = await _context.SaveChangesAsync();

            return (noOfRowsUpdated <= 0)
                ? throw new UnableToUpdateArtistException("Something went wrong while updating the artist")
                : artist;
        }
        #endregion
    }
}
