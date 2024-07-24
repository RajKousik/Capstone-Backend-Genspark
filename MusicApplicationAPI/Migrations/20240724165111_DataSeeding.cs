using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicApplicationAPI.Migrations
{
    public partial class DataSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Artists",
                columns: new[] { "ArtistId", "Bio", "ImageUrl", "Name" },
                values: new object[] { 1, "Bio of Artist One", "http://example.com/artist1.jpg", "Artist One" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "DOB", "Email", "PasswordHash", "PasswordHashKey", "Role", "Username" },
                values: new object[] { 101, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "kousik@gmail.com", new byte[] { 245, 183, 10, 59, 224, 186, 86, 186, 15, 23, 58, 190, 159, 142, 172, 34, 71, 120, 120, 147, 218, 26, 136, 64, 144, 224, 140, 26, 153, 46, 193, 162, 187, 19, 173, 82, 144, 135, 122, 155, 135, 120, 139, 14, 102, 203, 152, 137, 90, 40, 116, 146, 143, 86, 219, 27, 173, 240, 43, 155, 216, 130, 26, 155 }, new byte[] { 3, 167, 71, 129, 31, 105, 75, 197, 176, 180, 56, 125, 18, 44, 210, 43, 12, 45, 151, 50, 73, 81, 88, 9, 91, 217, 96, 176, 61, 57, 183, 56, 121, 184, 12, 5, 193, 225, 43, 134, 218, 33, 108, 241, 236, 22, 98, 33, 234, 94, 184, 87, 112, 144, 219, 218, 44, 225, 68, 71, 185, 143, 96, 98, 104, 30, 46, 202, 129, 137, 170, 223, 102, 191, 187, 54, 98, 116, 67, 29, 156, 33, 140, 51, 223, 4, 75, 73, 158, 179, 170, 207, 158, 114, 6, 118, 219, 233, 209, 27, 149, 106, 111, 105, 107, 219, 26, 229, 223, 9, 37, 144, 95, 187, 11, 198, 253, 140, 233, 144, 190, 70, 140, 170, 245, 105, 188, 65 }, 1, "Kousik Raj" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "DOB", "Email", "PasswordHash", "PasswordHashKey", "Role", "Username" },
                values: new object[] { 102, new DateTime(2003, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "mathew@gmail.com", new byte[] { 183, 204, 198, 162, 182, 6, 82, 156, 209, 54, 59, 152, 23, 50, 192, 107, 188, 51, 222, 168, 58, 137, 47, 212, 190, 225, 103, 238, 69, 86, 191, 4, 179, 199, 60, 120, 60, 57, 229, 23, 194, 213, 26, 49, 30, 107, 49, 204, 32, 177, 178, 86, 89, 114, 212, 196, 61, 71, 157, 61, 103, 45, 244, 39 }, new byte[] { 3, 167, 71, 129, 31, 105, 75, 197, 176, 180, 56, 125, 18, 44, 210, 43, 12, 45, 151, 50, 73, 81, 88, 9, 91, 217, 96, 176, 61, 57, 183, 56, 121, 184, 12, 5, 193, 225, 43, 134, 218, 33, 108, 241, 236, 22, 98, 33, 234, 94, 184, 87, 112, 144, 219, 218, 44, 225, 68, 71, 185, 143, 96, 98, 104, 30, 46, 202, 129, 137, 170, 223, 102, 191, 187, 54, 98, 116, 67, 29, 156, 33, 140, 51, 223, 4, 75, 73, 158, 179, 170, 207, 158, 114, 6, 118, 219, 233, 209, 27, 149, 106, 111, 105, 107, 219, 26, 229, 223, 9, 37, 144, 95, 187, 11, 198, 253, 140, 233, 144, 190, 70, 140, 170, 245, 105, 188, 65 }, 2, "Mathew" });

            migrationBuilder.InsertData(
                table: "Albums",
                columns: new[] { "AlbumId", "ArtistId", "CoverImageUrl", "ReleaseDate", "Title" },
                values: new object[] { 1, 1, "http://example.com/album1.jpg", new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Album One" });

            migrationBuilder.InsertData(
                table: "Playlists",
                columns: new[] { "PlaylistId", "IsPublic", "Name", "UserId" },
                values: new object[] { 1, true, "Playlist One", 102 });

            migrationBuilder.InsertData(
                table: "Songs",
                columns: new[] { "SongId", "AlbumId", "ArtistId", "Duration", "Genre", "ReleaseDate", "Title", "Url" },
                values: new object[] { 1, 1, 1, new TimeSpan(0, 0, 3, 45, 0), 0, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Song One", "http://example.com/song1.mp3" });

            migrationBuilder.InsertData(
                table: "Favorites",
                columns: new[] { "FavoriteId", "PlaylistId", "SongId", "UserId" },
                values: new object[] { 1, null, 1, 102 });

            migrationBuilder.InsertData(
                table: "PlaylistSongs",
                columns: new[] { "PlaylistSongId", "PlaylistId", "SongId" },
                values: new object[] { 1, 1, 1 });

            migrationBuilder.InsertData(
                table: "Ratings",
                columns: new[] { "RatingId", "RatingValue", "SongId", "UserId" },
                values: new object[] { 1, 5, 1, 102 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Favorites",
                keyColumn: "FavoriteId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PlaylistSongs",
                keyColumn: "PlaylistSongId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Ratings",
                keyColumn: "RatingId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Playlists",
                keyColumn: "PlaylistId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "SongId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Albums",
                keyColumn: "AlbumId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Artists",
                keyColumn: "ArtistId",
                keyValue: 1);
        }
    }
}
