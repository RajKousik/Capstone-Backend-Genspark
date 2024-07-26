using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicApplicationAPI.Migrations
{
    public partial class EmailVerificationDbSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailVerification_Users_UserId",
                table: "EmailVerification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmailVerification",
                table: "EmailVerification");

            migrationBuilder.RenameTable(
                name: "EmailVerification",
                newName: "EmailVerifications");

            migrationBuilder.RenameIndex(
                name: "IX_EmailVerification_UserId",
                table: "EmailVerifications",
                newName: "IX_EmailVerifications_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmailVerifications",
                table: "EmailVerifications",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_EmailVerifications_Users_UserId",
                table: "EmailVerifications",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailVerifications_Users_UserId",
                table: "EmailVerifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmailVerifications",
                table: "EmailVerifications");

            migrationBuilder.RenameTable(
                name: "EmailVerifications",
                newName: "EmailVerification");

            migrationBuilder.RenameIndex(
                name: "IX_EmailVerifications_UserId",
                table: "EmailVerification",
                newName: "IX_EmailVerification_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmailVerification",
                table: "EmailVerification",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_EmailVerification_Users_UserId",
                table: "EmailVerification",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
