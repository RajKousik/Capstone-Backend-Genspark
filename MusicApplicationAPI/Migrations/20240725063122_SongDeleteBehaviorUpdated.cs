using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicApplicationAPI.Migrations
{
    public partial class SongDeleteBehaviorUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Songs_SongId",
                table: "Favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Songs_SongId",
                table: "Ratings");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 101,
                columns: new[] { "PasswordHash", "PasswordHashKey" },
                values: new object[] { new byte[] { 81, 161, 132, 150, 15, 38, 9, 89, 218, 229, 115, 72, 133, 115, 9, 197, 82, 163, 91, 43, 38, 231, 225, 34, 237, 170, 102, 148, 54, 242, 162, 189, 81, 193, 155, 16, 75, 140, 62, 247, 171, 252, 140, 211, 205, 118, 244, 229, 114, 102, 69, 136, 74, 9, 212, 71, 225, 219, 27, 40, 77, 121, 189, 85 }, new byte[] { 89, 86, 200, 105, 147, 185, 15, 225, 66, 191, 170, 21, 126, 210, 166, 226, 200, 172, 38, 230, 196, 184, 203, 204, 138, 226, 156, 214, 204, 9, 252, 30, 136, 97, 23, 85, 12, 159, 158, 116, 169, 248, 22, 207, 221, 73, 66, 254, 234, 60, 21, 95, 157, 250, 86, 26, 0, 167, 122, 198, 178, 180, 161, 63, 167, 186, 197, 30, 234, 120, 93, 132, 3, 78, 184, 120, 217, 246, 16, 86, 65, 130, 75, 120, 243, 148, 218, 235, 18, 34, 20, 39, 2, 79, 170, 138, 208, 72, 93, 237, 59, 17, 178, 159, 78, 234, 32, 113, 79, 69, 105, 111, 144, 198, 123, 135, 180, 76, 152, 156, 137, 62, 167, 187, 194, 53, 168, 241 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 102,
                columns: new[] { "PasswordHash", "PasswordHashKey" },
                values: new object[] { new byte[] { 78, 65, 119, 12, 24, 29, 224, 1, 38, 231, 202, 247, 87, 96, 199, 170, 222, 141, 223, 108, 233, 152, 243, 189, 230, 129, 155, 52, 195, 104, 187, 134, 58, 136, 16, 170, 43, 248, 130, 184, 68, 79, 166, 41, 19, 143, 226, 106, 239, 174, 244, 131, 128, 25, 103, 16, 186, 104, 229, 38, 58, 241, 9, 36 }, new byte[] { 89, 86, 200, 105, 147, 185, 15, 225, 66, 191, 170, 21, 126, 210, 166, 226, 200, 172, 38, 230, 196, 184, 203, 204, 138, 226, 156, 214, 204, 9, 252, 30, 136, 97, 23, 85, 12, 159, 158, 116, 169, 248, 22, 207, 221, 73, 66, 254, 234, 60, 21, 95, 157, 250, 86, 26, 0, 167, 122, 198, 178, 180, 161, 63, 167, 186, 197, 30, 234, 120, 93, 132, 3, 78, 184, 120, 217, 246, 16, 86, 65, 130, 75, 120, 243, 148, 218, 235, 18, 34, 20, 39, 2, 79, 170, 138, 208, 72, 93, 237, 59, 17, 178, 159, 78, 234, 32, 113, 79, 69, 105, 111, 144, 198, 123, 135, 180, 76, 152, 156, 137, 62, 167, 187, 194, 53, 168, 241 } });

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Songs_SongId",
                table: "Favorites",
                column: "SongId",
                principalTable: "Songs",
                principalColumn: "SongId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Songs_SongId",
                table: "Ratings",
                column: "SongId",
                principalTable: "Songs",
                principalColumn: "SongId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Songs_SongId",
                table: "Favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Songs_SongId",
                table: "Ratings");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 101,
                columns: new[] { "PasswordHash", "PasswordHashKey" },
                values: new object[] { new byte[] { 231, 247, 32, 158, 20, 101, 223, 247, 247, 8, 80, 144, 114, 123, 56, 196, 64, 84, 52, 194, 138, 248, 64, 227, 31, 0, 211, 17, 232, 26, 109, 110, 234, 198, 105, 10, 46, 207, 221, 64, 254, 72, 187, 88, 210, 238, 29, 170, 225, 105, 105, 34, 199, 62, 6, 116, 79, 172, 112, 165, 1, 230, 107, 218 }, new byte[] { 39, 181, 158, 253, 61, 55, 50, 118, 16, 68, 133, 16, 254, 118, 16, 192, 223, 48, 140, 115, 98, 231, 57, 60, 91, 172, 18, 242, 186, 193, 29, 180, 41, 161, 84, 181, 38, 95, 213, 116, 146, 12, 24, 209, 125, 81, 227, 57, 254, 161, 189, 156, 48, 106, 40, 232, 88, 62, 214, 193, 235, 229, 221, 107, 84, 35, 13, 154, 57, 138, 120, 210, 133, 41, 126, 44, 237, 85, 189, 178, 162, 95, 203, 6, 144, 132, 4, 43, 34, 166, 62, 158, 112, 55, 25, 75, 26, 10, 255, 208, 65, 83, 111, 59, 5, 15, 221, 152, 101, 77, 123, 219, 14, 187, 216, 105, 127, 131, 220, 113, 164, 40, 150, 118, 74, 195, 81, 188 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 102,
                columns: new[] { "PasswordHash", "PasswordHashKey" },
                values: new object[] { new byte[] { 23, 207, 141, 49, 95, 120, 223, 163, 219, 35, 65, 79, 253, 101, 250, 81, 129, 162, 155, 60, 41, 218, 12, 242, 152, 115, 145, 118, 248, 236, 27, 217, 6, 72, 48, 182, 194, 3, 9, 217, 96, 88, 141, 163, 7, 131, 42, 30, 30, 129, 60, 205, 147, 47, 131, 43, 172, 184, 94, 72, 237, 204, 129, 216 }, new byte[] { 39, 181, 158, 253, 61, 55, 50, 118, 16, 68, 133, 16, 254, 118, 16, 192, 223, 48, 140, 115, 98, 231, 57, 60, 91, 172, 18, 242, 186, 193, 29, 180, 41, 161, 84, 181, 38, 95, 213, 116, 146, 12, 24, 209, 125, 81, 227, 57, 254, 161, 189, 156, 48, 106, 40, 232, 88, 62, 214, 193, 235, 229, 221, 107, 84, 35, 13, 154, 57, 138, 120, 210, 133, 41, 126, 44, 237, 85, 189, 178, 162, 95, 203, 6, 144, 132, 4, 43, 34, 166, 62, 158, 112, 55, 25, 75, 26, 10, 255, 208, 65, 83, 111, 59, 5, 15, 221, 152, 101, 77, 123, 219, 14, 187, 216, 105, 127, 131, 220, 113, 164, 40, 150, 118, 74, 195, 81, 188 } });

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Songs_SongId",
                table: "Favorites",
                column: "SongId",
                principalTable: "Songs",
                principalColumn: "SongId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Songs_SongId",
                table: "Ratings",
                column: "SongId",
                principalTable: "Songs",
                principalColumn: "SongId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
