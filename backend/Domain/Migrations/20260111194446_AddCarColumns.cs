using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddCarColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1d042f4a-ab79-49d1-986a-fa11f65692ef");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6bcaab06-f666-4de9-bbee-15849ca942d7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7ab5b3b1-6b6d-4670-bd4a-317ff844f41c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d9d94f0b-5894-45cd-9080-bfedc4dc14ab");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "18472bd6-d99a-4fc6-8471-f778220bf508", null, "Admin", "ADMIN" },
                    { "3ccba0a1-3322-47f4-9baf-279aaa27f1d3", null, "User", "USER" },
                    { "9edebab6-c117-404d-b7ac-6cace49003c8", null, "Support", "SUPPORT" },
                    { "f55b6c9d-a26c-483b-b4fb-4e6cd8b9aa5d", null, "Owner", "OWNER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "18472bd6-d99a-4fc6-8471-f778220bf508");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3ccba0a1-3322-47f4-9baf-279aaa27f1d3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9edebab6-c117-404d-b7ac-6cace49003c8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f55b6c9d-a26c-483b-b4fb-4e6cd8b9aa5d");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1d042f4a-ab79-49d1-986a-fa11f65692ef", null, "User", "USER" },
                    { "6bcaab06-f666-4de9-bbee-15849ca942d7", null, "Support", "SUPPORT" },
                    { "7ab5b3b1-6b6d-4670-bd4a-317ff844f41c", null, "Owner", "OWNER" },
                    { "d9d94f0b-5894-45cd-9080-bfedc4dc14ab", null, "Admin", "ADMIN" }
                });
        }
    }
}
