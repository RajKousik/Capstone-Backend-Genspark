using Moq;
using AutoMapper;
using MusicApplicationAPI.Exceptions.FavoriteExceptions;
using MusicApplicationAPI.Exceptions.SongExceptions;
using MusicApplicationAPI.Exceptions.UserExceptions;
using MusicApplicationAPI.Exceptions.PlaylistExceptions;
using MusicApplicationAPI.Interfaces.Repository;
using MusicApplicationAPI.Models.DTOs.FavoriteDTO;
using MusicApplicationAPI.Models.DbModels;
using MusicApplicationAPI.Services.FavoriteService;
using Microsoft.Extensions.Logging;

namespace MusicApplicationAPI.Tests
{
    public class FavoriteServiceTests
    {
        private readonly FavoriteService _favoriteService;
        private readonly Mock<IFavoriteRepository> _mockFavoriteRepository;
        private readonly Mock<IPlaylistRepository> _mockPlaylistRepository;
        private readonly Mock<ISongRepository> _mockSongRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<FavoriteService>> _mockLogger;

        public FavoriteServiceTests()
        {
            _mockFavoriteRepository = new Mock<IFavoriteRepository>();
            _mockPlaylistRepository = new Mock<IPlaylistRepository>();
            _mockSongRepository = new Mock<ISongRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<FavoriteService>>();

            _favoriteService = new FavoriteService(
                _mockFavoriteRepository.Object,
                _mockPlaylistRepository.Object,
                _mockSongRepository.Object,
                _mockUserRepository.Object,
                _mockMapper.Object,
                _mockLogger.Object);
        }

        [Test]
        public async Task MarkSongAsFavorite_ShouldThrowNoSuchUserExistException_WhenUserDoesNotExist()
        {
            // Arrange
            var favoriteSongDTO = new FavoriteSongDTO { UserId = 1, SongId = 1 };
            _mockUserRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((User)null);

            // Act & Assert
             Assert.ThrowsAsync<NoSuchUserExistException>(async() => await _favoriteService.MarkSongAsFavorite(favoriteSongDTO));
        }

        [Test]
        public async Task MarkSongAsFavorite_ShouldThrowNoSuchSongExistException_WhenSongDoesNotExist()
        {
            // Arrange
            var favoriteSongDTO = new FavoriteSongDTO { UserId = 1, SongId = 1 };
            _mockUserRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(new User());
            _mockSongRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((Song)null);

            // Act & Assert
            Assert.ThrowsAsync<NoSuchSongExistException>(async () => await _favoriteService.MarkSongAsFavorite(favoriteSongDTO));
        }

        [Test]
        public async Task MarkSongAsFavorite_ShouldThrowAlreadyMarkedAsFavorite_WhenAlreadyFavorited()
        {
            // Arrange
            var favoriteSongDTO = new FavoriteSongDTO { UserId = 1, SongId = 1 };
            _mockUserRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(new User());
            _mockSongRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(new Song());
            _mockFavoriteRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Favorite> { new Favorite { UserId = 1, SongId = 1 } });

            // Act & Assert
            Assert.ThrowsAsync<AlreadyMarkedAsFavorite>(async() => await _favoriteService.MarkSongAsFavorite(favoriteSongDTO));
        }

        [Test]
        public async Task RemoveSongFromFavorites_ShouldThrowNoSuchUserExistException_WhenUserDoesNotExist()
        {
            // Arrange
            var favoriteSongDTO = new FavoriteSongDTO { UserId = 1, SongId = 1 };
            _mockUserRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((User)null);

            // Act & Assert
            Assert.ThrowsAsync<NoSuchUserExistException>(async () => await _favoriteService.RemoveSongFromFavorites(favoriteSongDTO));
        }

        [Test]
        public async Task RemoveSongFromFavorites_ShouldThrowNoSuchSongExistException_WhenSongDoesNotExist()
        {
            // Arrange
            var favoriteSongDTO = new FavoriteSongDTO { UserId = 1, SongId = 1 };
            _mockUserRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(new User());
            _mockSongRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((Song)null);

            // Act & Assert
            Assert.ThrowsAsync<NoSuchSongExistException>(async() => await _favoriteService.RemoveSongFromFavorites(favoriteSongDTO));
        }

        [Test]
        public async Task MarkPlaylistAsFavorite_ShouldThrowNoSuchUserExistException_WhenUserDoesNotExist()
        {
            // Arrange
            var favoritePlaylistDTO = new FavoritePlaylistDTO { UserId = 1, PlaylistId = 1 };
            _mockUserRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((User)null);

            // Act & Assert
            Assert.ThrowsAsync<NoSuchUserExistException>(async () => await _favoriteService.MarkPlaylistAsFavorite(favoritePlaylistDTO));
        }

        [Test]
        public async Task MarkPlaylistAsFavorite_ShouldThrowNoSuchPlaylistExistException_WhenPlaylistDoesNotExist()
        {
            // Arrange
            var favoritePlaylistDTO = new FavoritePlaylistDTO { UserId = 1, PlaylistId = 1 };
            _mockUserRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(new User());
            _mockPlaylistRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((Playlist)null);

            // Act & Assert
            Assert.ThrowsAsync<NoSuchPlaylistExistException>(async() => await _favoriteService.MarkPlaylistAsFavorite(favoritePlaylistDTO));
        }

        [Test]
        public async Task GetFavoriteSongsByUserId_ShouldThrowNoSuchUserExistException_WhenUserDoesNotExist()
        {
            // Arrange
            int userId = 1;
            _mockUserRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((User)null);

            // Act & Assert
            Assert.ThrowsAsync<NoSuchUserExistException>(async () => await _favoriteService.GetFavoriteSongsByUserId(userId));
        }

        [Test]
        public async Task GetFavoriteSongsByUserId_ShouldThrowNoFavoritesExistsException_WhenNoFavoritesFound()
        {
            // Arrange
            int userId = 1;
            _mockUserRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(new User());
            _mockFavoriteRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Favorite>());
            _mockSongRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Song>());

            // Act & Assert
            Assert.ThrowsAsync<NoFavoritesExistsException>(async() => await _favoriteService.GetFavoriteSongsByUserId(userId));
        }

        [Test]
        public async Task GetFavoritePlaylistsByUserId_ShouldThrowNoSuchUserExistException_WhenUserDoesNotExist()
        {
            // Arrange
            int userId = 1;
            _mockUserRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((User)null);

            // Act & Assert
            Assert.ThrowsAsync<NoSuchUserExistException>(async() => await _favoriteService.GetFavoritePlaylistsByUserId(userId));
        }

        [Test]
        public async Task GetFavoritePlaylistsByUserId_ShouldThrowNoFavoritesExistsException_WhenNoFavoritesFound()
        {
            // Arrange
            int userId = 1;
            _mockUserRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(new User());
            _mockFavoriteRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Favorite>());
            _mockPlaylistRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Playlist>());

            // Act & Assert
            Assert.ThrowsAsync<NoFavoritesExistsException>(async () => await _favoriteService.GetFavoritePlaylistsByUserId(userId));
        }
    }
}
