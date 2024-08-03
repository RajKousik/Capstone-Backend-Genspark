using Moq;
using AutoMapper;
using MusicApplicationAPI.Exceptions.RatingExceptions;
using MusicApplicationAPI.Exceptions.SongExceptions;
using MusicApplicationAPI.Exceptions.UserExceptions;
using MusicApplicationAPI.Exceptions.PlaylistExceptions;
using MusicApplicationAPI.Interfaces.Repository;
using MusicApplicationAPI.Models.DTOs.RatingDTO;
using MusicApplicationAPI.Models.DbModels;
using MusicApplicationAPI.Services.RatingService;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace MusicApplicationAPI.Tests
{
    public class RatingServiceTests
    {
        private readonly RatingService _ratingService;
        private readonly Mock<IRatingRepository> _mockRatingRepository;
        private readonly Mock<ISongRepository> _mockSongRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<RatingService>> _mockLogger;

        public RatingServiceTests()
        {
            _mockRatingRepository = new Mock<IRatingRepository>();
            _mockSongRepository = new Mock<ISongRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<RatingService>>();

            _ratingService = new RatingService(
                _mockRatingRepository.Object,
                _mockSongRepository.Object,
                _mockUserRepository.Object,
                _mockMapper.Object,
                _mockLogger.Object);
        }

        [Test]
        public async Task AddRating_ShouldThrowNoSuchUserExistException_WhenUserDoesNotExist()
        {
            // Arrange
            var ratingDTO = new RatingDTO { UserId = 1, SongId = 1, RatingValue = 5 };
            _mockUserRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((User)null);

            // Act & Assert
            Assert.ThrowsAsync<NoSuchUserExistException>(async () => await _ratingService.AddRating(ratingDTO));
        }

        [Test]
        public async Task AddRating_ShouldThrowNoSuchSongExistException_WhenSongDoesNotExist()
        {
            // Arrange
            var ratingDTO = new RatingDTO { UserId = 1, SongId = 1, RatingValue = 5 };
            _mockUserRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(new User());
            _mockSongRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((Song)null);

            // Act & Assert
            Assert.ThrowsAsync<NoSuchSongExistException>(async () => await _ratingService.AddRating(ratingDTO));
        }

        [Test]
        public async Task AddRating_ShouldThrowAlreadyRatedException_WhenAlreadyRated()
        {
            // Arrange
            var ratingDTO = new RatingDTO { UserId = 1, SongId = 1, RatingValue = 5 };
            _mockUserRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(new User());
            _mockSongRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(new Song());
            _mockRatingRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Rating> { new Rating { UserId = 1, SongId = 1, RatingValue = 4 } });

            // Act & Assert
            Assert.ThrowsAsync<AlreadyRatedException>(async () => await _ratingService.AddRating(ratingDTO));
        }

        [Test]
        public async Task RemoveRating_ShouldThrowNoSuchUserExistException_WhenUserDoesNotExist()
        {
            // Arrange
            var ratingDTO = new RatingDTO { UserId = 1, SongId = 1 };
            _mockUserRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((User)null);

            // Act & Assert
            Assert.ThrowsAsync<NoSuchUserExistException>(async () => await _ratingService.DeleteRating(1, 1));
        }

        [Test]
        public async Task RemoveRating_ShouldThrowNoSuchSongExistException_WhenSongDoesNotExist()
        {
            // Arrange
            var ratingDTO = new RatingDTO { UserId = 1, SongId = 1 };
            _mockUserRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(new User());
            _mockSongRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((Song)null);

            // Act & Assert
            Assert.ThrowsAsync<NoSuchSongExistException>(async () => await _ratingService.DeleteRating(1, 1));
        }

        [Test]
        public async Task GetRatingsByUserId_ShouldThrowNoSuchUserExistException_WhenUserDoesNotExist()
        {
            // Arrange
            int userId = 1;
            _mockUserRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((User)null);

            // Act & Assert
            Assert.ThrowsAsync<NoSuchUserExistException>(async () => await _ratingService.GetRatingsByUserId(userId));
        }

        [Test]
        public async Task GetRatingsByUserId_ShouldThrowNoRatingsExistsException_WhenNoRatingsFound()
        {
            // Arrange
            int userId = 1;
            _mockUserRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(new User());
            _mockRatingRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Rating>());

            // Act & Assert
            Assert.ThrowsAsync<NoRatingsExistsException>(async () => await _ratingService.GetRatingsByUserId(userId));
        }

        [Test]
        public async Task GetRatingsBySongId_ShouldThrowNoSuchSongExistException_WhenSongDoesNotExist()
        {
            // Arrange
            int songId = 1;
            _mockSongRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((Song)null);

            // Act & Assert
            Assert.ThrowsAsync<NoSuchSongExistException>(async () => await _ratingService.GetRatingsBySongId(songId));
        }

        [Test]
        public async Task GetRatingsBySongId_ShouldThrowNoRatingsExistsException_WhenNoRatingsFound()
        {
            // Arrange
            int songId = 1;
            _mockSongRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(new Song());
            _mockRatingRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Rating>());

            // Act & Assert
            Assert.ThrowsAsync<NoRatingsExistsException>(async () => await _ratingService.GetRatingsBySongId(songId));
        }
    }
}
