using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicApplicationAPI.Migrations
{
    public partial class PremiumUserCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PremiumUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Money = table.Column<double>(type: "float", nullable: false)
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

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 101,
                columns: new[] { "PasswordHash", "PasswordHashKey" },
                values: new object[] { new byte[] { 200, 236, 192, 175, 27, 145, 246, 42, 88, 90, 100, 177, 49, 82, 232, 138, 130, 82, 69, 88, 48, 196, 134, 178, 242, 89, 45, 252, 157, 134, 137, 207, 75, 89, 193, 111, 16, 113, 184, 131, 115, 178, 221, 11, 245, 220, 132, 15, 86, 243, 49, 197, 65, 105, 158, 64, 167, 4, 180, 92, 16, 100, 105, 0 }, new byte[] { 134, 235, 108, 213, 225, 17, 29, 124, 202, 16, 168, 75, 113, 4, 79, 106, 219, 108, 88, 119, 86, 110, 99, 177, 178, 81, 28, 18, 241, 236, 9, 7, 4, 9, 200, 145, 60, 5, 196, 179, 136, 120, 165, 228, 204, 6, 140, 92, 67, 108, 30, 125, 214, 208, 152, 238, 27, 176, 75, 175, 57, 102, 29, 10, 13, 89, 255, 133, 97, 222, 27, 247, 212, 23, 121, 29, 25, 18, 128, 35, 159, 40, 92, 156, 35, 37, 232, 253, 130, 36, 211, 141, 136, 18, 149, 50, 240, 10, 176, 135, 233, 218, 204, 251, 178, 55, 20, 56, 225, 122, 40, 26, 114, 239, 202, 232, 234, 147, 220, 34, 195, 179, 128, 241, 189, 22, 29, 102 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 102,
                columns: new[] { "PasswordHash", "PasswordHashKey" },
                values: new object[] { new byte[] { 206, 52, 0, 107, 186, 126, 34, 100, 77, 31, 82, 189, 255, 143, 52, 216, 112, 177, 104, 52, 98, 130, 237, 141, 198, 90, 162, 64, 122, 8, 195, 162, 64, 57, 60, 35, 129, 48, 170, 200, 81, 120, 70, 206, 168, 54, 105, 81, 5, 251, 146, 178, 230, 220, 15, 162, 246, 125, 31, 74, 42, 45, 46, 123 }, new byte[] { 134, 235, 108, 213, 225, 17, 29, 124, 202, 16, 168, 75, 113, 4, 79, 106, 219, 108, 88, 119, 86, 110, 99, 177, 178, 81, 28, 18, 241, 236, 9, 7, 4, 9, 200, 145, 60, 5, 196, 179, 136, 120, 165, 228, 204, 6, 140, 92, 67, 108, 30, 125, 214, 208, 152, 238, 27, 176, 75, 175, 57, 102, 29, 10, 13, 89, 255, 133, 97, 222, 27, 247, 212, 23, 121, 29, 25, 18, 128, 35, 159, 40, 92, 156, 35, 37, 232, 253, 130, 36, 211, 141, 136, 18, 149, 50, 240, 10, 176, 135, 233, 218, 204, 251, 178, 55, 20, 56, 225, 122, 40, 26, 114, 239, 202, 232, 234, 147, 220, 34, 195, 179, 128, 241, 189, 22, 29, 102 } });

            migrationBuilder.CreateIndex(
                name: "IX_PremiumUsers_UserId",
                table: "PremiumUsers",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PremiumUsers");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 101,
                columns: new[] { "PasswordHash", "PasswordHashKey" },
                values: new object[] { new byte[] { 121, 122, 252, 245, 18, 153, 229, 192, 221, 182, 19, 137, 149, 93, 109, 152, 47, 214, 187, 104, 119, 64, 10, 20, 53, 202, 195, 140, 214, 170, 68, 33, 149, 84, 247, 97, 154, 145, 48, 244, 176, 33, 220, 24, 16, 150, 244, 159, 152, 111, 124, 210, 13, 94, 120, 117, 177, 87, 17, 147, 77, 168, 168, 144 }, new byte[] { 203, 63, 146, 159, 206, 74, 139, 226, 85, 219, 2, 113, 231, 255, 100, 209, 215, 25, 163, 54, 199, 248, 180, 8, 140, 164, 254, 172, 204, 39, 134, 188, 37, 72, 16, 97, 124, 224, 38, 51, 238, 237, 242, 131, 219, 87, 16, 151, 173, 155, 138, 68, 154, 217, 56, 115, 236, 134, 251, 174, 49, 23, 204, 151, 189, 117, 148, 53, 165, 1, 51, 100, 72, 86, 20, 21, 172, 95, 61, 222, 18, 180, 255, 131, 41, 11, 55, 96, 154, 235, 4, 78, 10, 49, 44, 77, 72, 70, 200, 3, 175, 7, 70, 96, 50, 128, 189, 39, 231, 155, 96, 130, 177, 129, 78, 151, 245, 248, 84, 17, 5, 216, 127, 42, 121, 133, 156, 149 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 102,
                columns: new[] { "PasswordHash", "PasswordHashKey" },
                values: new object[] { new byte[] { 239, 208, 77, 201, 197, 147, 154, 243, 104, 183, 199, 122, 144, 166, 124, 254, 29, 47, 19, 74, 122, 247, 53, 26, 16, 128, 151, 181, 250, 0, 19, 114, 223, 198, 144, 62, 59, 15, 193, 189, 89, 3, 144, 216, 175, 204, 120, 200, 20, 11, 45, 42, 248, 67, 6, 120, 177, 62, 37, 114, 159, 66, 68, 81 }, new byte[] { 203, 63, 146, 159, 206, 74, 139, 226, 85, 219, 2, 113, 231, 255, 100, 209, 215, 25, 163, 54, 199, 248, 180, 8, 140, 164, 254, 172, 204, 39, 134, 188, 37, 72, 16, 97, 124, 224, 38, 51, 238, 237, 242, 131, 219, 87, 16, 151, 173, 155, 138, 68, 154, 217, 56, 115, 236, 134, 251, 174, 49, 23, 204, 151, 189, 117, 148, 53, 165, 1, 51, 100, 72, 86, 20, 21, 172, 95, 61, 222, 18, 180, 255, 131, 41, 11, 55, 96, 154, 235, 4, 78, 10, 49, 44, 77, 72, 70, 200, 3, 175, 7, 70, 96, 50, 128, 189, 39, 231, 155, 96, 130, 177, 129, 78, 151, 245, 248, 84, 17, 5, 216, 127, 42, 121, 133, 156, 149 } });
        }
    }
}
