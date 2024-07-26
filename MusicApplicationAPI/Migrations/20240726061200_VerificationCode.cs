using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicApplicationAPI.Migrations
{
    public partial class VerificationCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EmailVerification",
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
                    table.PrimaryKey("PK_EmailVerification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailVerification_Users_UserId",
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
                values: new object[] { new byte[] { 29, 237, 144, 18, 243, 147, 20, 132, 76, 12, 172, 30, 89, 47, 72, 119, 150, 241, 252, 254, 201, 108, 88, 79, 97, 168, 45, 177, 224, 32, 176, 249, 8, 11, 36, 25, 253, 214, 46, 80, 249, 157, 226, 6, 146, 29, 114, 149, 27, 54, 197, 32, 52, 165, 145, 78, 199, 176, 106, 165, 207, 56, 27, 11 }, new byte[] { 40, 166, 2, 198, 72, 179, 81, 97, 219, 147, 2, 249, 236, 112, 166, 153, 7, 134, 158, 32, 9, 222, 238, 138, 49, 21, 138, 94, 103, 161, 239, 175, 41, 78, 154, 238, 51, 41, 23, 108, 253, 99, 74, 168, 53, 43, 71, 128, 159, 65, 103, 182, 143, 219, 239, 149, 161, 117, 158, 78, 224, 159, 119, 29, 163, 179, 15, 243, 150, 54, 131, 40, 224, 138, 165, 210, 180, 28, 72, 83, 145, 188, 85, 246, 38, 245, 196, 178, 173, 128, 115, 101, 239, 120, 255, 236, 41, 18, 50, 189, 203, 111, 232, 232, 49, 29, 103, 47, 66, 64, 240, 116, 214, 108, 24, 103, 211, 57, 40, 66, 169, 130, 204, 185, 10, 71, 56, 90 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 102,
                columns: new[] { "PasswordHash", "PasswordHashKey" },
                values: new object[] { new byte[] { 128, 227, 241, 185, 28, 6, 219, 208, 181, 125, 192, 119, 100, 13, 253, 175, 48, 79, 247, 147, 35, 151, 192, 0, 176, 189, 172, 220, 188, 218, 130, 56, 221, 38, 175, 143, 44, 59, 219, 102, 168, 119, 38, 111, 142, 189, 162, 33, 146, 227, 68, 185, 23, 22, 154, 0, 79, 161, 153, 99, 111, 251, 245, 116 }, new byte[] { 40, 166, 2, 198, 72, 179, 81, 97, 219, 147, 2, 249, 236, 112, 166, 153, 7, 134, 158, 32, 9, 222, 238, 138, 49, 21, 138, 94, 103, 161, 239, 175, 41, 78, 154, 238, 51, 41, 23, 108, 253, 99, 74, 168, 53, 43, 71, 128, 159, 65, 103, 182, 143, 219, 239, 149, 161, 117, 158, 78, 224, 159, 119, 29, 163, 179, 15, 243, 150, 54, 131, 40, 224, 138, 165, 210, 180, 28, 72, 83, 145, 188, 85, 246, 38, 245, 196, 178, 173, 128, 115, 101, 239, 120, 255, 236, 41, 18, 50, 189, 203, 111, 232, 232, 49, 29, 103, 47, 66, 64, 240, 116, 214, 108, 24, 103, 211, 57, 40, 66, 169, 130, 204, 185, 10, 71, 56, 90 } });

            migrationBuilder.CreateIndex(
                name: "IX_EmailVerification_UserId",
                table: "EmailVerification",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailVerification");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 101,
                columns: new[] { "PasswordHash", "PasswordHashKey" },
                values: new object[] { new byte[] { 206, 113, 135, 112, 226, 235, 252, 195, 6, 214, 192, 90, 243, 106, 247, 70, 116, 145, 29, 66, 79, 99, 53, 143, 151, 251, 127, 44, 217, 68, 61, 183, 176, 163, 198, 251, 171, 105, 252, 251, 190, 188, 215, 79, 153, 85, 220, 102, 239, 211, 92, 92, 240, 125, 132, 153, 75, 159, 89, 223, 103, 165, 161, 40 }, new byte[] { 141, 171, 239, 157, 159, 70, 29, 56, 137, 248, 45, 90, 135, 14, 174, 131, 192, 147, 81, 109, 187, 159, 134, 189, 227, 233, 102, 174, 200, 51, 50, 202, 161, 173, 41, 166, 96, 154, 116, 47, 86, 126, 217, 214, 185, 198, 41, 122, 241, 129, 139, 251, 123, 207, 255, 254, 121, 224, 22, 136, 126, 251, 55, 133, 168, 9, 107, 22, 25, 226, 113, 203, 216, 215, 168, 127, 165, 97, 103, 148, 172, 104, 55, 176, 145, 66, 247, 60, 138, 109, 134, 93, 249, 24, 22, 6, 90, 55, 113, 215, 114, 187, 162, 37, 101, 76, 243, 129, 58, 142, 105, 144, 25, 226, 18, 13, 241, 236, 197, 0, 81, 32, 232, 12, 105, 184, 134, 223 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 102,
                columns: new[] { "PasswordHash", "PasswordHashKey" },
                values: new object[] { new byte[] { 35, 228, 133, 251, 230, 61, 64, 136, 90, 235, 24, 241, 91, 243, 69, 24, 45, 249, 146, 143, 29, 5, 215, 199, 11, 142, 183, 40, 84, 240, 70, 91, 68, 209, 233, 150, 201, 93, 75, 67, 121, 71, 178, 76, 252, 102, 101, 107, 36, 35, 62, 30, 178, 149, 127, 46, 76, 176, 48, 239, 149, 24, 25, 156 }, new byte[] { 141, 171, 239, 157, 159, 70, 29, 56, 137, 248, 45, 90, 135, 14, 174, 131, 192, 147, 81, 109, 187, 159, 134, 189, 227, 233, 102, 174, 200, 51, 50, 202, 161, 173, 41, 166, 96, 154, 116, 47, 86, 126, 217, 214, 185, 198, 41, 122, 241, 129, 139, 251, 123, 207, 255, 254, 121, 224, 22, 136, 126, 251, 55, 133, 168, 9, 107, 22, 25, 226, 113, 203, 216, 215, 168, 127, 165, 97, 103, 148, 172, 104, 55, 176, 145, 66, 247, 60, 138, 109, 134, 93, 249, 24, 22, 6, 90, 55, 113, 215, 114, 187, 162, 37, 101, 76, 243, 129, 58, 142, 105, 144, 25, 226, 18, 13, 241, 236, 197, 0, 81, 32, 232, 12, 105, 184, 134, 223 } });
        }
    }
}
