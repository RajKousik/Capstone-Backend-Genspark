﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MusicApplicationAPI.Contexts;

#nullable disable

namespace MusicApplicationAPI.Migrations
{
    [DbContext(typeof(MusicManagementContext))]
    partial class MusicManagementContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.32")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("MusicApplicationAPI.Models.DbModels.Album", b =>
                {
                    b.Property<int>("AlbumId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AlbumId"), 1L, 1);

                    b.Property<int>("ArtistId")
                        .HasColumnType("int");

                    b.Property<string>("CoverImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("AlbumId");

                    b.HasIndex("ArtistId");

                    b.ToTable("Albums");

                    b.HasData(
                        new
                        {
                            AlbumId = 1,
                            ArtistId = 1,
                            CoverImageUrl = "http://example.com/album1.jpg",
                            ReleaseDate = new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Album One"
                        });
                });

            modelBuilder.Entity("MusicApplicationAPI.Models.DbModels.Artist", b =>
                {
                    b.Property<int>("ArtistId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ArtistId"), 1L, 1);

                    b.Property<string>("Bio")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("ArtistId");

                    b.ToTable("Artists");

                    b.HasData(
                        new
                        {
                            ArtistId = 1,
                            Bio = "Bio of Artist One",
                            ImageUrl = "http://example.com/artist1.jpg",
                            Name = "Artist One"
                        });
                });

            modelBuilder.Entity("MusicApplicationAPI.Models.DbModels.Favorite", b =>
                {
                    b.Property<int>("FavoriteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FavoriteId"), 1L, 1);

                    b.Property<int?>("PlaylistId")
                        .HasColumnType("int");

                    b.Property<int?>("SongId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("FavoriteId");

                    b.HasIndex("PlaylistId");

                    b.HasIndex("SongId");

                    b.HasIndex("UserId");

                    b.ToTable("Favorites");

                    b.HasData(
                        new
                        {
                            FavoriteId = 1,
                            SongId = 1,
                            UserId = 102
                        });
                });

            modelBuilder.Entity("MusicApplicationAPI.Models.DbModels.Playlist", b =>
                {
                    b.Property<int>("PlaylistId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PlaylistId"), 1L, 1);

                    b.Property<bool>("IsPublic")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("PlaylistId");

                    b.HasIndex("UserId");

                    b.ToTable("Playlists");

                    b.HasData(
                        new
                        {
                            PlaylistId = 1,
                            IsPublic = true,
                            Name = "Playlist One",
                            UserId = 102
                        });
                });

            modelBuilder.Entity("MusicApplicationAPI.Models.DbModels.PlaylistSong", b =>
                {
                    b.Property<int>("PlaylistSongId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PlaylistSongId"), 1L, 1);

                    b.Property<int>("PlaylistId")
                        .HasColumnType("int");

                    b.Property<int>("SongId")
                        .HasColumnType("int");

                    b.HasKey("PlaylistSongId");

                    b.HasIndex("PlaylistId");

                    b.HasIndex("SongId");

                    b.ToTable("PlaylistSongs");

                    b.HasData(
                        new
                        {
                            PlaylistSongId = 1,
                            PlaylistId = 1,
                            SongId = 1
                        });
                });

            modelBuilder.Entity("MusicApplicationAPI.Models.DbModels.Rating", b =>
                {
                    b.Property<int>("RatingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RatingId"), 1L, 1);

                    b.Property<int>("RatingValue")
                        .HasColumnType("int");

                    b.Property<int>("SongId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("RatingId");

                    b.HasIndex("SongId");

                    b.HasIndex("UserId");

                    b.ToTable("Ratings");

                    b.HasData(
                        new
                        {
                            RatingId = 1,
                            RatingValue = 5,
                            SongId = 1,
                            UserId = 102
                        });
                });

            modelBuilder.Entity("MusicApplicationAPI.Models.DbModels.Song", b =>
                {
                    b.Property<int>("SongId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SongId"), 1L, 1);

                    b.Property<int?>("AlbumId")
                        .HasColumnType("int");

                    b.Property<int>("ArtistId")
                        .HasColumnType("int");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<int>("Genre")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SongId");

                    b.HasIndex("AlbumId");

                    b.HasIndex("ArtistId");

                    b.ToTable("Songs");

                    b.HasData(
                        new
                        {
                            SongId = 1,
                            AlbumId = 1,
                            ArtistId = 1,
                            Duration = 120,
                            Genre = 0,
                            ReleaseDate = new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Song One",
                            Url = "http://example.com/song1.mp3"
                        });
                });

            modelBuilder.Entity("MusicApplicationAPI.Models.DbModels.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"), 1L, 1);

                    b.Property<DateTime>("DOB")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordHashKey")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = 101,
                            DOB = new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "kousik@gmail.com",
                            PasswordHash = new byte[] { 73, 147, 151, 53, 213, 226, 27, 121, 179, 141, 78, 88, 211, 165, 3, 180, 255, 46, 17, 73, 185, 58, 232, 25, 165, 46, 206, 241, 240, 81, 105, 186, 22, 38, 243, 50, 199, 207, 156, 69, 3, 99, 198, 237, 95, 79, 174, 57, 252, 105, 122, 17, 53, 132, 39, 207, 11, 158, 5, 8, 254, 83, 54, 216 },
                            PasswordHashKey = new byte[] { 122, 215, 188, 91, 175, 177, 223, 185, 150, 215, 203, 221, 198, 155, 116, 210, 159, 163, 83, 171, 154, 71, 24, 185, 62, 114, 63, 36, 162, 30, 178, 248, 255, 211, 28, 56, 213, 56, 200, 190, 80, 4, 32, 129, 212, 195, 148, 4, 111, 173, 224, 47, 8, 71, 85, 112, 220, 238, 27, 225, 12, 238, 157, 21, 1, 226, 10, 209, 249, 244, 226, 9, 81, 55, 107, 227, 252, 171, 115, 180, 72, 249, 141, 229, 199, 90, 102, 200, 67, 119, 75, 15, 70, 202, 242, 139, 4, 154, 242, 129, 99, 11, 39, 192, 98, 137, 59, 196, 34, 155, 185, 76, 114, 230, 211, 7, 220, 237, 152, 149, 77, 32, 23, 220, 193, 48, 140, 91 },
                            Role = 1,
                            Username = "Kousik Raj"
                        },
                        new
                        {
                            UserId = 102,
                            DOB = new DateTime(2003, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "mathew@gmail.com",
                            PasswordHash = new byte[] { 194, 56, 14, 54, 98, 189, 55, 58, 143, 23, 194, 219, 227, 60, 169, 233, 67, 124, 146, 165, 240, 170, 115, 46, 68, 184, 205, 107, 134, 199, 236, 24, 228, 62, 186, 204, 232, 16, 178, 37, 95, 196, 134, 185, 49, 251, 152, 146, 220, 53, 179, 31, 20, 255, 35, 65, 208, 223, 111, 18, 158, 182, 142, 7 },
                            PasswordHashKey = new byte[] { 122, 215, 188, 91, 175, 177, 223, 185, 150, 215, 203, 221, 198, 155, 116, 210, 159, 163, 83, 171, 154, 71, 24, 185, 62, 114, 63, 36, 162, 30, 178, 248, 255, 211, 28, 56, 213, 56, 200, 190, 80, 4, 32, 129, 212, 195, 148, 4, 111, 173, 224, 47, 8, 71, 85, 112, 220, 238, 27, 225, 12, 238, 157, 21, 1, 226, 10, 209, 249, 244, 226, 9, 81, 55, 107, 227, 252, 171, 115, 180, 72, 249, 141, 229, 199, 90, 102, 200, 67, 119, 75, 15, 70, 202, 242, 139, 4, 154, 242, 129, 99, 11, 39, 192, 98, 137, 59, 196, 34, 155, 185, 76, 114, 230, 211, 7, 220, 237, 152, 149, 77, 32, 23, 220, 193, 48, 140, 91 },
                            Role = 2,
                            Username = "Mathew"
                        });
                });

            modelBuilder.Entity("MusicApplicationAPI.Models.DbModels.Album", b =>
                {
                    b.HasOne("MusicApplicationAPI.Models.DbModels.Artist", "Artist")
                        .WithMany("Albums")
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Artist");
                });

            modelBuilder.Entity("MusicApplicationAPI.Models.DbModels.Favorite", b =>
                {
                    b.HasOne("MusicApplicationAPI.Models.DbModels.Playlist", "Playlist")
                        .WithMany("Favorites")
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("MusicApplicationAPI.Models.DbModels.Song", "Song")
                        .WithMany("Favorites")
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MusicApplicationAPI.Models.DbModels.User", "User")
                        .WithMany("Favorites")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Playlist");

                    b.Navigation("Song");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MusicApplicationAPI.Models.DbModels.Playlist", b =>
                {
                    b.HasOne("MusicApplicationAPI.Models.DbModels.User", "User")
                        .WithMany("Playlists")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MusicApplicationAPI.Models.DbModels.PlaylistSong", b =>
                {
                    b.HasOne("MusicApplicationAPI.Models.DbModels.Playlist", "Playlist")
                        .WithMany("PlaylistSongs")
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MusicApplicationAPI.Models.DbModels.Song", "Song")
                        .WithMany("PlaylistSongs")
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Playlist");

                    b.Navigation("Song");
                });

            modelBuilder.Entity("MusicApplicationAPI.Models.DbModels.Rating", b =>
                {
                    b.HasOne("MusicApplicationAPI.Models.DbModels.Song", "Song")
                        .WithMany("Ratings")
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MusicApplicationAPI.Models.DbModels.User", "User")
                        .WithMany("Ratings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Song");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MusicApplicationAPI.Models.DbModels.Song", b =>
                {
                    b.HasOne("MusicApplicationAPI.Models.DbModels.Album", "Album")
                        .WithMany("Songs")
                        .HasForeignKey("AlbumId");

                    b.HasOne("MusicApplicationAPI.Models.DbModels.Artist", "Artist")
                        .WithMany("Songs")
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Album");

                    b.Navigation("Artist");
                });

            modelBuilder.Entity("MusicApplicationAPI.Models.DbModels.Album", b =>
                {
                    b.Navigation("Songs");
                });

            modelBuilder.Entity("MusicApplicationAPI.Models.DbModels.Artist", b =>
                {
                    b.Navigation("Albums");

                    b.Navigation("Songs");
                });

            modelBuilder.Entity("MusicApplicationAPI.Models.DbModels.Playlist", b =>
                {
                    b.Navigation("Favorites");

                    b.Navigation("PlaylistSongs");
                });

            modelBuilder.Entity("MusicApplicationAPI.Models.DbModels.Song", b =>
                {
                    b.Navigation("Favorites");

                    b.Navigation("PlaylistSongs");

                    b.Navigation("Ratings");
                });

            modelBuilder.Entity("MusicApplicationAPI.Models.DbModels.User", b =>
                {
                    b.Navigation("Favorites");

                    b.Navigation("Playlists");

                    b.Navigation("Ratings");
                });
#pragma warning restore 612, 618
        }
    }
}
