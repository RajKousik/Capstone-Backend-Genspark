using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using MusicApplicationAPI.Interfaces.Repository;
using MusicApplicationAPI.Interfaces.Service;
using MusicApplicationAPI.Interfaces.Service.TokenService;
using MusicApplicationAPI.Models.DbModels;
using MusicApplicationAPI.Models.DTOs.ArtistDTO;
using MusicApplicationAPI.Models.DTOs.OtherDTO;
using MusicApplicationAPI.Models.DTOs.SongDTO;
using MusicApplicationAPI.Services;
using MusicApplicationAPI.Services.SongService;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicApplicationUnitTesting.ServiceTest
{
    internal class ArtistServiceTest
    {
        private Mock<IArtistRepository> _artistRepositoryMock;
        private Mock<ISongRepository> _songRepositoryMock;
        private Mock<IPasswordService> _passwordServiceMock;
        private Mock<ITokenService> _tokenServiceMock;
        private Mock<IMapper> _mapperMock;
        private Mock<ILogger<ArtistService>> _loggerMock;
        private IArtistService _artistService;

        [SetUp]
        public void Setup()
        {
            _artistRepositoryMock = new Mock<IArtistRepository>();
            _songRepositoryMock = new Mock<ISongRepository>();
            _passwordServiceMock = new Mock<IPasswordService>();
            _tokenServiceMock = new Mock<ITokenService>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<ArtistService>>();

            _artistService = new ArtistService(
                _artistRepositoryMock.Object,
                _songRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object,
                _passwordServiceMock.Object,
                _tokenServiceMock.Object
            );
        }

        [Test]
        public async Task AddArtist_ShouldReturnArtistReturnDTO()
        {
            // Arrange
            var artistAddDTO = new ArtistAddDTO
            {
                Name = "New Artist",
                Email = "newartist@gmail.com",
                Password = "password123",
                Bio = "New artist bio",
                ImageUrl = "https://some-url"
            };

            var artist = new Artist
            {
                ArtistId = 1,
                Name = artistAddDTO.Name,
                Email = artistAddDTO.Email,
                Bio = artistAddDTO.Bio,
                Status = "Inactive",
                ImageUrl = artistAddDTO.ImageUrl
            };

            _artistRepositoryMock.Setup(repo => repo.Add(It.IsAny<Artist>())).ReturnsAsync(artist);
            _mapperMock.Setup(mapper => mapper.Map<ArtistReturnDTO>(artist)).Returns(new ArtistReturnDTO { ArtistId = artist.ArtistId });

            // Act
            var result = await _artistService.AddArtist(artistAddDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(artist.ArtistId, result.ArtistId);
            _artistRepositoryMock.Verify(repo => repo.Add(It.IsAny<Artist>()), Times.Once);
        }

        [Test]
        public async Task Login_ShouldReturnArtistLoginReturnDTO()
        {
            // Arrange
            var artistLoginDTO = new ArtistLoginDTO
            {
                Email = "artist@gmail.com",
                Password = "password123"
            };

            var artist = new Artist
            {
                ArtistId = 1,
                Email = artistLoginDTO.Email,
                PasswordHash = new byte[] { /* hashed password */ },
                PasswordHashKey = new byte[] { /* key */ },
                Status = "Active"
            };

            _artistRepositoryMock.Setup(repo => repo.GetArtistByEmail(artistLoginDTO.Email)).ReturnsAsync(artist);
            _passwordServiceMock.Setup(service => service.VerifyPassword(artistLoginDTO.Password, artist.PasswordHash, artist.PasswordHashKey)).Returns(true);
            _tokenServiceMock.Setup(service => service.GenerateArtistToken(artist)).Returns("token");

            // Act
            var result = await _artistService.Login(artistLoginDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(artist.ArtistId, result.ArtistId);
            _artistRepositoryMock.Verify(repo => repo.GetArtistByEmail(artistLoginDTO.Email), Times.Once);
        }

        [Test]
        public async Task ActivateArtist_ShouldReturnArtistReturnDTO()
        {
            // Arrange
            var artistId = 1;
            var artist = new Artist
            {
                ArtistId = artistId,
                Status = "Inactive"
            };

            _artistRepositoryMock.Setup(repo => repo.GetById(artistId)).ReturnsAsync(artist);
            _artistRepositoryMock.Setup(repo => repo.Update(artist)).ReturnsAsync(artist);
            _mapperMock.Setup(mapper => mapper.Map<ArtistReturnDTO>(artist)).Returns(new ArtistReturnDTO { ArtistId = artist.ArtistId });

            // Act
            var result = await _artistService.ActivateArtist(artistId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(artist.ArtistId, result.ArtistId);
            _artistRepositoryMock.Verify(repo => repo.Update(artist), Times.Once);
        }

        [Test]
        public async Task Register_ShouldReturnArtistReturnDTO()
        {
            // Arrange
            var artistAddDTO = new ArtistAddDTO
            {
                Name = "New Artist",
                Email = "newartist@gmail.com",
                Password = "password123",
                Bio = "New artist bio",
                ImageUrl = "https://some-url"
            };

            var artist = new Artist
            {
                ArtistId = 1,
                Name = artistAddDTO.Name,
                Email = artistAddDTO.Email,
                Bio = artistAddDTO.Bio,
                Status = "Inactive",
                ImageUrl = artistAddDTO.ImageUrl
            };

            _artistRepositoryMock.Setup(repo => repo.GetArtistByEmail(artistAddDTO.Email)).ReturnsAsync((Artist)null);
            _artistRepositoryMock.Setup(repo => repo.GetArtistByName(artistAddDTO.Name)).ReturnsAsync((Artist)null);
            _passwordServiceMock.Setup(service => service.HashPassword(artistAddDTO.Password, out It.Ref<byte[]>.IsAny)).Returns(new byte[] { /* hashed password */ });
            _artistRepositoryMock.Setup(repo => repo.Add(It.IsAny<Artist>())).ReturnsAsync(artist);
            _mapperMock.Setup(mapper => mapper.Map<ArtistReturnDTO>(artist)).Returns(new ArtistReturnDTO { ArtistId = artist.ArtistId });

            // Act
            var result = await _artistService.Register(artistAddDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(artist.ArtistId, result.ArtistId);
            _artistRepositoryMock.Verify(repo => repo.Add(It.IsAny<Artist>()), Times.Once);
        }

        [Test]
        public async Task UpdateArtist_ShouldReturnUpdatedArtist()
        {
            // Arrange
            var artistId = 1;
            var artistUpdateDTO = new ArtistUpdateDTO
            {
                Name = "Updated Artist",
                Bio = "Updated bio",
                ImageUrl = "https://updated-url"
            };

            var existingArtist = new Artist
            {
                ArtistId = artistId,
                Name = "Original Artist",
                Bio = "Original bio",
                ImageUrl = "https://original-url"
            };

            _artistRepositoryMock.Setup(repo => repo.GetById(artistId)).ReturnsAsync(existingArtist);
            _mapperMock.Setup(mapper => mapper.Map(artistUpdateDTO, existingArtist)).Returns(existingArtist);
            _artistRepositoryMock.Setup(repo => repo.Update(existingArtist)).ReturnsAsync(existingArtist);

            // Act
            var result = await _artistService.UpdateArtist(artistId, artistUpdateDTO);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetArtistById_ShouldReturnArtistReturnDTO()
        {
            // Arrange
            var artistId = 1;
            var artist = new Artist
            {
                ArtistId = artistId,
                Name = "Artist",
                Bio = "Artist bio",
                ImageUrl = "https://artist-url"
            };

            _artistRepositoryMock.Setup(repo => repo.GetById(artistId)).ReturnsAsync(artist);
            _mapperMock.Setup(mapper => mapper.Map<ArtistReturnDTO>(artist)).Returns(new ArtistReturnDTO { ArtistId = artist.ArtistId });

            // Act
            var result = await _artistService.GetArtistById(artistId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(artist.ArtistId, result.ArtistId);
            _artistRepositoryMock.Verify(repo => repo.GetById(artistId), Times.Once);
        }

        [Test]
        public async Task GetAllArtists_ShouldReturnListOfArtistReturnDTO()
        {
            // Arrange
            var artists = new List<Artist>
            {
                new Artist { ArtistId = 1, Name = "Artist1" },
                new Artist { ArtistId = 2, Name = "Artist2" }
            };

            _artistRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(artists);
            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<ArtistReturnDTO>>(artists)).Returns(new List<ArtistReturnDTO>
            {
                new ArtistReturnDTO { ArtistId = 1 },
                new ArtistReturnDTO { ArtistId = 2 }
            });

            // Act
            var result = await _artistService.GetAllArtists();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            _artistRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Test]
        public async Task DeleteArtist_ShouldReturnArtistReturnDTO()
        {
            // Arrange
            var artistId = 1;
            var artist = new Artist
            {
                ArtistId = artistId,
                Name = "Artist to delete"
            };

            _artistRepositoryMock.Setup(repo => repo.GetById(artistId)).ReturnsAsync(artist);
            _artistRepositoryMock.Setup(repo => repo.Delete(artistId)).ReturnsAsync(artist);
            _mapperMock.Setup(mapper => mapper.Map<ArtistReturnDTO>(artist)).Returns(new ArtistReturnDTO { ArtistId = artist.ArtistId });

            // Act
            var result = await _artistService.DeleteArtist(artistId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(artist.ArtistId, result.ArtistId);
            _artistRepositoryMock.Verify(repo => repo.Delete(artistId), Times.Once);
        }

        [Test]
        public async Task GetSongsByArtist_ShouldReturnListOfSongReturnDTO()
        {
            // Arrange
            var artistId = 1;
            var songs = new List<SongReturnDTO>
            {
                new SongReturnDTO { Title = "Song1", ArtistId = artistId },
                new SongReturnDTO { Title = "Song2", ArtistId = artistId }
            };

            _artistRepositoryMock.Setup(repo => repo.GetById(artistId)).ReturnsAsync(new Artist { ArtistId = artistId });
            _songRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Song> { new Song { ArtistId = artistId } });
            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<SongReturnDTO>>(It.IsAny<IEnumerable<Song>>())).Returns(songs);

            // Act
            var result = await _artistService.GetSongsByArtist(artistId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            _artistRepositoryMock.Verify(repo => repo.GetById(artistId), Times.Once);
        }

        [Test]
        public async Task ChangePassword_ShouldReturnTrue()
        {
            // Arrange
            var requestDTO = new ChangePasswordRequestDTO
            {
                CurrentPassword = "oldPassword",
                NewPassword = "newPassword"
            };

            var artistId = 1;
            var artist = new Artist
            {
                ArtistId = artistId,
                PasswordHash = new byte[] { /* hashed password */ },
                PasswordHashKey = new byte[] { /* key */ }
            };

            _artistRepositoryMock.Setup(repo => repo.GetById(artistId)).ReturnsAsync(artist);
            _passwordServiceMock.Setup(service => service.VerifyPassword(requestDTO.CurrentPassword, artist.PasswordHash, artist.PasswordHashKey)).Returns(true);
            _passwordServiceMock.Setup(service => service.HashPassword(requestDTO.NewPassword, out It.Ref<byte[]>.IsAny)).Returns(new byte[] { /* hashed new password */ });
            _artistRepositoryMock.Setup(repo => repo.Update(artist)).ReturnsAsync(artist);

            // Act
            var result = await _artistService.ChangePassword(requestDTO, artistId);

            // Assert
            Assert.IsTrue(result);
            _artistRepositoryMock.Verify(repo => repo.Update(artist), Times.Once);
        }
    }
}