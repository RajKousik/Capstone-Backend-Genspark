using AutoMapper;
using MusicApplicationAPI.Interfaces.Repository;
using MusicApplicationAPI.Interfaces.Service;
using MusicApplicationAPI.Exceptions.RatingExceptions;
using MusicApplicationAPI.Exceptions.SongExceptions;
using MusicApplicationAPI.Exceptions.UserExceptions;
using MusicApplicationAPI.Models.DbModels;
using MusicApplicationAPI.Models.DTOs.RatingDTO;

namespace MusicApplicationAPI.Services.RatingService
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly ISongRepository _songRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RatingService> _logger;

        public RatingService(
            IRatingRepository ratingRepository,
            ISongRepository songRepository,
            IUserRepository userRepository,
            IMapper mapper,
            ILogger<RatingService> logger)
        {
            _ratingRepository = ratingRepository;
            _songRepository = songRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Adds a rating for a song by a user.
        /// </summary>
        /// <param name="ratingDTO">The rating details to add.</param>
        /// <returns>The added rating details.</returns>
        /// <exception cref="NoSuchUserExistException">Thrown when the user does not exist.</exception>
        /// <exception cref="NoSuchSongExistException">Thrown when the song does not exist.</exception>
        /// <exception cref="AlreadyRatedException">Thrown when the user has already rated the song.</exception>
        /// <exception cref="UnableToAddRatingException">Thrown when unable to add the rating.</exception>
        public async Task<RatingReturnDTO> AddRating(RatingDTO ratingDTO)
        {
            try
            {
                var user = await _userRepository.GetById(ratingDTO.UserId);
                if (user == null) throw new NoSuchUserExistException("User not found.");

                var song = await _songRepository.GetById(ratingDTO.SongId);
                if (song == null) throw new NoSuchSongExistException("Song not found.");

                var existingRating = (await _ratingRepository.GetAll())
                    .FirstOrDefault(r => r.UserId == ratingDTO.UserId && r.SongId == ratingDTO.SongId);

                if (existingRating != null)
                {
                    throw new AlreadyRatedException("User has already rated this song.");
                }

                var rating = _mapper.Map<Rating>(ratingDTO);
                var addedRating = await _ratingRepository.Add(rating);

                return _mapper.Map<RatingReturnDTO>(addedRating);
            }
            catch (NoSuchUserExistException ex)
            {
                _logger.LogError(ex, "User not found.");
                throw;
            }
            catch (NoSuchSongExistException ex)
            {
                _logger.LogError(ex, "Song not found.");
                throw;
            }
            catch (AlreadyRatedException ex)
            {
                _logger.LogError(ex, "User has already rated this song.");
                throw;
            }
            catch (UnableToAddRatingException ex)
            {
                _logger.LogError(ex, "Unable to add rating.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding rating.");
                throw;
            }
        }

        /// <summary>
        /// Updates an existing rating for a song by a user.
        /// </summary>
        /// <param name="ratingDTO">The updated rating details.</param>
        /// <returns>The updated rating details.</returns>
        /// <exception cref="NoSuchUserExistException">Thrown when the user does not exist.</exception>
        /// <exception cref="NoSuchSongExistException">Thrown when the song does not exist.</exception>
        /// <exception cref="NoSuchRatingExistException">Thrown when the rating does not exist.</exception>
        /// <exception cref="UnableToUpdateRatingException">Thrown when unable to update the rating.</exception>
        public async Task<RatingReturnDTO> UpdateRating(RatingDTO ratingDTO)
        {
            try
            {
                var user = await _userRepository.GetById(ratingDTO.UserId);
                if (user == null) throw new NoSuchUserExistException("User not found.");

                var song = await _songRepository.GetById(ratingDTO.SongId);
                if (song == null) throw new NoSuchSongExistException("Song not found.");

                var existingRating = (await _ratingRepository.GetAll())
                    .FirstOrDefault(r => r.UserId == ratingDTO.UserId && r.SongId == ratingDTO.SongId);

                if (existingRating == null)
                {
                    throw new NoSuchRatingExistException("Rating does not exist.");
                }

                existingRating.RatingValue = ratingDTO.RatingValue;
                var updatedRating = await _ratingRepository.Update(existingRating);

                return _mapper.Map<RatingReturnDTO>(updatedRating);
            }
            catch (NoSuchUserExistException ex)
            {
                _logger.LogError(ex, "User not found.");
                throw;
            }
            catch (NoSuchSongExistException ex)
            {
                _logger.LogError(ex, "Song not found.");
                throw;
            }
            catch (NoSuchRatingExistException ex)
            {
                _logger.LogError(ex, "Rating does not exist.");
                throw;
            }
            catch (UnableToUpdateRatingException ex)
            {
                _logger.LogError(ex, "Unable to update rating.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating rating.");
                throw;
            }
        }

        /// <summary>
        /// Retrieves ratings for a specific song.
        /// </summary>
        /// <param name="songId">The ID of the song.</param>
        /// <returns>A list of ratings for the specified song.</returns>
        /// <exception cref="NoSuchSongExistException">Thrown when the song does not exist.</exception>
        /// <exception cref="NoRatingsExistsException">Thrown when no ratings are found for the song.</exception>
        public async Task<IEnumerable<RatingReturnDTO>> GetRatingsBySongId(int songId)
        {
            try
            {
                var song = await _songRepository.GetById(songId);
                if (song == null) throw new NoSuchSongExistException("Song not found.");

                var ratings = (await _ratingRepository.GetAll())
                    .Where(r => r.SongId == songId)
                    .OrderByDescending(r => r.RatingValue)
                    .ToList();

                if (!ratings.Any())
                    throw new NoRatingsExistsException("No ratings found for this song.");

                return _mapper.Map<IEnumerable<RatingReturnDTO>>(ratings);
            }
            catch (NoSuchSongExistException ex)
            {
                _logger.LogError(ex, "Song not found.");
                throw;
            }
            catch (NoRatingsExistsException ex)
            {
                _logger.LogError(ex, "No ratings found for this song.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ratings by song.");
                throw;
            }
        }

        /// <summary>
        /// Retrieves ratings given by a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of ratings given by the specified user.</returns>
        /// <exception cref="NoSuchUserExistException">Thrown when the user does not exist.</exception>
        /// <exception cref="NoRatingsExistsException">Thrown when no ratings are found for the user.</exception>
        public async Task<IEnumerable<RatingReturnDTO>> GetRatingsByUserId(int userId)
        {
            try
            {
                var user = await _userRepository.GetById(userId);
                if (user == null) throw new NoSuchUserExistException("User not found.");

                var ratings = (await _ratingRepository.GetAll())
                    .Where(r => r.UserId == userId)
                    .OrderByDescending(r => r.RatingValue)
                    .ToList();

                if (!ratings.Any())
                    throw new NoRatingsExistsException("No ratings found for this user.");

                return _mapper.Map<IEnumerable<RatingReturnDTO>>(ratings);
            }
            catch (NoSuchUserExistException ex)
            {
                _logger.LogError(ex, "User not found.");
                throw;
            }
            catch (NoRatingsExistsException ex)
            {
                _logger.LogError(ex, "No ratings found for this user.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ratings by user.");
                throw;
            }
        }

        /// <summary>
        /// Deletes a rating for a song by a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="songId">The ID of the song.</param>
        /// <exception cref="NoSuchUserExistException">Thrown when the user does not exist.</exception>
        /// <exception cref="NoSuchSongExistException">Thrown when the song does not exist.</exception>
        /// <exception cref="NoSuchRatingExistException">Thrown when the rating does not exist.</exception>
        /// <exception cref="UnableToDeleteRatingException">Thrown when unable to delete the rating.</exception>
        public async Task DeleteRating(int userId, int songId)
        {
            try
            {
                var user = await _userRepository.GetById(userId);
                if (user == null) throw new NoSuchUserExistException("User not found.");
                var song = await _songRepository.GetById(songId);
                if (song == null) throw new NoSuchSongExistException("Song not found.");

                var rating = (await _ratingRepository.GetAll())
                    .FirstOrDefault(r => r.UserId == userId && r.SongId == songId);

                if (rating == null)
                {
                    throw new NoSuchRatingExistException("Rating does not exist.");
                }

                await _ratingRepository.Delete(rating.RatingId);
            }
            catch (NoSuchUserExistException ex)
            {
                _logger.LogError(ex, "User not found.");
                throw;
            }
            catch (NoSuchSongExistException ex)
            {
                _logger.LogError(ex, "Song not found.");
                throw;
            }
            catch (NoSuchRatingExistException ex)
            {
                _logger.LogError(ex, "Rating does not exist.");
                throw;
            }
            catch (UnableToDeleteRatingException ex)
            {
                _logger.LogError(ex, "Unable to delete rating.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting rating.");
                throw;
            }
        }

        /// <summary>
        /// Retrieves the top-rated songs by average rating.
        /// </summary>
        /// <returns>A list of top-rated songs with their average rating.</returns>
        /// <exception cref="UnableToRetrieveTopRatedSongsException">Thrown when unable to retrieve the top-rated songs.</exception>
        public async Task<IEnumerable<SongRatingDTO>> TopRatedSongs()
        {
            try
            {
                var songs = await _songRepository.GetAll();
                var ratings = await _ratingRepository.GetAll();

                if (!songs.Any())
                {
                    throw new NoSongsExistsException("No songs found.");
                }

                if (!ratings.Any())
                {
                    throw new NoRatingsExistsException("No ratings found.");
                }

                var averageRatings = ratings
                                        .GroupBy(r => r.SongId)
                                        .Select(g => new
                                        {
                                            SongId = g.Key,
                                            AverageRating = g.Average(r => r.RatingValue)
                                        })
                                        .OrderByDescending(r => r.AverageRating)
                                        .ToList();

                var songRatingDTOs = averageRatings
                                        .Join(songs,
                                            ar => ar.SongId,
                                            s => s.SongId,
                                            (ar, s) => new SongRatingDTO
                                            {
                                                SongId = ar.SongId,
                                                Title = s.Title, 
                                                AverageRating = ar.AverageRating
                                            })
                                        .ToList();

                return songRatingDTOs;
            }
            catch (NoSongsExistsException ex)
            {
                _logger.LogError(ex, "Songs not found.");
                throw;
            }
            catch (NoRatingsExistsException ex)
            {
                _logger.LogError(ex, "Ratings not found.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving top-rated songs.");
                throw new UnableToRetrieveTopRatedSongsException($"Unable to retrieve top-rated songs. {ex}");
            }
        }

    }
}




