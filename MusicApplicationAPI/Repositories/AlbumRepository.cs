using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicApplicationAPI.Contexts;
using MusicApplicationAPI.Exceptions.AlbumExceptions;
using MusicApplicationAPI.Interfaces.Repository;
using MusicApplicationAPI.Models.DbModels;

namespace MusicApplicationAPI.Repositories
{
    public class AlbumRepository : IRepository<int, Album>
    {
        #region Fields
        private readonly MusicManagementContext _context;
        #endregion

        #region Constructor
        public AlbumRepository(MusicManagementContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds a new album to the database.
        /// </summary>
        /// <param name="album">The album to be added.</param>
        /// <returns>The added album.</returns>
        /// <exception cref="UnableToAddAlbumException">Thrown when the album could not be added to the database.</exception>
        public async Task<Album> Add(Album album)
        {
            await _context.Albums.AddAsync(album);
            int noOfRowsUpdated = await _context.SaveChangesAsync();
            if (noOfRowsUpdated <= 0)
                throw new UnableToAddAlbumException("Something went wrong while adding the album");
            return album;
        }

        /// <summary>
        /// Deletes an album from the database by its ID.
        /// </summary>
        /// <param name="albumId">The ID of the album to be deleted.</param>
        /// <returns>The deleted album.</returns>
        /// <exception cref="NoSuchAlbumExistException">Thrown when no album with the specified ID exists.</exception>
        /// <exception cref="UnableToDeleteAlbumException">Thrown when the album could not be deleted from the database.</exception>
        public async Task<Album> Delete(int albumId)
        {
            var album = await GetById(albumId);

            _context.Albums.Remove(album);
            int noOfRowsUpdated = await _context.SaveChangesAsync();

            return (noOfRowsUpdated <= 0)
                    ? throw new UnableToDeleteAlbumException("Something went wrong while deleting the album")
                    : album;
        }

        /// <summary>
        /// Retrieves all albums from the database.
        /// </summary>
        /// <returns>A list of all albums.</returns>
        public async Task<IEnumerable<Album>> GetAll()
        {
            return await _context.Albums.ToListAsync();
        }

        /// <summary>
        /// Retrieves an album from the database by its ID.
        /// </summary>
        /// <param name="albumId">The ID of the album to be retrieved.</param>
        /// <returns>The album with the specified ID.</returns>
        /// <exception cref="NoSuchAlbumExistException">Thrown when no album with the specified ID exists.</exception>
        public async Task<Album> GetById(int albumId)
        {
            var album = await _context.Albums.FirstOrDefaultAsync(a => a.AlbumId == albumId);
            return album == null
                ? throw new NoSuchAlbumExistException($"Album with Id {albumId} doesn't exist!")
                : album;
        }

        /// <summary>
        /// Updates an existing album in the database.
        /// </summary>
        /// <param name="item">The album object with updated information.</param>
        /// <returns>The updated album.</returns>
        /// <exception cref="UnableToUpdateAlbumException">Thrown when the album could not be updated in the database.</exception>
        /// <exception cref="NoSuchAlbumExistException">Thrown when no album with the specified ID exists.</exception>
        public async Task<Album> Update(Album item)
        {
            var album = await GetById(item.AlbumId);
            _context.Albums.Update(album);
            int noOfRowsUpdated = await _context.SaveChangesAsync();

            return (noOfRowsUpdated <= 0)
                ? throw new UnableToUpdateAlbumException("Something went wrong while updating the album")
                : album;
        }
        #endregion
    }
}
