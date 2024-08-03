using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using MusicApplicationAPI.Exceptions.PlaylistExceptions;
using MusicApplicationAPI.Exceptions.UserExceptions;
using MusicApplicationAPI.Interfaces.Repository;
using MusicApplicationAPI.Interfaces.Service;
using MusicApplicationAPI.Models.DbModels;
using MusicApplicationAPI.Models.DTOs.PlaylistDTO;
using MusicApplicationAPI.Models.Enums;
using MusicApplicationAPI.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MusicApplicationAPI.Tests
{
    [TestFixture]
    public class PlaylistServiceTests
    {
        private Mock<IPlaylistRepository> _playlistRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private Mock<ILogger<PlaylistService>> _loggerMock;
        private Mock<IConfiguration> _configurationMock;
        private Mock<IFavoriteRepository> _favoriteRepositoryMock;
        private PlaylistService _playlistService;

        [SetUp]
        public void Setup()
        {
            _playlistRepositoryMock = new Mock<IPlaylistRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<PlaylistService>>();
            _configurationMock = new Mock<IConfiguration>();
            _favoriteRepositoryMock = new Mock<IFavoriteRepository>();

            _playlistService = new PlaylistService(
                _playlistRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object,
                _userRepositoryMock.Object,
                _configurationMock.Object,
                _favoriteRepositoryMock.Object
            );
        }

        

        [Test]
        public async Task UpdatePlaylist_ShouldReturnUpdatedPlaylist()
        {
            // Arrange
            var playlistId = 1;
            var playlistUpdateDTO = new PlaylistUpdateDTO { Name = "Updated Playlist" };
            var playlist = new Playlist { PlaylistId = playlistId };
            _playlistRepositoryMock.Setup(repo => repo.GetById(playlistId)).ReturnsAsync(playlist);
            _mapperMock.Setup(mapper => mapper.Map(It.IsAny<PlaylistUpdateDTO>(), It.IsAny<Playlist>())).Verifiable();
            _playlistRepositoryMock.Setup(repo => repo.Update(It.IsAny<Playlist>())).ReturnsAsync(playlist);
            _mapperMock.Setup(mapper => mapper.Map<PlaylistReturnDTO>(It.IsAny<Playlist>())).Returns(new PlaylistReturnDTO());

            // Act
            var result = await _playlistService.UpdatePlaylist(playlistId, playlistUpdateDTO);

            // Assert
            Assert.NotNull(result);
            _playlistRepositoryMock.Verify(repo => repo.GetById(playlistId), Times.Once);
            _playlistRepositoryMock.Verify(repo => repo.Update(It.IsAny<Playlist>()), Times.Once);
        }

        [Test]
        public async Task GetPlaylistById_ShouldReturnPlaylist()
        {
            // Arrange
            var playlistId = 1;
            var playlist = new Playlist { PlaylistId = playlistId };
            _playlistRepositoryMock.Setup(repo => repo.GetById(playlistId)).ReturnsAsync(playlist);
            _mapperMock.Setup(mapper => mapper.Map<PlaylistReturnDTO>(It.IsAny<Playlist>())).Returns(new PlaylistReturnDTO());

            // Act
            var result = await _playlistService.GetPlaylistById(playlistId);

            // Assert
            Assert.NotNull(result);
            _playlistRepositoryMock.Verify(repo => repo.GetById(playlistId), Times.Once);
        }

        [Test]
        public async Task GetAllPlaylists_ShouldReturnPlaylistList()
        {
            // Arrange
            var playlists = new List<Playlist>
            {
                new Playlist { PlaylistId = 1 },
                new Playlist { PlaylistId = 2 }
            };
            _playlistRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(playlists);
            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<PlaylistReturnDTO>>(It.IsAny<IEnumerable<Playlist>>())).Returns(new List<PlaylistReturnDTO>());

            // Act
            var result = await _playlistService.GetAllPlaylists();

            // Assert
            Assert.NotNull(result);
        }

        [Test]
        public async Task DeletePlaylist_ShouldReturnDeletedPlaylist()
        {
            // Arrange
            var playlistId = 1;
            var playlist = new Playlist { PlaylistId = playlistId };
            _playlistRepositoryMock.Setup(repo => repo.Delete(playlistId)).ReturnsAsync(playlist);
            _mapperMock.Setup(mapper => mapper.Map<PlaylistReturnDTO>(It.IsAny<Playlist>())).Returns(new PlaylistReturnDTO());
            _favoriteRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Favorite> { new Favorite { PlaylistId = playlistId } });

            // Act
            var result = await _playlistService.DeletePlaylist(playlistId);

            // Assert
            Assert.NotNull(result);
            _playlistRepositoryMock.Verify(repo => repo.Delete(playlistId), Times.Once);
            _favoriteRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Test]
        public async Task GetPlaylistsByUserId_ShouldReturnUserPlaylists()
        {
            // Arrange
            var userId = 1;
            var user = new User { UserId = userId };
            var playlists = new List<Playlist>
            {
                new Playlist { PlaylistId = 1, UserId = userId },
                new Playlist { PlaylistId = 2, UserId = userId }
            };
            _userRepositoryMock.Setup(repo => repo.GetById(userId)).ReturnsAsync(user);
            _playlistRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(playlists);
            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<PlaylistReturnDTO>>(It.IsAny<IEnumerable<Playlist>>())).Returns(new List<PlaylistReturnDTO>());

            // Act
            var result = await _playlistService.GetPlaylistsByUserId(userId);

            // Assert
            Assert.NotNull(result);
        }

        [Test]
        public async Task GetPublicPlaylists_ShouldReturnPublicPlaylists()
        {
            // Arrange
            var playlists = new List<Playlist>
            {
                new Playlist { PlaylistId = 1, IsPublic = true },
                new Playlist { PlaylistId = 2, IsPublic = false }
            };
            _playlistRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(playlists);
            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<PlaylistReturnDTO>>(It.IsAny<IEnumerable<Playlist>>())).Returns(new List<PlaylistReturnDTO>());

            // Act
            var result = await _playlistService.GetPublicPlaylists();

            // Assert
            Assert.NotNull(result);
        }
    }
}
