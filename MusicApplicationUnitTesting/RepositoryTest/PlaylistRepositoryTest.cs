using Microsoft.EntityFrameworkCore;
using MusicApplicationAPI.Contexts;
using MusicApplicationAPI.Exceptions.PlaylistExceptions;
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
    public class PlaylistRepositoryTest
    {
        MusicManagementContext context;
        IPlaylistRepository playlistRepository;
        IUserRepository userRepository;

        [SetUp]
        public void SetUp()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MusicManagementContext>()
                                    .UseInMemoryDatabase("dummyPlaylistRepositoryDB");
            context = new MusicManagementContext(optionsBuilder.Options);
            playlistRepository = new PlaylistRepository(context);
            userRepository = new UserRepository(context);
        }

        [Test, Order(1)]
        public async Task AddPlaylistSuccess()
        {
            var hmac = new HMACSHA512();
            var user = new User
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
            await userRepository.Add(user);

            var playlist = new Playlist
            {
                Name = "Playlist1",
                UserId = 1,
                ImageUrl = "https://dummy-url",
                IsPublic = true
            };
            var addedPlaylist = await playlistRepository.Add(playlist);
            Assert.That(addedPlaylist.Name, Is.EqualTo(playlist.Name));
        }

        [Test, Order(2)]
        public async Task GetAllPlaylistsSuccess()
        {
            var playlists = await playlistRepository.GetAll();
            Assert.That(playlists.Count, Is.EqualTo(1));
        }

        [Test, Order(3)]
        public async Task GetPlaylistByIdSuccess()
        {
            var playlist = await playlistRepository.GetById(1);
            Assert.That(playlist.Name, Is.EqualTo("Playlist1"));
        }

        [Test, Order(4)]
        public async Task UpdatePlaylistSuccess()
        {
            var playlist = await playlistRepository.GetById(1);
            playlist.Name = "UpdatedPlaylist";
            var updatedPlaylist = await playlistRepository.Update(playlist);
            Assert.That(updatedPlaylist.Name, Is.EqualTo("UpdatedPlaylist"));
        }

        [Test, Order(5)]
        public async Task DeletePlaylistSuccess()
        {
            var playlist = await playlistRepository.Delete(1);
            Assert.That(playlist.Name, Is.EqualTo("UpdatedPlaylist"));
            Assert.IsEmpty(await playlistRepository.GetAll());
        }

        [Test, Order(6)]
        public void AddPlaylistFailure()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await playlistRepository.Add(null));
        }

        [Test, Order(7)]
        public async Task GetAllPlaylistsFailure()
        {
            var playlists = await playlistRepository.GetAll();
            Assert.IsEmpty(playlists);
        }

        [Test, Order(8)]
        public void GetPlaylistByIdFailure()
        {
            Assert.ThrowsAsync<NoSuchPlaylistExistException>(async () => await playlistRepository.GetById(1));
        }

        [Test, Order(9)]
        public void UpdatePlaylistFailure()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await playlistRepository.Update(null));
        }

        [Test, Order(10)]
        public void DeletePlaylistFailure()
        {
            Assert.ThrowsAsync<NoSuchPlaylistExistException>(async () => await playlistRepository.Delete(1));
        }
    }
}
