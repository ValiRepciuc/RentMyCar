using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class CarAddMissingColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "01e3e30b-5fd5-46cf-90fb-d9ccbb91eb0b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "04e67cc8-c913-40c4-b9bd-7b1e4e75a334");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8d79ac7a-1997-4447-a5e2-a2b96989ad5e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b5777bae-44ca-4280-a23a-f59ec2fc91ac");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                    { "01e3e30b-5fd5-46cf-90fb-d9ccbb91eb0b", null, "User", "USER" },
                    { "04e67cc8-c913-40c4-b9bd-7b1e4e75a334", null, "Owner", "OWNER" },
                    { "8d79ac7a-1997-4447-a5e2-a2b96989ad5e", null, "Admin", "ADMIN" },
                    { "b5777bae-44ca-4280-a23a-f59ec2fc91ac", null, "Support", "SUPPORT" }
                });
        }
    }
}
