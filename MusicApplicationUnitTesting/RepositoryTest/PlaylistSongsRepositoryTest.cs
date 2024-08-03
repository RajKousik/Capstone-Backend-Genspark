using Microsoft.EntityFrameworkCore;
using MusicApplicationAPI.Contexts;
using MusicApplicationAPI.Exceptions.PlaylistSongExceptions;
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
    public class PlaylistSongsRepositoryTest
    {
        MusicManagementContext context;
        IPlaylistRepository playlistRepository;
        ISongRepository songRepository;
        IPlaylistSongRepository playlistSongsRepository;
        IUserRepository userRepository;
        IArtistRepository artistRepository;

        [SetUp]
        public void SetUp()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MusicManagementContext>()
                                    .UseInMemoryDatabase("dummyPlaylistSongsRepositoryDB");
            context = new MusicManagementContext(optionsBuilder.Options);
            playlistRepository = new PlaylistRepository(context);
            songRepository = new SongRepository(context);
            playlistSongsRepository = new PlaylistSongRepository(context);
            userRepository = new UserRepository(context);
            artistRepository = new ArtistRepository(context);
        }

        [Test, Order(1)]
        public async Task AddPlaylistSongSuccess()
        {
            var hmac = new HMACSHA512();
            var user = new User
            {
                Email = "user1@gmail.com",
                PasswordHashKey = hmac.Key,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("password1")),
                Role = RoleType.NormalUser,
                Status = "Active",
                Phone = "9382938912",
                Username = "Raj"
            };
            await userRepository.Add(user);

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

            var song = new Song
            {
                Title = "song1",
                ArtistId = 1,
                AlbumId = null,
                ImageUrl = "https://dummy-url",
                Url = "https://dummy-url",
                Duration = 190,
                Genre = GenreType.Metal,
                ReleaseDate = new DateTime(2003, 2, 21, 1, 2, 3)
            };
            await songRepository.Add(song);

            var playlist = new Playlist
            {
                Name = "Playlist1",
                UserId = 1,
                ImageUrl = "https://dummy-url",
                IsPublic = true
            };
            await playlistRepository.Add(playlist);

            var playlistSong = new PlaylistSong
            {
                SongId = 1,
                PlaylistId = 1
            };
            var addedPlaylistSong = await playlistSongsRepository.Add(playlistSong);
            Assert.That(addedPlaylistSong.SongId, Is.EqualTo(playlistSong.SongId));
            Assert.That(addedPlaylistSong.PlaylistId, Is.EqualTo(playlistSong.PlaylistId));
        }

        [Test, Order(2)]
        public async Task GetAllPlaylistSongsSuccess()
        {
            var playlistSongs = await playlistSongsRepository.GetAll();
            Assert.That(playlistSongs.Count, Is.EqualTo(1));
        }

        [Test, Order(3)]
        public async Task GetPlaylistSongByIdSuccess()
        {
            var playlistSong = await playlistSongsRepository.GetById(1);
            Assert.That(playlistSong.SongId, Is.EqualTo(1));
            Assert.That(playlistSong.PlaylistId, Is.EqualTo(1));
        }

        [Test, Order(4)]
        public async Task DeletePlaylistSongSuccess()
        {
            var playlistSong = await playlistSongsRepository.Delete(1);
            Assert.That(playlistSong.SongId, Is.EqualTo(1));
            Assert.That(playlistSong.PlaylistId, Is.EqualTo(1));
            Assert.IsEmpty(await playlistSongsRepository.GetAll());
        }

        [Test, Order(5)]
        public void AddPlaylistSongFailure()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await playlistSongsRepository.Add(null));
        }

        [Test, Order(6)]
        public async Task GetAllPlaylistSongsFailure()
        {
            var playlistSongs = await playlistSongsRepository.GetAll();
            Assert.IsEmpty(playlistSongs);
        }

        [Test, Order(7)]
        public void GetPlaylistSongByIdFailure()
        {
            Assert.ThrowsAsync<NoSuchPlaylistSongExistException>(async () => await playlistSongsRepository.GetById(1));
        }

        [Test, Order(8)]
        public void DeletePlaylistSongFailure()
        {
            Assert.ThrowsAsync<NoSuchPlaylistSongExistException>(async () => await playlistSongsRepository.Delete(1));
        }
    }
}

