using Microsoft.EntityFrameworkCore;
using MusicApplicationAPI.Contexts;
using MusicApplicationAPI.Exceptions.AlbumExceptions;
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
    public class AlbumRepositoryTest
    {
        MusicManagementContext context;
        IArtistRepository artistRepository;
        IAlbumRepository albumRepository;

        [SetUp]
        public void SetUp()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MusicManagementContext>()
                                    .UseInMemoryDatabase("dummyAlbumRepositoryDB");
            context = new MusicManagementContext(optionsBuilder.Options);
            artistRepository = new ArtistRepository(context);
            albumRepository = new AlbumRepository(context);
        }

        [Test, Order(1)]
        public async Task AddAlbumSuccess()
        {
            var hmac = new HMACSHA512();
            var artist = new Artist
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
            await artistRepository.Add(artist);

            var album = new Album
            {
                Title = "Album1",
                ArtistId = 1,
                ReleaseDate = new DateTime(2000, 1, 1),
                CoverImageUrl = "https://dummy-cover-url"
            };
            var addedAlbum = await albumRepository.Add(album);
            Assert.That(addedAlbum.Title, Is.EqualTo(album.Title));
        }

        [Test, Order(2)]
        public async Task GetAllAlbumsSuccess()
        {
            var albums = await albumRepository.GetAll();
            Assert.That(albums.Count, Is.EqualTo(1));
        }

        [Test, Order(3)]
        public async Task GetAlbumByIdSuccess()
        {
            var album = await albumRepository.GetById(1);
            Assert.That(album.Title, Is.EqualTo("Album1"));
        }

        [Test, Order(4)]
        public async Task UpdateAlbumSuccess()
        {
            var album = await albumRepository.GetById(1);
            album.Title = "Updated Album Title";
            var updatedAlbum = await albumRepository.Update(album);
            Assert.That(updatedAlbum.Title, Is.EqualTo("Updated Album Title"));
        }

        [Test, Order(5)]
        public async Task DeleteAlbumSuccess()
        {
            var album = await albumRepository.Delete(1);
            Assert.That(album.Title, Is.EqualTo("Updated Album Title"));
            Assert.IsEmpty(await albumRepository.GetAll());
        }

        [Test, Order(6)]
        public void AddAlbumFailure()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await albumRepository.Add(null));
        }

        [Test, Order(7)]
        public async Task GetAllAlbumsFailure()
        {
            var albums = await albumRepository.GetAll();
            Assert.IsEmpty(albums);
        }

        [Test, Order(8)]
        public void GetAlbumByIdFailure()
        {
            Assert.ThrowsAsync<NoSuchAlbumExistException>(async () => await albumRepository.GetById(1));
        }

        [Test, Order(9)]
        public void UpdateAlbumFailure()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await albumRepository.Update(null));
        }

        [Test, Order(10)]
        public void DeleteAlbumFailure()
        {
            Assert.ThrowsAsync<NoSuchAlbumExistException>(async () => await albumRepository.Delete(1));
        }
    }
}
