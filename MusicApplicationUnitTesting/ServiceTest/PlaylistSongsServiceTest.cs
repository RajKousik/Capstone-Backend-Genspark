using Moq;
using AutoMapper;
using MusicApplicationAPI.Exceptions.PlaylistExceptions;
using MusicApplicationAPI.Exceptions.PlaylistSongExceptions;
using MusicApplicationAPI.Exceptions.SongExceptions;
using MusicApplicationAPI.Exceptions.UserExceptions;
using MusicApplicationAPI.Interfaces.Repository;
using MusicApplicationAPI.Models.DTOs.PlaylistSongDTO;
using MusicApplicationAPI.Models.DbModels;
using MusicApplicationAPI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicApplicationAPI.Tests
{
    public class PlaylistSongServiceTests
    {
        private readonly PlaylistSongService _playlistSongService;
        private readonly Mock<IPlaylistRepository> _mockPlaylistRepository;
        private readonly Mock<ISongRepository> _mockSongRepository;
        private readonly Mock<IPlaylistSongRepository> _mockPlaylistSongRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<PlaylistSongService>> _mockLogger;
        private readonly Mock<IConfiguration> _mockConfiguration;

        public PlaylistSongServiceTests()
        {
            _mockPlaylistRepository = new Mock<IPlaylistRepository>();
            _mockSongRepository = new Mock<ISongRepository>();
            _mockPlaylistSongRepository = new Mock<IPlaylistSongRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<PlaylistSongService>>();
            _mockConfiguration = new Mock<IConfiguration>();

            _playlistSongService = new PlaylistSongService(
                _mockPlaylistRepository.Object,
                _mockSongRepository.Object,
                _mockPlaylistSongRepository.Object,
                _mockMapper.Object,
                _mockLogger.Object,
                _mockConfiguration.Object,
                _mockUserRepository.Object);
        }

        [Test]
        public async Task AddSongToPlaylist_ShouldThrowNoSuchPlaylistExistException_WhenPlaylistDoesNotExist()
        {
            // Arrange
            var playlistSongDTO = new PlaylistSongDTO { PlaylistId = 1, SongId = 1 };
            _mockPlaylistRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((Playlist)null);

            // Act & Assert
            Assert.ThrowsAsync<NoSuchPlaylistExistException>(async () => await _playlistSongService.AddSongToPlaylist(playlistSongDTO));
        }

        [Test]
        public async Task AddSongToPlaylist_ShouldThrowNoSuchSongExistException_WhenSongDoesNotExist()
        {
            // Arrange
            var playlistSongDTO = new PlaylistSongDTO { PlaylistId = 1, SongId = 1 };
            _mockPlaylistRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(new Playlist());
            _mockSongRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((Song)null);

            // Act & Assert
            Assert.ThrowsAsync<NoSuchSongExistException>(async () => await _playlistSongService.AddSongToPlaylist(playlistSongDTO));
        }

        [Test]
        public async Task AddSongToPlaylist_ShouldThrowUnableToAddPlaylistSongException_WhenSongAlreadyInPlaylist()
        {
            // Arrange
            var playlistSongDTO = new PlaylistSongDTO { PlaylistId = 1, SongId = 1 };
            _mockPlaylistRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(new Playlist());
            _mockSongRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(new Song());
            _mockPlaylistSongRepository.Setup(repo => repo.GetPlaylistSongsByPlaylistId(It.IsAny<int>()))
                .ReturnsAsync(new List<PlaylistSong> { new PlaylistSong { PlaylistId = 1, SongId = 1 } });

            // Act & Assert
            Assert.ThrowsAsync<UnableToAddPlaylistSongException>(async () => await _playlistSongService.AddSongToPlaylist(playlistSongDTO));
        }

        [Test]
        public async Task RemoveSongFromPlaylist_ShouldThrowNoSuchPlaylistExistException_WhenPlaylistDoesNotExist()
        {
            // Arrange
            var playlistSongDTO = new PlaylistSongDTO { PlaylistId = 1, SongId = 1 };
            _mockPlaylistRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((Playlist)null);

            // Act & Assert
            Assert.ThrowsAsync<NoSuchPlaylistSongExistException>(async () => await _playlistSongService.RemoveSongFromPlaylist(playlistSongDTO));
        }

        [Test]
        public async Task RemoveSongFromPlaylist_ShouldThrowNoSuchSongExistException_WhenSongDoesNotExist()
        {
            // Arrange
            var playlistSongDTO = new PlaylistSongDTO { PlaylistId = 1, SongId = 1 };
            _mockPlaylistRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(new Playlist());
            _mockSongRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((Song)null);

            // Act & Assert
            Assert.ThrowsAsync<NoSuchPlaylistSongExistException>(async () => await _playlistSongService.RemoveSongFromPlaylist(playlistSongDTO));
        }

        [Test]
        public async Task RemoveSongFromPlaylist_ShouldThrowNoSuchPlaylistSongExistException_WhenSongNotInPlaylist()
        {
            // Arrange
            var playlistSongDTO = new PlaylistSongDTO { PlaylistId = 1, SongId = 1 };
            _mockPlaylistRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(new Playlist());
            _mockSongRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(new Song());
            _mockPlaylistSongRepository.Setup(repo => repo.GetAll())
                .ReturnsAsync(new List<PlaylistSong>());

            // Act & Assert
            Assert.ThrowsAsync<NoSuchPlaylistSongExistException>(async () => await _playlistSongService.RemoveSongFromPlaylist(playlistSongDTO));
        }

        [Test]
        public async Task GetSongsInPlaylist_ShouldThrowNoSuchPlaylistExistException_WhenPlaylistDoesNotExist()
        {
            // Arrange
            int playlistId = 1;
            _mockPlaylistRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((Playlist)null);

            // Act & Assert
            Assert.ThrowsAsync<NoSuchPlaylistExistException>(async () => await _playlistSongService.GetSongsInPlaylist(playlistId));
        }

        [Test]
        public async Task GetSongsInPlaylist_ShouldThrowNoSongsInPlaylistException_WhenNoSongsFound()
        {
            // Arrange
            int playlistId = 1;
            _mockPlaylistRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(new Playlist());
            _mockPlaylistSongRepository.Setup(repo => repo.GetPlaylistSongsByPlaylistId(It.IsAny<int>()))
                .ReturnsAsync(new List<PlaylistSong>());

            // Act & Assert
            Assert.ThrowsAsync<NoSongsInPlaylistException>(async () => await _playlistSongService.GetSongsInPlaylist(playlistId));
        }

        [Test]
        public async Task ClearPlaylist_ShouldThrowNoSuchPlaylistExistException_WhenPlaylistDoesNotExist()
        {
            // Arrange
            int playlistId = 1;
            _mockPlaylistRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((Playlist)null);

            // Act & Assert
            Assert.ThrowsAsync<NoSuchPlaylistExistException>(async () => await _playlistSongService.ClearPlaylist(playlistId));
        }

        [Test]
        public async Task ClearPlaylist_ShouldThrowNoSongsInPlaylistException_WhenNoSongsFoundToClear()
        {
            // Arrange
            int playlistId = 1;
            _mockPlaylistRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(new Playlist());
            _mockPlaylistSongRepository.Setup(repo => repo.GetPlaylistSongsByPlaylistId(It.IsAny<int>()))
                .ReturnsAsync(new List<PlaylistSong>());

            // Act & Assert
            Assert.ThrowsAsync<NoSongsInPlaylistException>(async () => await _playlistSongService.ClearPlaylist(playlistId));
        }

        [Test]
        public async Task GetSongCountInPlaylist_ShouldThrowNoSuchPlaylistExistException_WhenPlaylistDoesNotExist()
        {
            // Arrange
            int playlistId = 1;
            _mockPlaylistRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((Playlist)null);

            // Act & Assert
            Assert.ThrowsAsync<NoSuchPlaylistExistException>(async () => await _playlistSongService.GetSongCountInPlaylist(playlistId));
        }

        [Test]
        public async Task GetSongCountInPlaylist_ShouldReturnCorrectCount()
        {
            // Arrange
            int playlistId = 1;
            _mockPlaylistRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(new Playlist());
            _mockPlaylistSongRepository.Setup(repo => repo.GetPlaylistSongsByPlaylistId(It.IsAny<int>()))
                .ReturnsAsync(new List<PlaylistSong> { new PlaylistSong(), new PlaylistSong() });

            // Act
            var count = await _playlistSongService.GetSongCountInPlaylist(playlistId);

            // Assert
            Assert.AreEqual(2, count);
        }
    }
}
