using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddCarDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "260e7264-7af5-4880-b9e4-98117d9ea565");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3fbf13c0-26a5-4d82-b22d-a8e0608c73a3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c092d332-16c1-4d19-9912-b4e72222115f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fed5082c-dcb5-4dec-bd4a-f6133774becf");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                    { "260e7264-7af5-4880-b9e4-98117d9ea565", null, "User", "USER" },
                    { "3fbf13c0-26a5-4d82-b22d-a8e0608c73a3", null, "Owner", "OWNER" },
                    { "c092d332-16c1-4d19-9912-b4e72222115f", null, "Support", "SUPPORT" },
                    { "fed5082c-dcb5-4dec-bd4a-f6133774becf", null, "Admin", "ADMIN" }
                });
        }
    }
}
