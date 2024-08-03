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
    public class PremiumUserRepositoryTest
    {
        MusicManagementContext context;
        IPremiumUserRepository premiumUserRepository;
        IUserRepository userRepository;

        [SetUp]
        public void SetUp()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MusicManagementContext>()
                                    .UseInMemoryDatabase("dummyPremiumUserRepositoryDB");
            context = new MusicManagementContext(optionsBuilder.Options);
            premiumUserRepository = new PremiumUserRepository(context);
            userRepository = new UserRepository(context);
        }

        [Test, Order(1)]
        public async Task AddPremiumUserSuccess()
        {
            var hmac = new HMACSHA512();
            var user = new User
            {
                Email = "user1@gmail.com",
                PasswordHashKey = hmac.Key,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("password1")),
                Role = RoleType.PremiumUser,
                Status = "Active",
                Phone = "9382938912",
                Username = "Raj"
            };
            await userRepository.Add(user);

            var premiumUser = new PremiumUser
            {
                UserId = 1,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(1),
                Money = (double)9.99
            };
            var addedPremiumUser = await premiumUserRepository.Add(premiumUser);
            Assert.That(addedPremiumUser.UserId, Is.EqualTo(premiumUser.UserId));
            Assert.That(addedPremiumUser.StartDate, Is.EqualTo(premiumUser.StartDate));
            Assert.That(addedPremiumUser.EndDate, Is.EqualTo(premiumUser.EndDate));
            Assert.That(addedPremiumUser.Money, Is.EqualTo(premiumUser.Money));
        }

        [Test, Order(2)]
        public async Task GetAllPremiumUsersSuccess()
        {
            var premiumUsers = await premiumUserRepository.GetAll();
            Assert.That(premiumUsers.Count, Is.EqualTo(1));
        }

        [Test, Order(3)]
        public async Task GetPremiumUserByIdSuccess()
        {
            var premiumUser = await premiumUserRepository.GetById(1);
            Assert.That(premiumUser.UserId, Is.EqualTo(1));
        }

        [Test, Order(4)]
        public async Task UpdatePremiumUserSuccess()
        {
            var premiumUser = await premiumUserRepository.GetById(1);
            premiumUser.Money = 19.99;
            var updatedPremiumUser = await premiumUserRepository.Update(premiumUser);
            Assert.That(updatedPremiumUser.Money, Is.EqualTo(19.99m));
        }

        [Test, Order(5)]
        public async Task DeletePremiumUserSuccess()
        {
            var premiumUser = await premiumUserRepository.Delete(1);
            Assert.That(premiumUser.UserId, Is.EqualTo(1));
            Assert.That(premiumUser.Money, Is.EqualTo(19.99m));
            Assert.IsEmpty(await premiumUserRepository.GetAll());
        }

        [Test, Order(6)]
        public void AddPremiumUserFailure()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await premiumUserRepository.Add(null));
        }

        [Test, Order(7)]
        public async Task GetAllPremiumUsersFailure()
        {
            var premiumUsers = await premiumUserRepository.GetAll();
            Assert.IsEmpty(premiumUsers);
        }

        [Test, Order(8)]
        public async Task GetPremiumUserByIdFailure()
        {
            var result = await premiumUserRepository.GetById(1);
            Assert.IsNull(result);
        }

        [Test, Order(9)]
        public void UpdatePremiumUserFailure()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await premiumUserRepository.Update(null));
        }

        [Test, Order(10)]
        public async Task DeletePremiumUserFailure()
        {
            var result = await premiumUserRepository.Delete(1);
             Assert.IsNull(result);
        }
    }
}
