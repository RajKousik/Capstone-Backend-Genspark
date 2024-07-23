using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicApplicationAPI.Contexts;
using MusicApplicationAPI.Exceptions.PlaylistExceptions;
using MusicApplicationAPI.Interfaces.Repository;
using MusicApplicationAPI.Models.DbModels;

namespace MusicApplicationAPI.Repositories
{
    public class PlaylistRepository : IRepository<int, Playlist>
    {
        #region Fields
        private readonly MusicManagementContext _context;
        #endregion

        #region Constructor
        public PlaylistRepository(MusicManagementContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds a new playlist to the database.
        /// </summary>
        /// <param name="playlist">The playlist object to be added.</param>
        /// <returns>The added playlist.</returns>
        /// <exception cref="UnableToAddPlaylistException">Thrown when the playlist could not be added to the database.</exception>
        public async Task<Playlist> Add(Playlist playlist)
        {
            await _context.Playlists.AddAsync(playlist);
            int noOfRowsUpdated = await _context.SaveChangesAsync();
            if (noOfRowsUpdated <= 0)
                throw new UnableToAddPlaylistException("Something went wrong while adding a playlist");
            return playlist;
        }

        /// <summary>
        /// Deletes a playlist from the database by its ID.
        /// </summary>
        /// <param name="playlistId">The ID of the playlist to be deleted.</param>
        /// <returns>The deleted playlist.</returns>
        /// <exception cref="NoSuchPlaylistException">Thrown when no playlist with the specified ID exists.</exception>
        /// <exception cref="UnableToDeletePlaylistException">Thrown when the playlist could not be deleted from the database.</exception>
        public async Task<Playlist> Delete(int playlistId)
        {
            var playlist = await GetById(playlistId);

            _context.Playlists.Remove(playlist);
            int noOfRowsUpdated = await _context.SaveChangesAsync();

            return (noOfRowsUpdated <= 0)
                    ?
                    throw new UnableToDeletePlaylistException("Something went wrong while deleting a playlist")
                    :
                    playlist;
        }

        /// <summary>
        /// Retrieves all playlists from the database.
        /// </summary>
        /// <returns>A list of all playlists.</returns>
        public async Task<IEnumerable<Playlist>> GetAll()
        {
            return await _context.Playlists.ToListAsync();
        }

        /// <summary>
        /// Retrieves a playlist from the database by its ID.
        /// </summary>
        /// <param name="playlistId">The ID of the playlist to be retrieved.</param>
        /// <returns>The playlist with the specified ID.</returns>
        /// <exception cref="NoSuchPlaylistException">Thrown when no playlist with the specified ID exists.</exception>
        public async Task<Playlist> GetById(int playlistId)
        {
            var playlist = await _context.Playlists.FirstOrDefaultAsync(p => p.PlaylistId == playlistId);
            return playlist == null
            ?
                    throw new NoSuchPlaylistException($"Playlist with Id {playlistId} doesn't exist!")
                    :
                    playlist;
        }

        /// <summary>
        /// Updates an existing playlist in the database.
        /// </summary>
        /// <param name="item">The playlist object with updated information.</param>
        /// <returns>The updated playlist.</returns>
        /// <exception cref="UnableToUpdatePlaylistException">Thrown when the playlist could not be updated in the database.</exception>
        /// <exception cref="NoSuchPlaylistException">Thrown when no playlist with the specified ID exists.</exception>
        public async Task<Playlist> Update(Playlist item)
        {
            var playlist = await GetById(item.PlaylistId);
            _context.Playlists.Update(playlist);
            int noOfRowsUpdated = await _context.SaveChangesAsync();

            return (noOfRowsUpdated <= 0)
                    ?
                    throw new UnableToUpdatePlaylistException("Something went wrong while updating a playlist")
                    :
                    playlist;
        }
        #endregion
    }
}
