using Microsoft.EntityFrameworkCore;
using MusicApplicationAPI.Models.DbModels;
using MusicApplicationAPI.Models.Enums;
using System.Security.Cryptography;
using System.Text;

namespace MusicApplicationAPI.Contexts
{
    public class MusicManagementContext : DbContext
    {
        #region Constructor
        public MusicManagementContext(DbContextOptions<MusicManagementContext> options) : base(options)
        {
        }
        #endregion

        #region DbSet Properties

        public DbSet<User> Users { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistSong> PlaylistSongs { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        #endregion

        #region OnModelCreating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            modelBuilder.Entity<PlaylistSong>()
                .HasOne(ps => ps.Playlist)
                .WithMany(p => p.PlaylistSongs)
                .HasForeignKey(ps => ps.PlaylistId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.User)
                .WithMany(u => u.Favorites)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Song)
                .WithMany(s => s.Favorites)
                .HasForeignKey(f => f.SongId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Playlist)
                .WithMany(p => p.Favorites)
                .HasForeignKey(f => f.PlaylistId)
                .OnDelete(DeleteBehavior.SetNull); //TODO

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.User)
                .WithMany(u => u.Ratings)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Song)
                .WithMany(s => s.Ratings)
                .HasForeignKey(r => r.SongId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Song>()
                .HasOne(s => s.Artist)
                .WithMany(a => a.Songs)
                .HasForeignKey(s => s.ArtistId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlaylistSong>()
                .HasOne(ps => ps.Song)
                .WithMany(s => s.PlaylistSongs)
                .HasForeignKey(ps => ps.SongId)
                .OnDelete(DeleteBehavior.Cascade);


            #region Data Seeding

            modelBuilder.Entity<Artist>().HasData(
                new Artist { ArtistId = 1, Name = "Artist One", Bio = "Bio of Artist One", ImageUrl = "http://example.com/artist1.jpg" }
            );

            modelBuilder.Entity<Album>().HasData(
                new Album { AlbumId = 1, Title = "Album One", ArtistId = 1, ReleaseDate = new DateTime(2020, 1, 1), CoverImageUrl = "http://example.com/album1.jpg" }
            );

            modelBuilder.Entity<Song>().HasData(
                new Song { SongId = 1, Title = "Song One", ArtistId = 1, AlbumId = 1, Genre = GenreType.Pop, Duration = 120, ReleaseDate = new DateTime(2020, 1, 1), Url = "http://example.com/song1.mp3" }
            );

            var hmac = new HMACSHA512();

            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    UserId = 101,
                    Username = "Kousik Raj",
                    Email = "kousik@gmail.com",
                    DOB = new DateTime(2000, 01, 01),
                    Role = RoleType.Admin,
                    PasswordHashKey = hmac.Key,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Admin@123")),
                },
                new User()
                {
                    UserId = 102,
                    Username = "Mathew",
                    Email = "mathew@gmail.com",
                    DOB = new DateTime(2003, 01, 01),
                    Role = RoleType.NormalUser,
                    PasswordHashKey = hmac.Key,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Mathew@123")),
                }
            );

            modelBuilder.Entity<Playlist>().HasData(
                new Playlist { PlaylistId = 1, UserId = 102, Name = "Playlist One", IsPublic = true }
            );

            modelBuilder.Entity<PlaylistSong>().HasData(
                new PlaylistSong { PlaylistSongId = 1, PlaylistId = 1, SongId = 1 }
            );

            modelBuilder.Entity<Favorite>().HasData(
                new Favorite { FavoriteId = 1, UserId = 102, SongId = 1 }
            );

            modelBuilder.Entity<Rating>().HasData(
                new Rating { RatingId = 1, UserId = 102, SongId = 1, RatingValue = 5 }
            );

            #endregion
        }
        #endregion
    }
}
