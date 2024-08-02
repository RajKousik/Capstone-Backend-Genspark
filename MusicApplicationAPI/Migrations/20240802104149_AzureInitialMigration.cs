using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicApplicationAPI.Migrations
{
    public partial class AzureInitialMigration : Migration
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
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordHashKey = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordHashKey = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "EmailVerifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    VerificationCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailVerifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailVerifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
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
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                name: "PremiumUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Money = table.Column<double>(type: "float", nullable: false),
                    LastNotifiedTwoDaysBefore = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastNotifiedOneHourBefore = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PremiumUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PremiumUsers_Users_UserId",
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
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                columns: new[] { "ArtistId", "Bio", "Email", "ImageUrl", "Name", "PasswordHash", "PasswordHashKey", "Role", "Status" },
                values: new object[] { 1, "Bio of Artist One", "artist1@gmail.com", "http://example.com/artist1.jpg", "Artist One", new byte[] { 245, 17, 219, 213, 55, 33, 158, 50, 138, 218, 16, 45, 178, 59, 17, 199, 85, 237, 44, 50, 37, 97, 118, 176, 217, 237, 106, 162, 86, 72, 178, 48, 176, 40, 205, 227, 247, 52, 35, 15, 114, 156, 243, 145, 151, 144, 233, 145, 231, 101, 254, 17, 221, 205, 61, 194, 212, 72, 248, 95, 150, 87, 76, 45 }, new byte[] { 248, 61, 181, 152, 250, 160, 207, 242, 60, 129, 9, 113, 226, 195, 212, 204, 242, 124, 15, 144, 233, 137, 116, 41, 217, 101, 5, 104, 164, 156, 199, 56, 137, 20, 7, 188, 255, 87, 211, 130, 182, 123, 39, 188, 70, 55, 122, 44, 156, 153, 71, 215, 208, 204, 78, 92, 200, 66, 55, 160, 108, 59, 70, 18, 102, 124, 73, 164, 46, 28, 87, 69, 210, 191, 115, 38, 134, 22, 201, 30, 128, 219, 100, 130, 1, 245, 196, 246, 120, 36, 123, 201, 22, 52, 238, 123, 100, 250, 200, 206, 36, 220, 83, 193, 97, 51, 76, 171, 130, 123, 178, 137, 128, 210, 230, 248, 237, 152, 229, 60, 37, 231, 64, 80, 138, 181, 66, 84 }, 4, "Active" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "DOB", "Email", "PasswordHash", "PasswordHashKey", "Phone", "Role", "Status", "Username" },
                values: new object[] { 101, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "kousik@gmail.com", new byte[] { 113, 173, 78, 250, 19, 72, 240, 152, 38, 186, 145, 224, 77, 83, 216, 130, 114, 189, 144, 209, 228, 202, 89, 2, 245, 222, 168, 166, 202, 57, 115, 177, 5, 216, 231, 8, 42, 159, 80, 128, 66, 96, 160, 154, 149, 158, 112, 209, 138, 74, 208, 134, 23, 139, 195, 77, 59, 182, 198, 58, 128, 58, 197, 192 }, new byte[] { 248, 61, 181, 152, 250, 160, 207, 242, 60, 129, 9, 113, 226, 195, 212, 204, 242, 124, 15, 144, 233, 137, 116, 41, 217, 101, 5, 104, 164, 156, 199, 56, 137, 20, 7, 188, 255, 87, 211, 130, 182, 123, 39, 188, 70, 55, 122, 44, 156, 153, 71, 215, 208, 204, 78, 92, 200, 66, 55, 160, 108, 59, 70, 18, 102, 124, 73, 164, 46, 28, 87, 69, 210, 191, 115, 38, 134, 22, 201, 30, 128, 219, 100, 130, 1, 245, 196, 246, 120, 36, 123, 201, 22, 52, 238, 123, 100, 250, 200, 206, 36, 220, 83, 193, 97, 51, 76, 171, 130, 123, 178, 137, 128, 210, 230, 248, 237, 152, 229, 60, 37, 231, 64, 80, 138, 181, 66, 84 }, "9790852900", 1, null, "Kousik Raj" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "DOB", "Email", "PasswordHash", "PasswordHashKey", "Phone", "Role", "Status", "Username" },
                values: new object[] { 102, new DateTime(2003, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "mathew@gmail.com", new byte[] { 249, 172, 48, 217, 224, 72, 242, 138, 29, 70, 26, 250, 225, 253, 52, 76, 203, 106, 121, 118, 196, 208, 51, 115, 75, 188, 35, 180, 54, 236, 148, 164, 100, 118, 127, 105, 115, 160, 94, 103, 51, 202, 232, 202, 113, 62, 188, 207, 139, 183, 130, 122, 147, 108, 3, 73, 85, 51, 3, 54, 182, 226, 98, 78 }, new byte[] { 248, 61, 181, 152, 250, 160, 207, 242, 60, 129, 9, 113, 226, 195, 212, 204, 242, 124, 15, 144, 233, 137, 116, 41, 217, 101, 5, 104, 164, 156, 199, 56, 137, 20, 7, 188, 255, 87, 211, 130, 182, 123, 39, 188, 70, 55, 122, 44, 156, 153, 71, 215, 208, 204, 78, 92, 200, 66, 55, 160, 108, 59, 70, 18, 102, 124, 73, 164, 46, 28, 87, 69, 210, 191, 115, 38, 134, 22, 201, 30, 128, 219, 100, 130, 1, 245, 196, 246, 120, 36, 123, 201, 22, 52, 238, 123, 100, 250, 200, 206, 36, 220, 83, 193, 97, 51, 76, 171, 130, 123, 178, 137, 128, 210, 230, 248, 237, 152, 229, 60, 37, 231, 64, 80, 138, 181, 66, 84 }, "9012382181", 2, null, "Mathew" });

            migrationBuilder.InsertData(
                table: "Albums",
                columns: new[] { "AlbumId", "ArtistId", "CoverImageUrl", "ReleaseDate", "Title" },
                values: new object[] { 1, 1, "http://example.com/album1.jpg", new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Album One" });

            migrationBuilder.InsertData(
                table: "Playlists",
                columns: new[] { "PlaylistId", "ImageUrl", "IsPublic", "Name", "UserId" },
                values: new object[] { 1, "https://some-url", true, "Playlist One", 102 });

            migrationBuilder.InsertData(
                table: "Songs",
                columns: new[] { "SongId", "AlbumId", "ArtistId", "Duration", "Genre", "ImageUrl", "ReleaseDate", "Title", "Url" },
                values: new object[] { 1, 1, 1, 120, 0, "https://some-url", new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Song One", "http://example.com/song1.mp3" });

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
                name: "IX_EmailVerifications_UserId",
                table: "EmailVerifications",
                column: "UserId",
                unique: true);

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
                name: "IX_PremiumUsers_UserId",
                table: "PremiumUsers",
                column: "UserId",
                unique: true);

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
                name: "EmailVerifications");

            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.DropTable(
                name: "PlaylistSongs");

            migrationBuilder.DropTable(
                name: "PremiumUsers");

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
