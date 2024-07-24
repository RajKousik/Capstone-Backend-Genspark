using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicApplicationAPI.Contexts;
using MusicApplicationAPI.Exceptions.SongExceptions;
using MusicApplicationAPI.Interfaces.Repository;
using MusicApplicationAPI.Models.DbModels;
using MusicApplicationAPI.Models.DTOs.SongDTO;

namespace MusicApplicationAPI.Repositories
{
    public class SongRepository : ISongRepository
    {
        #region Fields
        private readonly MusicManagementContext _context;
        #endregion

        #region Constructor
        public SongRepository(MusicManagementContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Adds a new song to the database.
        /// </summary>
        /// <param name="song">The song to be added.</param>
        /// <returns>The added song.</returns>
        /// <exception cref="UnableToAddSongException">Thrown when the song could not be added to the database.</exception>
        public async Task<Song> Add(Song song)
        {
            await _context.Songs.AddAsync(song);
            int noOfRowsUpdated = await _context.SaveChangesAsync();
            if (noOfRowsUpdated <= 0)
                throw new UnableToAddSongException($"Something went wrong while adding a song");
            return song;
        }

        /// <summary>
        /// Deletes a song from the database by its ID.
        /// </summary>
        /// <param name="songId">The ID of the song to be deleted.</param>
        /// <returns>The deleted song.</returns>
        /// <exception cref="UnableToDeleteSongException">Thrown when the song could not be deleted from the database.</exception>
        /// <exception cref="NoSuchSongExistException">Thrown when no song with the specified ID exists.</exception>
        public async Task<Song> Delete(int songId)
        {
            var song = await GetById(songId);

            _context.Songs.Remove(song);
            int noOfRowsUpdated = await _context.SaveChangesAsync();

            return (noOfRowsUpdated <= 0)
                    ?
                    throw new UnableToDeleteSongException($"Something went wrong while deleting a song")
                    :
                    song;
        }

        /// <summary>
        /// Retrieves all songs from the database.
        /// </summary>
        /// <returns>A list of all songs.</returns>
        public async Task<IEnumerable<Song>> GetAll()
        {
            var songs =  await _context.Songs.Include(s => s.Artist).Include(s=>s.Album).ToListAsync();
            return songs;
        }

        /// <summary>
        /// Retrieves a song from the database by its ID.
        /// </summary>
        /// <param name="songId">The ID of the song to be retrieved.</param>
        /// <returns>The song with the specified ID.</returns>
        /// <exception cref="NoSuchSongExistException">Thrown when no song with the specified ID exists.</exception>
        public async Task<Song> GetById(int songId)
        {
            var song = await _context.Songs.FirstOrDefaultAsync(s => s.SongId == songId);
            return song == null
            ?
                    throw new NoSuchSongExistException($"Song with Id {songId} doesn't exist!")
                    :
                    song;
        }

        public async Task<IEnumerable<Song>> GetSongsByAlbumId(int albumId)
        {
            var songs = (await _context.Songs.ToListAsync()).Where(s => s.AlbumId == albumId);
            return songs == null
            ?
                    throw new NoSongsExistsException($"No songs with album Id {albumId}")
                    :
                    songs;
        }

        public async Task<IEnumerable<Song>> GetSongsByArtistId(int artistId)
        {
            var songs = (await _context.Songs.ToListAsync()).Where(s => s.ArtistId == artistId);
            return songs == null
                    ?
                    throw new NoSongsExistsException($"No songs with artist Id {artistId}")
                    :
                    songs;
        }

        /// <summary>
        /// Updates an existing song in the database.
        /// </summary>
        /// <param name="item">The song object with updated information.</param>
        /// <returns>The updated song.</returns>
        /// <exception cref="UnableToUpdateSongException">Thrown when the song could not be updated in the database.</exception>
        /// <exception cref="NoSuchSongExistException">Thrown when no song with the specified ID exists.</exception>
        public async Task<Song> Update(Song item)
        {
            var song = await GetById(item.SongId);
            _context.Songs.Update(song);
            int noOfRowsUpdated = await _context.SaveChangesAsync();

            return (noOfRowsUpdated <= 0)
                    ?
                    throw new UnableToUpdateSongException($"Something went wrong while updating a song")
                    :
                    song;
        }
        #endregion
    }
}
