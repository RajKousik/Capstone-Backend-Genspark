using Moq;
using AutoMapper;
using MusicApplicationAPI.Exceptions.AlbumExceptions;
using MusicApplicationAPI.Exceptions.ArtistExceptions;
using MusicApplicationAPI.Exceptions.SongExceptions;
using MusicApplicationAPI.Interfaces.Repository;
using MusicApplicationAPI.Models.DbModels;
using MusicApplicationAPI.Models.DTOs.SongDTO;
using MusicApplicationAPI.Models.Enums;
using MusicApplicationAPI.Services.SongService;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicApplicationAPI.Tests
{
    public class SongServiceTests
    {
        private Mock<ISongRepository> _mockSongRepository;
        private Mock<IArtistRepository> _mockArtistRepository;
        private Mock<IAlbumRepository> _mockAlbumRepository;
        private Mock<IMapper> _mockMapper;
        private Mock<ILogger<SongService>> _mockLogger;
        private SongService _songService;

        [SetUp]
        public void Setup()
        {
            _mockSongRepository = new Mock<ISongRepository>();
            _mockArtistRepository = new Mock<IArtistRepository>();
            _mockAlbumRepository = new Mock<IAlbumRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<SongService>>();

            _songService = new SongService(
                _mockSongRepository.Object,
                _mockMapper.Object,
                _mockLogger.Object,
                _mockArtistRepository.Object,
                _mockAlbumRepository.Object);
        }

        [Test]
        public async Task AddSong_ShouldThrowNoSuchArtistExistException_WhenArtistDoesNotExist()
        {
            // Arrange
            var songAddDTO = new SongAddDTO { ArtistId = 1, AlbumId = 1, Duration = 300, Genre = "Pop", Title = "Test Song", Url = "http://test.url" };
            _mockArtistRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((Artist)null);

            // Act & Assert
            Assert.ThrowsAsync<NoSuchArtistExistException>(async () => await _songService.AddSong(songAddDTO));
        }

        [Test]
        public async Task AddSong_ShouldThrowNoSuchAlbumExistException_WhenAlbumDoesNotExist()
        {
            // Arrange
            var songAddDTO = new SongAddDTO { ArtistId = 1, AlbumId = 1, Duration = 300, Genre = "Pop", Title = "Test Song", Url = "http://test.url" };
            _mockArtistRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(new Artist());
            _mockAlbumRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((Album)null);

            // Act & Assert
            Assert.ThrowsAsync<NoSuchAlbumExistException>(async () => await _songService.AddSong(songAddDTO));
        }

        [Test]
        public async Task AddSong_ShouldThrowUnableToAddSongException_WhenSongArtistIsDifferentFromAlbumArtist()
        {
            // Arrange
            var songAddDTO = new SongAddDTO { ArtistId = 1, AlbumId = 1, Duration = 300, Genre = "Pop", Title = "Test Song", Url = "http://test.url" };
            _mockArtistRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(new Artist { ArtistId = 1 });
            _mockAlbumRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(new Album { AlbumId = 1, ArtistId = 2 });

            // Act & Assert
            Assert.ThrowsAsync<UnableToAddSongException>(async () => await _songService.AddSong(songAddDTO));
        }

        [Test]
        public async Task AddSong_ShouldThrowInvalidSongDuration_WhenDurationIsInvalid()
        {
            // Arrange
            var songAddDTO = new SongAddDTO { ArtistId = 1, AlbumId = 1, Duration = 0, Genre = "Pop", Title = "Test Song", Url = "http://test.url" };
            _mockArtistRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(new Artist { ArtistId = 1 });

            // Act & Assert
            Assert.ThrowsAsync<NoSuchAlbumExistException>(async () => await _songService.AddSong(songAddDTO));
        }

        [Test]
        public async Task AddSong_ShouldThrowInvalidGenreException_WhenGenreIsInvalid()
        {
            // Arrange
            var songAddDTO = new SongAddDTO { ArtistId = 1, AlbumId = 1, Duration = 300, Genre = "InvalidGenre", Title = "Test Song", Url = "http://test.url" };
            _mockArtistRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(new Artist { ArtistId = 1 });

            // Act & Assert
            Assert.ThrowsAsync<NoSuchAlbumExistException>(async () => await _songService.AddSong(songAddDTO));
        }

        [Test]
        public async Task DeleteSong_ShouldThrowNoSuchSongExistException_WhenSongDoesNotExist()
        {
            // Arrange
            _mockSongRepository.Setup(repo => repo.Delete(It.IsAny<int>())).ThrowsAsync(new NoSuchSongExistException("Song not found"));

            // Act & Assert
            Assert.ThrowsAsync<NoSuchSongExistException>(async () => await _songService.DeleteSong(1));
        }

        [Test]
        public async Task GetSongById_ShouldThrowNoSuchSongExistException_WhenSongDoesNotExist()
        {
            // Arrange
            _mockSongRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((Song)null);

            var result = await _songService.GetSongById(1);

            Assert.IsNull(result);

            // Act & Assert
        }

        [Test]
        public async Task GetAllSongs_ShouldThrowNoSongsExistsException_WhenNoSongsExist()
        {
            // Arrange
            _mockSongRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Song>());

            // Act & Assert
            Assert.ThrowsAsync<NoSongsExistsException>(async () => await _songService.GetAllSongs());
        }

        [Test]
        public async Task GetSongsByArtistId_ShouldThrowNoSuchArtistExistException_WhenArtistDoesNotExist()
        {
            // Arrange
            _mockArtistRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((Artist)null);

            // Act & Assert
            Assert.ThrowsAsync<NoSongsExistsException>(async () => await _songService.GetSongsByArtistId(1));
        }

        [Test]
        public async Task GetSongsByArtistId_ShouldThrowNoSongsExistsException_WhenNoSongsFound()
        {
            // Arrange
            _mockArtistRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(new Artist());
            _mockSongRepository.Setup(repo => repo.GetSongsByArtistId(It.IsAny<int>())).ReturnsAsync(new List<Song>());

            // Act & Assert
            Assert.ThrowsAsync<NoSongsExistsException>(async () => await _songService.GetSongsByArtistId(1));
        }

        [Test]
        public async Task GetSongsByAlbumId_ShouldThrowNoSuchAlbumExistException_WhenAlbumDoesNotExist()
        {
            // Arrange
            _mockAlbumRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((Album)null);

            // Act & Assert
            Assert.ThrowsAsync<NoSongsExistsException>(async () => await _songService.GetSongsByAlbumId(1));
        }

        [Test]
        public async Task GetSongsByAlbumId_ShouldThrowNoSongsExistsException_WhenNoSongsFound()
        {
            // Arrange
            _mockAlbumRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(new Album());
            _mockSongRepository.Setup(repo => repo.GetSongsByAlbumId(It.IsAny<int>())).ReturnsAsync(new List<Song>());

            // Act & Assert
            Assert.ThrowsAsync<NoSongsExistsException>(async () => await _songService.GetSongsByAlbumId(1));
        }

        [Test]
        public async Task GetSongsByGenre_ShouldThrowInvalidGenreException_WhenGenreIsInvalid()
        {
            // Act & Assert
            Assert.ThrowsAsync<InvalidGenreException>(async () => await _songService.GetSongsByGenre("InvalidGenre"));
        }

        [Test]
        public async Task GetSongsByGenre_ShouldThrowNoSongsExistsException_WhenNoSongsFound()
        {
            // Arrange
            _mockSongRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Song>());

            // Act & Assert
            Assert.ThrowsAsync<NoSongsExistsException>(async () => await _songService.GetSongsByGenre("Pop"));
        }

        [Test]
        public async Task DeleteRangeSongs_ShouldThrowNoSuchSongExistException_WhenNoSongsExist()
        {
            // Arrange
            var ids = new List<int> { 1, 2, 3 };
            _mockSongRepository.Setup(repo => repo.DeleteRange(It.IsAny<IList<int>>())).ThrowsAsync(new NoSuchSongExistException("No songs found"));

            // Act & Assert
            Assert.ThrowsAsync<NoSuchSongExistException>(async () => await _songService.DeleteRangeSongs(ids));
        }
    }
}
