using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicApplicationAPI.Contexts;
using MusicApplicationAPI.Exceptions.PlaylistSongExceptions;
using MusicApplicationAPI.Interfaces.Repository;
using MusicApplicationAPI.Models.DbModels;

namespace MusicApplicationAPI.Repositories
{
    public class PlaylistSongRepository : IRepository<int, PlaylistSong>
    {
        #region Fields
        private readonly MusicManagementContext _context;
        #endregion

        #region Constructor
        public PlaylistSongRepository(MusicManagementContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds a new playlist-song relationship to the database.
        /// </summary>
        /// <param name="playlistSong">The playlist-song relationship to be added.</param>
        /// <returns>The added playlist-song relationship.</returns>
        /// <exception cref="UnableToAddPlaylistSongException">Thrown when the playlist-song relationship could not be added to the database.</exception>
        public async Task<PlaylistSong> Add(PlaylistSong playlistSong)
        {
            await _context.PlaylistSongs.AddAsync(playlistSong);
            int noOfRowsUpdated = await _context.SaveChangesAsync();
            if (noOfRowsUpdated <= 0)
                throw new UnableToAddPlaylistSongException("Something went wrong while adding the playlist-song relationship");
            return playlistSong;
        }

        /// <summary>
        /// Deletes a playlist-song relationship from the database by its ID.
        /// </summary>
        /// <param name="playlistSongId">The ID of the playlist-song relationship to be deleted.</param>
        /// <returns>The deleted playlist-song relationship.</returns>
        /// <exception cref="NoSuchPlaylistSongException">Thrown when no playlist-song relationship with the specified ID exists.</exception>
        /// <exception cref="UnableToDeletePlaylistSongException">Thrown when the playlist-song relationship could not be deleted from the database.</exception>
        public async Task<PlaylistSong> Delete(int playlistSongId)
        {
            var playlistSong = await GetById(playlistSongId);

            _context.PlaylistSongs.Remove(playlistSong);
            int noOfRowsUpdated = await _context.SaveChangesAsync();

            return (noOfRowsUpdated <= 0)
                    ?
                    throw new UnableToDeletePlaylistSongException("Something went wrong while deleting the playlist-song relationship")
                    :
                    playlistSong;
        }

        /// <summary>
        /// Retrieves all playlist-song relationships from the database.
        /// </summary>
        /// <returns>A list of all playlist-song relationships.</returns>
        public async Task<IEnumerable<PlaylistSong>> GetAll()
        {
            return await _context.PlaylistSongs.ToListAsync();
        }

        /// <summary>
        /// Retrieves a playlist-song relationship from the database by its ID.
        /// </summary>
        /// <param name="playlistSongId">The ID of the playlist-song relationship to be retrieved.</param>
        /// <returns>The playlist-song relationship with the specified ID.</returns>
        /// <exception cref="NoSuchPlaylistSongException">Thrown when no playlist-song relationship with the specified ID exists.</exception>
        public async Task<PlaylistSong> GetById(int playlistSongId)
        {
            var playlistSong = await _context.PlaylistSongs.FirstOrDefaultAsync(ps => ps.PlaylistSongId == playlistSongId);
            return playlistSong == null
            ?
                    throw new NoSuchPlaylistSongException($"PlaylistSong with Id {playlistSongId} doesn't exist!")
                    :
                    playlistSong;
        }

        /// <summary>
        /// Updates an existing playlist-song relationship in the database.
        /// </summary>
        /// <param name="item">The playlist-song relationship with updated information.</param>
        /// <returns>The updated playlist-song relationship.</returns>
        /// <exception cref="UnableToUpdatePlaylistSongException">Thrown when the playlist-song relationship could not be updated in the database.</exception>
        /// <exception cref="NoSuchPlaylistSongException">Thrown when no playlist-song relationship with the specified ID exists.</exception>
        public async Task<PlaylistSong> Update(PlaylistSong item)
        {
            var playlistSong = await GetById(item.PlaylistSongId);
            _context.PlaylistSongs.Update(playlistSong);
            int noOfRowsUpdated = await _context.SaveChangesAsync();

            return (noOfRowsUpdated <= 0)
                    ?
                    throw new UnableToUpdatePlaylistSongException("Something went wrong while updating the playlist-song relationship")
                    :
                    playlistSong;
        }
        #endregion
    }
}
