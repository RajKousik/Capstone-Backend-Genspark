using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MusicApplicationAPI.Interfaces.Repository;
using MusicApplicationAPI.Interfaces.Service;
using MusicApplicationAPI.Models.DbModels;
using MusicApplicationAPI.Models.DTOs;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MusicApplicationAPI.Exceptions.ArtistExceptions;
using MusicApplicationAPI.Models.DTOs.ArtistDTO;
using MusicApplicationAPI.Models.DTOs.SongDTO;
using MusicApplicationAPI.Repositories;
using MusicApplicationAPI.Exceptions.SongExceptions;
using MusicApplicationAPI.Exceptions.UserExceptions;
using MusicApplicationAPI.Models.Enums;
using MusicApplicationAPI.Services.UserService;
using MusicApplicationAPI.Exceptions.EmailExceptions;
using MusicApplicationAPI.Interfaces.Service.TokenService;
using MusicApplicationAPI.Models.DTOs.OtherDTO;

namespace MusicApplicationAPI.Services
{
    public class ArtistService : IArtistService
    {
        private readonly IArtistRepository _artistRepository;
        private readonly ISongRepository _songRepository;
        private readonly IPasswordService _passwordService;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly ILogger<ArtistService> _logger;

        public ArtistService(IArtistRepository artistRepository, ISongRepository songRepository, IMapper mapper, ILogger<ArtistService> logger, IPasswordService passwordService, ITokenService tokenService)
        {
            _artistRepository = artistRepository;
            _songRepository = songRepository;
            _mapper = mapper;
            _logger = logger;
            _passwordService = passwordService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Adds a new artist.
        /// </summary>
        /// <param name="artistAddDTO">The artist data to add.</param>
        /// <returns>The added artist as a DTO.</returns>
        /// <exception cref="UnableToAddArtistException">Thrown when the artist cannot be added.</exception>
        public async Task<ArtistReturnDTO> AddArtist(ArtistAddDTO artistAddDTO)
        {
            try
            {
                var artist = _mapper.Map<Artist>(artistAddDTO);
                var addedArtist = await _artistRepository.Add(artist);
                return _mapper.Map<ArtistReturnDTO>(addedArtist);
            }
            catch (UnableToAddArtistException ex)
            {
                _logger.LogError(ex, "Error adding artist.");
                throw new UnableToAddArtistException("Unable to add artist.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding artist.");
                throw new UnableToAddArtistException("Unable to add artist.");
            }
        }

        public async Task<ArtistLoginReturnDTO> Login(ArtistLoginDTO artistLoginDTO)
        {
            try
            {
                var artistInDB = await _artistRepository.GetArtistByEmail(artistLoginDTO.Email);
                if(artistInDB == null)
                {
                    throw new UnauthorizedUserException();
                }
                ValidateArtistCredentials(artistLoginDTO.Password, artistInDB);
                EnsureArtistIsActive(artistInDB);

                return GenerateLoginReturnDTO(artistInDB);
                
            }
            catch (EmailNotVerifiedException ex)
            {
                _logger.LogError(ex.Message);
                throw new EmailNotVerifiedException("You haven't verified your email, verify it first");
            }
            catch (UnauthorizedUserException ex)
            {
                _logger.LogError(ex.Message);
                throw new UnauthorizedUserException("Invalid username or password");
            }
            catch (PremiumSubscriptionExpiredException ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<ArtistReturnDTO> ActivateArtist(int artistId)
        {
            try
            {
                var artistInDB = await _artistRepository.GetById(artistId);
                
                if(artistInDB.Status?.ToLower() == "active")
                {
                    throw new Exception("Artist Already activated");
                }

                artistInDB.Status = "Active";
                await _artistRepository.Update(artistInDB);

                return _mapper.Map<ArtistReturnDTO>(artistInDB);

                //return GenerateLoginReturnDTO(artistInDB);

            }
            catch (NoSuchArtistExistException ex)
            {
                _logger.LogError(ex.Message);
                throw new NoSuchArtistExistException();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }



        public async Task<ArtistReturnDTO> Register(ArtistAddDTO artistAddDTO)
        {
            Artist artist;
            try
            {
                var emailExists = await _artistRepository.GetArtistByEmail(artistAddDTO.Email);
                if (emailExists != null)
                {
                    throw new DuplicateEmailException("Email id is already registered");
                }

                var artistNameExists = await _artistRepository.GetArtistByName(artistAddDTO.Name);

                if (artistNameExists != null)
                {
                    throw new ArtistNameAlreadyExists("Artist Name already taken");
                }

                artist = new Artist()
                {
                    Name = artistAddDTO.Name,
                    Email = artistAddDTO.Email,
                    Bio = artistAddDTO.Bio,
                    Status = "InActive",
                    ImageUrl = artistAddDTO.ImageUrl,
                    Role = RoleType.Artist
                };

                artist.PasswordHash = _passwordService.HashPassword(artistAddDTO.Password, out byte[] key);
                artist.PasswordHashKey = key;

                var addedUser = await _artistRepository.Add(artist);

                ArtistReturnDTO artistReturnDTO = _mapper.Map<ArtistReturnDTO>(addedUser);

                return artistReturnDTO;
            }
            catch (UnableToAddArtistException ex)
            {
                _logger.LogError(ex.Message);
                throw new UnableToAddArtistException(ex.Message);
            }
            catch (ArtistNameAlreadyExists ex)
            {
                _logger.LogError(ex.Message);
                throw new ArtistNameAlreadyExists(ex.Message);
            }
            catch (DuplicateEmailException ex)
            {
                _logger.LogError(ex.Message);
                throw new DuplicateEmailException(ex.Message);
            }
            catch (InvalidPasswordException ex)
            {
                _logger.LogError(ex.Message);
                throw new InvalidPasswordException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                var message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                throw new Exception(message);
            }
        }

        /// <summary>
        /// Updates an existing artist.
        /// </summary>
        /// <param name="artistId">The ID of the artist to update.</param>
        /// <param name="artistUpdateDTO">The updated artist data.</param>
        /// <returns>The updated artist as a DTO.</returns>
        /// <exception cref="NoSuchArtistExistException">Thrown when the artist does not exist.</exception>
        /// <exception cref="UnableToUpdateArtistException">Thrown when the artist cannot be updated.</exception>
        public async Task<ArtistReturnDTO> UpdateArtist(int artistId, ArtistUpdateDTO artistUpdateDTO)
        {
            try
            {
                var existingArtist = await _artistRepository.GetById(artistId);
                if (existingArtist == null)
                    throw new NoSuchArtistExistException("Artist not found.");

                _mapper.Map(artistUpdateDTO, existingArtist);
                var updatedArtist = await _artistRepository.Update(existingArtist);
                return _mapper.Map<ArtistReturnDTO>(updatedArtist);
            }
            catch (NoSuchArtistExistException ex)
            {
                _logger.LogError(ex, "Artist not found.");
                throw;
            }
            catch (UnableToUpdateArtistException ex)
            {
                _logger.LogError(ex, "Unable to update the artist");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating artist.");
                throw;
            }
        }

        /// <summary>
        /// Retrieves an artist by ID.
        /// </summary>
        /// <param name="artistId">The ID of the artist to retrieve.</param>
        /// <returns>The artist as a DTO.</returns>
        /// <exception cref="NoSuchArtistExistException">Thrown when the artist does not exist.</exception>
        public async Task<ArtistReturnDTO> GetArtistById(int artistId)
        {
            try
            {
                var artist = await _artistRepository.GetById(artistId);
                if (artist == null)
                    throw new NoSuchArtistExistException("Artist not found.");

                return _mapper.Map<ArtistReturnDTO>(artist);
            }
            catch (NoSuchArtistExistException ex)
            {
                _logger.LogError(ex, "Artist not found.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving artist by ID.");
                throw;
            }
        }



        public async Task<bool> ChangePassword(ChangePasswordRequestDTO requestDTO, int artistId)
        {
            try
            {
                var artist = await _artistRepository.GetById(artistId);

                if (artist == null)
                    throw new NoSuchArtistExistException("Artist not found.");

                if (!_passwordService.VerifyPassword(requestDTO.CurrentPassword, artist.PasswordHash, artist.PasswordHashKey))
                    return false;

                artist.PasswordHash = _passwordService.HashPassword(requestDTO.NewPassword, out byte[] key);
                artist.PasswordHashKey = key;
                await _artistRepository.Update(artist);
                return true;
            }
            catch (NoSuchArtistExistException ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            catch (InvalidPasswordException ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            catch (UnableToUpdateArtistException ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Retrieves all artists.
        /// </summary>
        /// <returns>A list of all artists as DTOs.</returns>
        /// <exception cref="NoArtistsExistsException">Thrown when no artists exist.</exception>
        public async Task<IEnumerable<ArtistReturnDTO>> GetAllArtists()
        {
            try
            {
                var artists = (await _artistRepository.GetAll()).ToList();
                if (artists.Count == 0)
                    throw new NoArtistsExistsException("No artists found.");

                return _mapper.Map<IEnumerable<ArtistReturnDTO>>(artists);
            }
            catch (NoArtistsExistsException ex)
            {
                _logger.LogError(ex, "No artists found.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all artists.");
                throw;
            }
        }

        /// <summary>
        /// Deletes an artist by ID.
        /// </summary>
        /// <param name="artistId">The ID of the artist to delete.</param>
        /// <returns>The deleted artist as a DTO.</returns>
        /// <exception cref="NoSuchArtistException">Thrown when the artist does not exist.</exception>
        /// <exception cref="UnableToDeleteArtistException">Thrown when the artist cannot be deleted.</exception>
        public async Task<ArtistReturnDTO> DeleteArtist(int artistId)
        {
            try
            {
                var artist = await _artistRepository.GetById(artistId);
                if (artist == null)
                    throw new NoSuchArtistExistException("Artist not found.");

                var deletedArtist = await _artistRepository.Delete(artistId);
                return _mapper.Map<ArtistReturnDTO>(deletedArtist);
            }
            catch (NoSuchArtistExistException ex)
            {
                _logger.LogError(ex, "Artist not found.");
                throw;
            }
            catch (UnableToDeleteArtistException ex)
            {
                _logger.LogError(ex, "Artist not found.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting artist.");
                throw;
            }
        }

        /// <summary>
        /// Retrieves songs by a specific artist ID.
        /// </summary>
        /// <param name="artistId">The ID of the artist.</param>
        /// <returns>A list of songs by the specified artist as DTOs.</returns>
        /// <exception cref="NoSuchArtistExistException">Thrown when the artist does not exist.</exception>
        /// <exception cref="NoSongsExistsException">Thrown when no songs are found for the artist.</exception>
        public async Task<IEnumerable<SongReturnDTO>> GetSongsByArtist(int artistId)
        {
            try
            {
                var artist = await _artistRepository.GetById(artistId);
                if (artist == null)
                    throw new NoSuchArtistExistException("Artist not found.");

                var songs = (await _songRepository.GetAll()).Where(s => s.ArtistId == artistId).ToList();
                if (songs.Count == 0)
                    throw new NoSongsExistsException("No songs found for this artist.");

                return _mapper.Map<IEnumerable<SongReturnDTO>>(songs);
            }
            catch (NoSuchArtistExistException ex)
            {
                _logger.LogError(ex, "Artist not found.");
                throw;
            }
            catch (NoSongsExistsException ex)
            {
                _logger.LogError(ex, "No songs found for this artist.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving songs by artist.");
                throw;
            }
        }


        #region Private Methods


        private ArtistLoginReturnDTO GenerateLoginReturnDTO(Artist artist)
        {
            return new ArtistLoginReturnDTO
            {
                Email = artist.Email,
                ArtistId = artist.ArtistId,
                Token = _tokenService.GenerateArtistToken(artist),
                Name = artist.Name,
                Role = artist.Role
            };
        }

        private void ValidateArtistCredentials(string password, Artist artist)
        {
            if (!_passwordService.VerifyPassword(password, artist.PasswordHash, artist.PasswordHashKey))
            {
                throw new UnauthorizedUserException("Invalid username or password");
            }
        }


        private void EnsureArtistIsActive(Artist artist)
        {
            if (artist.Status == null || artist.Status.ToLower() != "active")
            {
                throw new ArtistNotActiveException($"Your Account is not activated.");
            }
        }

        #endregion
    }
}
