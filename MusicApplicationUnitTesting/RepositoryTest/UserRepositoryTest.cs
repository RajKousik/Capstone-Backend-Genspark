using Microsoft.EntityFrameworkCore;
using MusicApplicationAPI.Contexts;
using MusicApplicationAPI.Exceptions.UserExceptions;
using MusicApplicationAPI.Interfaces.Repository;
using MusicApplicationAPI.Models.DbModels;
using MusicApplicationAPI.Models.Enums;
using MusicApplicationAPI.Repositories;
using NUnit.Framework;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MusicApplicationUnitTesting.RepositoryTest
{
    public class UserRepositoryTest
    {
        MusicManagementContext context;
        IUserRepository userRepository;

        [SetUp]
        public void SetUp()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MusicManagementContext>()
                                    .UseInMemoryDatabase("dummyUserRepositoryDB");
            context = new MusicManagementContext(optionsBuilder.Options);
            userRepository = new UserRepository(context);
        }

        [Test, Order(1)]
        public async Task AddUserSuccess()
        {
            var hmac = new HMACSHA512();
            var user1 = new User
            {
                Username = "Raj Kousik",
                Email = "user1@gmail.com",
                DOB = new DateTime(2000, 01, 01),
                Status = "Active",
                Phone = "9890876751",
                Role = RoleType.NormalUser,
                PasswordHashKey = hmac.Key,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("password1")),
            };
            var addedUser = await userRepository.Add(user1);
            Assert.That(addedUser.Email, Is.EqualTo(user1.Email));
        }

        [Test, Order(2)]
        public async Task GetAllUsersSuccess()
        {
            var users = await userRepository.GetAll();
            Assert.That(users.Count, Is.EqualTo(1));
        }

        [Test, Order(3)]
        public async Task GetUserByIdSuccess()
        {
            var user = await userRepository.GetById(1);
            Assert.That(user.Email, Is.EqualTo("user1@gmail.com"));
        }

        [Test, Order(4)]
        public async Task UpdateUserSuccess()
        {
            var user = await userRepository.GetById(1);
            user.Email = "updated@gmail.com";
            var updatedUser = await userRepository.Update(user);
            Assert.That(updatedUser.Email, Is.EqualTo("updated@gmail.com"));
        }

        [Test, Order(5)]
        public async Task DeleteUserSuccess()
        {
            var user = await userRepository.Delete(1);
            Assert.That(user.Email, Is.EqualTo("updated@gmail.com"));
            Assert.IsEmpty(await userRepository.GetAll());
        }

        [Test, Order(6)]
        public void AddUserFailure()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await userRepository.Add(null));
        }

        [Test, Order(7)]
        public async Task GetAllUsersFailure()
        {
            var users = await userRepository.GetAll();
            Assert.IsEmpty(users);
        }

        [Test, Order(8)]
        public void GetUserByIdFailure()
        {
            Assert.ThrowsAsync<NoSuchUserExistException>(async () => await userRepository.GetById(1));
        }

        [Test, Order(9)]
        public void UpdateUserFailure()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await userRepository.Update(null));
        }

        [Test, Order(10)]
        public void DeleteUserFailure()
        {
            Assert.ThrowsAsync<NoSuchUserExistException>(async () => await userRepository.Delete(1));
        }
    }
}
