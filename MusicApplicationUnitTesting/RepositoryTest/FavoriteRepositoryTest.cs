using Microsoft.EntityFrameworkCore;
using MusicApplicationAPI.Contexts;
using MusicApplicationAPI.Exceptions.FavoriteExceptions;
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
    public class FavoriteRepositoryTest
    {
        MusicManagementContext context;
        IFavoriteRepository favoriteRepository;
        IUserRepository userRepository;
        ISongRepository songRepository;
        IArtistRepository artistRepository;

        [SetUp]
        public void SetUp()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MusicManagementContext>()
                                    .UseInMemoryDatabase("dummyFavoriteRepositoryDB");
            context = new MusicManagementContext(optionsBuilder.Options);
            favoriteRepository = new FavoriteRepository(context);
            userRepository = new UserRepository(context);
            songRepository = new SongRepository(context);
            artistRepository = new ArtistRepository(context);
        }

        [Test, Order(1)]
        public async Task AddFavoriteSuccess()
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

            var favorite = new Favorite
            {
                SongId = 1,
                UserId = 1
            };
            var addedFavorite = await favoriteRepository.Add(favorite);
            Assert.That(addedFavorite.SongId, Is.EqualTo(favorite.SongId));
            Assert.That(addedFavorite.UserId, Is.EqualTo(favorite.UserId));
        }

        [Test, Order(2)]
        public async Task GetAllFavoritesSuccess()
        {
            var favorites = await favoriteRepository.GetAll();
            Assert.That(favorites.Count, Is.EqualTo(1));
        }

        [Test, Order(3)]
        public async Task GetFavoriteByIdSuccess()
        {
            var favorite = await favoriteRepository.GetById(1);
            Assert.That(favorite.SongId, Is.EqualTo(1));
            Assert.That(favorite.UserId, Is.EqualTo(1));
        }

        [Test, Order(4)]
        public async Task DeleteFavoriteSuccess()
        {
            var favorite = await favoriteRepository.Delete(1);
            Assert.That(favorite.SongId, Is.EqualTo(1));
            Assert.That(favorite.UserId, Is.EqualTo(1));
            Assert.IsEmpty(await favoriteRepository.GetAll());
        }

        [Test, Order(5)]
        public void AddFavoriteFailure()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await favoriteRepository.Add(null));
        }

        [Test, Order(6)]
        public async Task GetAllFavoritesFailure()
        {
            var favorites = await favoriteRepository.GetAll();
            Assert.IsEmpty(favorites);
        }

        [Test, Order(7)]
        public void GetFavoriteByIdFailure()
        {
            Assert.ThrowsAsync<NoSuchFavoriteExistException>(async () => await favoriteRepository.GetById(1));
        }

        [Test, Order(8)]
        public void DeleteFavoriteFailure()
        {
            Assert.ThrowsAsync<NoSuchFavoriteExistException>(async () => await favoriteRepository.Delete(1));
        }
    }
}
