using Microsoft.EntityFrameworkCore;
using MusicApplicationAPI.Contexts;
using MusicApplicationAPI.Exceptions.SongExceptions;
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
    public class SongRepositoryTest
    {
        MusicManagementContext context;
        ISongRepository songRepository;
        IArtistRepository artistRepository;

        [SetUp]
        public void SetUp()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MusicManagementContext>()
                                    .UseInMemoryDatabase("dummySongRepositoryDB");
            context = new MusicManagementContext(optionsBuilder.Options);
            songRepository = new SongRepository(context);
            artistRepository = new ArtistRepository(context);
        }

        [Test, Order(1)]
        public async Task AddSongSuccess()
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
            await artistRepository.Add(artist1);

            var song1 = new Song
            {
                Title = "song1",
                ArtistId = artist1.ArtistId,
                AlbumId = null,
                ImageUrl = "https://dummy-url",
                Url = "https://dummy-url",
                Duration = 190,
                Genre = GenreType.Metal,
                ReleaseDate = new DateTime(2003, 2, 21)
            };
            var addedSong = await songRepository.Add(song1);
            Assert.That(addedSong.Title, Is.EqualTo(song1.Title));
        }

        [Test, Order(2)]
        public async Task GetAllSongsSuccess()
        {
            var songs = await songRepository.GetAll();
            Assert.That(songs.Count, Is.EqualTo(1));
        }

        [Test, Order(3)]
        public async Task GetSongByIdSuccess()
        {
            var song = await songRepository.GetById(1);
            Assert.That(song.Title, Is.EqualTo("song1"));
        }

        [Test, Order(4)]
        public async Task UpdateSongSuccess()
        {
            var song = await songRepository.GetById(1);
            song.Title = "updatedSong";
            var updatedSong = await songRepository.Update(song);
            Assert.That(updatedSong.Title, Is.EqualTo("updatedSong"));
        }

        [Test, Order(5)]
        public async Task DeleteSongSuccess()
        {
            var song = await songRepository.Delete(1);
            Assert.That(song.Title, Is.EqualTo("updatedSong"));
            Assert.IsEmpty(await songRepository.GetAll());
        }

        [Test, Order(6)]
        public void AddSongFailure()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await songRepository.Add(null));
        }

        [Test, Order(7)]
        public async Task GetAllSongsFailure()
        {
            var songs = await songRepository.GetAll();
            Assert.IsEmpty(songs);
        }

        [Test, Order(8)]
        public void GetSongByIdFailure()
        {
            Assert.ThrowsAsync<NoSuchSongExistException>(async () => await songRepository.GetById(1));
        }

        [Test, Order(9)]
        public void UpdateSongFailure()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await songRepository.Update(null));
        }

        [Test, Order(10)]
        public void DeleteSongFailure()
        {
            Assert.ThrowsAsync<NoSuchSongExistException>(async () => await songRepository.Delete(1));
        }
    }
}
