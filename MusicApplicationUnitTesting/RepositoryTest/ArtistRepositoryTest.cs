using Microsoft.EntityFrameworkCore;
using MusicApplicationAPI.Contexts;
using MusicApplicationAPI.Exceptions.ArtistExceptions;
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
    public class ArtistRepositoryTest
    {
        MusicManagementContext context;
        IArtistRepository artistRepository;

        [SetUp]
        public void SetUp()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MusicManagementContext>()
                                    .UseInMemoryDatabase("dummyArtistRepositoryDB");
            context = new MusicManagementContext(optionsBuilder.Options);
            artistRepository = new ArtistRepository(context);
        }

        [Test, Order(1)]
        public async Task AddArtistSuccess()
        {
            var hmac = new HMACSHA512();
            var artist1 = new Artist()
            {
                Email = "artist1@gmail.com",
                Name = "Artist1",
                Bio = "some Bio",
                ImageUrl = "https://some-url",
                Status = "Active",
                PasswordHashKey = hmac.Key,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("password1")),
                Role = RoleType.Artist
            };
            var addedArtist = await artistRepository.Add(artist1);
            Assert.That(addedArtist.Email, Is.EqualTo(artist1.Email));
        }

        [Test, Order(2)]
        public async Task GetAllArtistsSuccess()
        {
            var artists = await artistRepository.GetAll();
            Assert.That(artists.Count, Is.EqualTo(1));
        }

        [Test, Order(3)]
        public async Task GetArtistByIdSuccess()
        {
            var artist = await artistRepository.GetById(1);
            Assert.That(artist.Email, Is.EqualTo("artist1@gmail.com"));
        }

        [Test, Order(4)]
        public async Task UpdateArtistSuccess()
        {
            var artist = await artistRepository.GetById(1);
            artist.Email = "updated@gmail.com";
            var updatedArtist = await artistRepository.Update(artist);
            Assert.That(updatedArtist.Email, Is.EqualTo("updated@gmail.com"));
        }

        [Test, Order(5)]
        public async Task DeleteArtistSuccess()
        {
            var artist = await artistRepository.Delete(1);
            Assert.That(artist.Email, Is.EqualTo("updated@gmail.com"));
            Assert.IsEmpty(await artistRepository.GetAll());
        }

        [Test, Order(6)]
        public void AddArtistFailure()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await artistRepository.Add(null));
        }

        [Test, Order(7)]
        public async Task GetAllArtistsFailure()
        {
            var artists = await artistRepository.GetAll();
            Assert.IsEmpty(artists);
        }

        [Test, Order(8)]
        public void GetArtistByIdFailure()
        {
            Assert.ThrowsAsync<NoSuchArtistExistException>(async () => await artistRepository.GetById(1));
        }

        [Test, Order(9)]
        public void UpdateArtistFailure()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await artistRepository.Update(null));
        }

        [Test, Order(10)]
        public void DeleteArtistFailure()
        {
            Assert.ThrowsAsync<NoSuchArtistExistException>(async () => await artistRepository.Delete(1));
        }
    }
}
