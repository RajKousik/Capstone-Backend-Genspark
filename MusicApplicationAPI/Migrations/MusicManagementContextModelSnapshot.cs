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
                            PasswordHash = new byte[] { 81, 161, 132, 150, 15, 38, 9, 89, 218, 229, 115, 72, 133, 115, 9, 197, 82, 163, 91, 43, 38, 231, 225, 34, 237, 170, 102, 148, 54, 242, 162, 189, 81, 193, 155, 16, 75, 140, 62, 247, 171, 252, 140, 211, 205, 118, 244, 229, 114, 102, 69, 136, 74, 9, 212, 71, 225, 219, 27, 40, 77, 121, 189, 85 },
                            PasswordHashKey = new byte[] { 89, 86, 200, 105, 147, 185, 15, 225, 66, 191, 170, 21, 126, 210, 166, 226, 200, 172, 38, 230, 196, 184, 203, 204, 138, 226, 156, 214, 204, 9, 252, 30, 136, 97, 23, 85, 12, 159, 158, 116, 169, 248, 22, 207, 221, 73, 66, 254, 234, 60, 21, 95, 157, 250, 86, 26, 0, 167, 122, 198, 178, 180, 161, 63, 167, 186, 197, 30, 234, 120, 93, 132, 3, 78, 184, 120, 217, 246, 16, 86, 65, 130, 75, 120, 243, 148, 218, 235, 18, 34, 20, 39, 2, 79, 170, 138, 208, 72, 93, 237, 59, 17, 178, 159, 78, 234, 32, 113, 79, 69, 105, 111, 144, 198, 123, 135, 180, 76, 152, 156, 137, 62, 167, 187, 194, 53, 168, 241 },
                            Role = 1,
                            Username = "Kousik Raj"
                        },
                        new
                        {
                            UserId = 102,
                            DOB = new DateTime(2003, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "mathew@gmail.com",
                            PasswordHash = new byte[] { 78, 65, 119, 12, 24, 29, 224, 1, 38, 231, 202, 247, 87, 96, 199, 170, 222, 141, 223, 108, 233, 152, 243, 189, 230, 129, 155, 52, 195, 104, 187, 134, 58, 136, 16, 170, 43, 248, 130, 184, 68, 79, 166, 41, 19, 143, 226, 106, 239, 174, 244, 131, 128, 25, 103, 16, 186, 104, 229, 38, 58, 241, 9, 36 },
                            PasswordHashKey = new byte[] { 89, 86, 200, 105, 147, 185, 15, 225, 66, 191, 170, 21, 126, 210, 166, 226, 200, 172, 38, 230, 196, 184, 203, 204, 138, 226, 156, 214, 204, 9, 252, 30, 136, 97, 23, 85, 12, 159, 158, 116, 169, 248, 22, 207, 221, 73, 66, 254, 234, 60, 21, 95, 157, 250, 86, 26, 0, 167, 122, 198, 178, 180, 161, 63, 167, 186, 197, 30, 234, 120, 93, 132, 3, 78, 184, 120, 217, 246, 16, 86, 65, 130, 75, 120, 243, 148, 218, 235, 18, 34, 20, 39, 2, 79, 170, 138, 208, 72, 93, 237, 59, 17, 178, 159, 78, 234, 32, 113, 79, 69, 105, 111, 144, 198, 123, 135, 180, 76, 152, 156, 137, 62, 167, 187, 194, 53, 168, 241 },
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
                        .OnDelete(DeleteBehavior.Restrict)
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
