using Microsoft.EntityFrameworkCore;
using MusicApplicationAPI.Contexts;
using MusicApplicationAPI.Exceptions.RatingExceptions;
using MusicApplicationAPI.Interfaces.Repository;
using MusicApplicationAPI.Models.DbModels;
using MusicApplicationAPI.Models.Enums;
using MusicApplicationAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MusicApplicationUnitTesting.RepositoryTest
{
    public class RatingRepositoryTest
    {
        MusicManagementContext context;
        IRatingRepository ratingRepository;
        IUserRepository userRepository;
        ISongRepository songRepository;
        IArtistRepository artistRepository;


        [SetUp]
        public void SetUp()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MusicManagementContext>()
                                    .UseInMemoryDatabase("dummyRatingRepositoryDB");
            context = new MusicManagementContext(optionsBuilder.Options);
            ratingRepository = new RatingRepository(context);
            userRepository = new UserRepository(context);
            songRepository = new SongRepository(context);
            artistRepository = new ArtistRepository(context);

        }

        [Test, Order(1)]
        public async Task AddRatingSuccess()
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

            var rating = new Rating
            {
                UserId = 1,
                SongId = 1,
                RatingValue = 4
            };
            var addedRating = await ratingRepository.Add(rating);
            Assert.That(addedRating.UserId, Is.EqualTo(rating.UserId));
            Assert.That(addedRating.SongId, Is.EqualTo(rating.SongId));
            Assert.That(addedRating.RatingValue, Is.EqualTo(rating.RatingValue));
        }

        [Test, Order(2)]
        public async Task GetAllRatingsSuccess()
        {
            var ratings = await ratingRepository.GetAll();
            Assert.That(ratings.Count, Is.EqualTo(1));
        }

        [Test, Order(3)]
        public async Task GetRatingByIdSuccess()
        {
            var rating = await ratingRepository.GetById(1);
            Assert.That(rating.UserId, Is.EqualTo(1));
            Assert.That(rating.SongId, Is.EqualTo(1));
            Assert.That(rating.RatingValue, Is.EqualTo(4));
        }

        [Test, Order(4)]
        public async Task UpdateRatingSuccess()
        {
            var rating = await ratingRepository.GetById(1);
            rating.RatingValue = 5;
            var updatedRating = await ratingRepository.Update(rating);
            Assert.That(updatedRating.RatingValue, Is.EqualTo(5));
        }

        [Test, Order(5)]
        public async Task DeleteRatingSuccess()
        {
            var rating = await ratingRepository.Delete(1);
            Assert.That(rating.UserId, Is.EqualTo(1));
            Assert.That(rating.SongId, Is.EqualTo(1));
            Assert.That(rating.RatingValue, Is.EqualTo(5));
            Assert.IsEmpty(await ratingRepository.GetAll());
        }

        [Test, Order(6)]
        public void AddRatingFailure()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await ratingRepository.Add(null));
        }

        [Test, Order(7)]
        public async Task GetAllRatingsFailure()
        {
            var ratings = await ratingRepository.GetAll();
            Assert.IsEmpty(ratings);
        }

        [Test, Order(8)]
        public void GetRatingByIdFailure()
        {
            Assert.ThrowsAsync<NoSuchRatingExistException>(async () => await ratingRepository.GetById(1));
        }

        [Test, Order(9)]
        public void UpdateRatingFailure()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await ratingRepository.Update(null));
        }

        [Test, Order(10)]
        public void DeleteRatingFailure()
        {
            Assert.ThrowsAsync<NoSuchRatingExistException>(async () => await ratingRepository.Delete(1));
        }
    }
}

