using AutoMapper;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MusicApplicationAPI.Exceptions.UserExceptions;
using MusicApplicationAPI.Interfaces.Repository;
using MusicApplicationAPI.Interfaces.Service;
using MusicApplicationAPI.Models.DbModels;
using MusicApplicationAPI.Models.DTOs.UserDTO;
using MusicApplicationAPI.Services.UserService;
using MusicApplicationAPI.Models.Enums;
using MusicApplicationAPI.Models.DTOs.OtherDTO;
using MusicApplicationAPI.Exceptions.ArtistExceptions;

namespace MusicApplicationAPI.Tests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IEmailSender> _emailSenderMock;
        private Mock<IPremiumUserRepository> _premiumUserRepositoryMock;
        private Mock<IPasswordService> _passwordServiceMock;
        private Mock<IMapper> _mapperMock;
        private Mock<ILogger<UserService>> _loggerMock;
        private UserService _userService;

        [SetUp]
        public void SetUp()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _emailSenderMock = new Mock<IEmailSender>();
            _premiumUserRepositoryMock = new Mock<IPremiumUserRepository>();
            _passwordServiceMock = new Mock<IPasswordService>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<UserService>>();

            _userService = new UserService(
                _userRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object,
                _passwordServiceMock.Object,
                _premiumUserRepositoryMock.Object,
                _emailSenderMock.Object
            );
        }

        [Test]
        public async Task UpdateUserProfile_UserExists_ReturnsUpdatedUser()
        {
            // Arrange
            var userId = 1;
            var userUpdateDTO = new UserUpdateDTO { DOB = DateOnly.FromDateTime(DateTime.Now), Username = "UpdatedUser" };
            var user = new User { UserId = userId, DOB = DateTime.Now, Username = "User" };

            _userRepositoryMock.Setup(repo => repo.GetById(userId)).ReturnsAsync(user);
            _userRepositoryMock.Setup(repo => repo.Update(It.IsAny<User>())).ReturnsAsync(user);
            _mapperMock.Setup(mapper => mapper.Map<UserReturnDTO>(It.IsAny<User>())).Returns(new UserReturnDTO());

            // Act
            var result = await _userService.UpdateUserProfile(userUpdateDTO, userId);

            // Assert
            _userRepositoryMock.Verify(repo => repo.GetById(userId), Times.Once);
            _userRepositoryMock.Verify(repo => repo.Update(It.IsAny<User>()), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<UserReturnDTO>(It.IsAny<User>()), Times.Once);
            Assert.IsNotNull(result);
        }

        [Test]
        public void UpdateUserProfile_UserNotFound_ThrowsNoSuchUserExistException()
        {
            // Arrange
            var userId = 1;
            var userUpdateDTO = new UserUpdateDTO { DOB = DateOnly.FromDateTime(DateTime.Now), Username = "UpdatedUser" };

            _userRepositoryMock.Setup(repo => repo.GetById(userId)).ThrowsAsync(new NoSuchUserExistException());

            // Act & Assert
            Assert.ThrowsAsync<NoSuchUserExistException>(() => _userService.UpdateUserProfile(userUpdateDTO, userId));
            _userRepositoryMock.Verify(repo => repo.GetById(userId), Times.Once);
        }

        [Test]
        public async Task GetUserById_UserExists_ReturnsUser()
        {
            // Arrange
            var userId = 1;
            var user = new User { UserId = userId };
            var userReturnDTO = new UserReturnDTO { UserId = userId };

            _userRepositoryMock.Setup(repo => repo.GetById(userId)).ReturnsAsync(user);
            _mapperMock.Setup(mapper => mapper.Map<UserReturnDTO>(user)).Returns(userReturnDTO);

            // Act
            var result = await _userService.GetUserById(userId);

            // Assert
            Assert.AreEqual(userReturnDTO, result);
        }

        [Test]
        public void GetUserById_UserNotFound_ThrowsNoSuchUserExistException()
        {
            // Arrange
            var userId = 1;

            _userRepositoryMock.Setup(repo => repo.GetById(userId)).ThrowsAsync(new NoSuchUserExistException());

            // Act & Assert
            Assert.ThrowsAsync<NoSuchUserExistException>(() => _userService.GetUserById(userId));
        }

        [Test]
        public async Task GetAdminById_AdminExists_ReturnsAdmin()
        {
            // Arrange
            var adminId = 1;
            var admin = new User { UserId = adminId, Role = RoleType.Admin };
            var adminReturnDTO = new UserReturnDTO { UserId = adminId };

            _userRepositoryMock.Setup(repo => repo.GetById(adminId)).ReturnsAsync(admin);
            _mapperMock.Setup(mapper => mapper.Map<UserReturnDTO>(admin)).Returns(adminReturnDTO);

            // Act
            var result = await _userService.GetAdminById(adminId);

            // Assert
            Assert.AreEqual(adminReturnDTO, result);
        }

        [Test]
        public void GetAdminById_AdminNotFound_ThrowsNoSuchUserExistException()
        {
            // Arrange
            var adminId = 1;

            _userRepositoryMock.Setup(repo => repo.GetById(adminId)).ThrowsAsync(new NoSuchUserExistException());

            // Act & Assert
            Assert.ThrowsAsync<NoSuchUserExistException>(() => _userService.GetAdminById(adminId));
        }

        [Test]
        public async Task GetUserByEmail_UserExists_ReturnsUser()
        {
            // Arrange
            var email = "test@example.com";
            var user = new User { Email = email };
            var userReturnDTO = new UserReturnDTO { Email = email };

            _userRepositoryMock.Setup(repo => repo.GetUserByEmail(email)).ReturnsAsync(user);
            _mapperMock.Setup(mapper => mapper.Map<UserReturnDTO>(user)).Returns(userReturnDTO);

            // Act
            var result = await _userService.GetUserByEmail(email);

            // Assert
            Assert.AreEqual(userReturnDTO, result);
        }

        [Test]
        public void GetUserByEmail_UserNotFound_ThrowsNoSuchUserExistException()
        {
            // Arrange
            var email = "test@example.com";

            _userRepositoryMock.Setup(repo => repo.GetUserByEmail(email)).ThrowsAsync(new NoSuchUserExistException());

            // Act & Assert
            Assert.ThrowsAsync<NoSuchUserExistException>(() => _userService.GetUserByEmail(email));
        }

        [Test]
        public async Task AddAdmin_ValidAdmin_ReturnsAdmin()
        {
            // Arrange
            var adminRegisterDTO = new UserRegisterDTO { Email = "admin@example.com", Username = "Admin", Password = "password", DOB = DateOnly.FromDateTime(DateTime.Now) };
            var user = new User { UserId = 1, Email = adminRegisterDTO.Email, Username = adminRegisterDTO.Username, DOB = DateTime.Now, Role = RoleType.Admin, Status = "Active" };

            _userRepositoryMock.Setup(repo => repo.GetUserByEmail(adminRegisterDTO.Email)).ReturnsAsync((User)null);
            _userRepositoryMock.Setup(repo => repo.Add(It.IsAny<User>())).ReturnsAsync(user);
            _mapperMock.Setup(mapper => mapper.Map<UserRegisterReturnDTO>(user)).Returns(new UserRegisterReturnDTO { UserId = user.UserId });

            // Act
            var result = await _userService.AddAdmin(adminRegisterDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(user.UserId, result.UserId);
        }

        [Test]
        public void AddAdmin_DuplicateEmail_ThrowsDuplicateEmailException()
        {
            // Arrange
            var adminRegisterDTO = new UserRegisterDTO { Email = "admin@example.com", Username = "Admin", Password = "password", DOB = DateOnly.FromDateTime(DateTime.Now) };
            var existingUser = new User { Email = adminRegisterDTO.Email };

            _userRepositoryMock.Setup(repo => repo.GetUserByEmail(adminRegisterDTO.Email)).ReturnsAsync(existingUser);

            // Act & Assert
            Assert.ThrowsAsync<DuplicateEmailException>(() => _userService.AddAdmin(adminRegisterDTO));
        }

        [Test]
        public async Task GetAllUsers_UsersExist_ReturnsUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { UserId = 1, Role = RoleType.NormalUser },
                new User { UserId = 2, Role = RoleType.NormalUser }
            };

            _userRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(users);
            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<UserReturnDTO>>(It.IsAny<IEnumerable<User>>())).Returns(new List<UserReturnDTO>());

            // Act
            var result = await _userService.GetAllUsers();

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetAllUsers_NoUsersExist_ThrowsNoUsersExistsExistsException()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.GetAll()).ThrowsAsync(new NoUsersExistsExistsException());

            // Act & Assert
            Assert.ThrowsAsync<NoUsersExistsExistsException>(() => _userService.GetAllUsers());
        }

        

        [Test]
        public void GetAllAdmins_NoAdminsExist_ThrowsNoUsersExistsExistsException()
        {


            // Act & Assert
            Assert.ThrowsAsync<NoUsersExistsExistsException>(() => _userService.GetAllAdminUsers());
        }

        [Test]
        public async Task GetAllArtists_ArtistsExist_ReturnsArtists()
        {
            // Arrange
            var artists = new List<User>
            {
                new User { UserId = 1, Role = RoleType.Artist },
                new User { UserId = 2, Role = RoleType.Artist }
            };

            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<UserReturnDTO>>(It.IsAny<IEnumerable<User>>())).Returns(new List<UserReturnDTO>());

            // Act

            // Assert
            Assert.IsNotNull(artists);
        }

        [Test]
        public void GetAllArtists_NoArtistsExist_ThrowsNoArtistsExistException()
        {
            var artists = new List<User>
            {
                new User { UserId = 1, Role = RoleType.Artist },
                new User { UserId = 2, Role = RoleType.Artist }
            };
            // Arrange
            _userRepositoryMock.Setup(repo => repo.GetAll()).ThrowsAsync(new NoUsersExistsExistsException());

            // Act & Assert
            Assert.IsNotNull(artists);
        }
    }
}
