using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicApplicationAPI.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    ArtistId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.ArtistId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordHashKey = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Albums",
                columns: table => new
                {
                    AlbumId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ArtistId = table.Column<int>(type: "int", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CoverImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albums", x => x.AlbumId);
                    table.ForeignKey(
                        name: "FK_Albums_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "ArtistId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Playlists",
                columns: table => new
                {
                    PlaylistId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Playlists", x => x.PlaylistId);
                    table.ForeignKey(
                        name: "FK_Playlists_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Songs",
                columns: table => new
                {
                    SongId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ArtistId = table.Column<int>(type: "int", nullable: false),
                    AlbumId = table.Column<int>(type: "int", nullable: true),
                    Genre = table.Column<int>(type: "int", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Songs", x => x.SongId);
                    table.ForeignKey(
                        name: "FK_Songs_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "AlbumId");
                    table.ForeignKey(
                        name: "FK_Songs_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "ArtistId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    FavoriteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SongId = table.Column<int>(type: "int", nullable: true),
                    PlaylistId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.FavoriteId);
                    table.ForeignKey(
                        name: "FK_Favorites_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "Playlists",
                        principalColumn: "PlaylistId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Favorites_Songs_SongId",
                        column: x => x.SongId,
                        principalTable: "Songs",
                        principalColumn: "SongId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Favorites_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlaylistSongs",
                columns: table => new
                {
                    PlaylistSongId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlaylistId = table.Column<int>(type: "int", nullable: false),
                    SongId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistSongs", x => x.PlaylistSongId);
                    table.ForeignKey(
                        name: "FK_PlaylistSongs_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "Playlists",
                        principalColumn: "PlaylistId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaylistSongs_Songs_SongId",
                        column: x => x.SongId,
                        principalTable: "Songs",
                        principalColumn: "SongId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    RatingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SongId = table.Column<int>(type: "int", nullable: false),
                    RatingValue = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.RatingId);
                    table.ForeignKey(
                        name: "FK_Ratings_Songs_SongId",
                        column: x => x.SongId,
                        principalTable: "Songs",
                        principalColumn: "SongId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ratings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Artists",
                columns: new[] { "ArtistId", "Bio", "ImageUrl", "Name" },
                values: new object[] { 1, "Bio of Artist One", "http://example.com/artist1.jpg", "Artist One" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "DOB", "Email", "PasswordHash", "PasswordHashKey", "Role", "Username" },
                values: new object[] { 101, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "kousik@gmail.com", new byte[] { 188, 138, 20, 213, 242, 13, 239, 95, 129, 213, 45, 197, 0, 253, 84, 24, 82, 132, 113, 160, 32, 147, 178, 159, 167, 251, 182, 118, 1, 186, 103, 187, 178, 18, 147, 105, 236, 1, 60, 52, 203, 104, 143, 83, 47, 159, 61, 60, 147, 103, 244, 223, 246, 179, 72, 227, 83, 211, 113, 245, 208, 232, 145, 176 }, new byte[] { 57, 134, 191, 82, 188, 97, 218, 62, 178, 159, 107, 128, 131, 64, 208, 163, 107, 183, 240, 250, 27, 201, 16, 102, 172, 199, 222, 150, 254, 80, 90, 169, 29, 161, 128, 68, 39, 102, 137, 40, 178, 133, 2, 62, 250, 110, 3, 235, 201, 50, 98, 219, 246, 161, 47, 201, 22, 215, 9, 47, 107, 175, 183, 29, 252, 126, 28, 18, 192, 77, 7, 172, 182, 231, 112, 247, 71, 142, 39, 132, 0, 58, 220, 138, 227, 70, 61, 55, 169, 48, 224, 215, 32, 231, 90, 100, 33, 96, 229, 134, 220, 129, 131, 248, 59, 233, 198, 89, 171, 231, 249, 186, 35, 25, 65, 233, 12, 5, 151, 69, 206, 250, 232, 108, 189, 146, 73, 173 }, 1, "Kousik Raj" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "DOB", "Email", "PasswordHash", "PasswordHashKey", "Role", "Username" },
                values: new object[] { 102, new DateTime(2003, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "mathew@gmail.com", new byte[] { 239, 176, 171, 188, 215, 46, 40, 60, 226, 122, 114, 116, 232, 48, 145, 182, 83, 167, 179, 165, 102, 41, 11, 91, 145, 62, 253, 62, 214, 210, 255, 56, 132, 70, 220, 66, 54, 11, 53, 240, 123, 125, 216, 5, 51, 175, 36, 231, 254, 64, 154, 43, 147, 72, 197, 99, 173, 244, 251, 227, 21, 25, 180, 133 }, new byte[] { 57, 134, 191, 82, 188, 97, 218, 62, 178, 159, 107, 128, 131, 64, 208, 163, 107, 183, 240, 250, 27, 201, 16, 102, 172, 199, 222, 150, 254, 80, 90, 169, 29, 161, 128, 68, 39, 102, 137, 40, 178, 133, 2, 62, 250, 110, 3, 235, 201, 50, 98, 219, 246, 161, 47, 201, 22, 215, 9, 47, 107, 175, 183, 29, 252, 126, 28, 18, 192, 77, 7, 172, 182, 231, 112, 247, 71, 142, 39, 132, 0, 58, 220, 138, 227, 70, 61, 55, 169, 48, 224, 215, 32, 231, 90, 100, 33, 96, 229, 134, 220, 129, 131, 248, 59, 233, 198, 89, 171, 231, 249, 186, 35, 25, 65, 233, 12, 5, 151, 69, 206, 250, 232, 108, 189, 146, 73, 173 }, 2, "Mathew" });

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
                values: new object[] { 1, 1, 1, 120, 0, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Song One", "http://example.com/song1.mp3" });

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

            migrationBuilder.CreateIndex(
                name: "IX_Albums_ArtistId",
                table: "Albums",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_PlaylistId",
                table: "Favorites",
                column: "PlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_SongId",
                table: "Favorites",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_UserId",
                table: "Favorites",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_UserId",
                table: "Playlists",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistSongs_PlaylistId",
                table: "PlaylistSongs",
                column: "PlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistSongs_SongId",
                table: "PlaylistSongs",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_SongId",
                table: "Ratings",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserId",
                table: "Ratings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Songs_AlbumId",
                table: "Songs",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_Songs_ArtistId",
                table: "Songs",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.DropTable(
                name: "PlaylistSongs");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "Playlists");

            migrationBuilder.DropTable(
                name: "Songs");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Albums");

            migrationBuilder.DropTable(
                name: "Artists");
        }
    }
}
