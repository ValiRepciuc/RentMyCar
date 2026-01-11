using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddCarFrontendFields : Migration
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

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Car",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Features",
                table: "Car",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Car",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrls",
                table: "Car",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Rating",
                table: "Car",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "ReviewCount",
                table: "Car",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Seats",
                table: "Car",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "29fb2b17-60c8-4721-aff3-8e8b924eedf8", null, "Admin", "ADMIN" },
                    { "88f276ac-abfc-4213-a03a-0c4f3a5fac50", null, "Owner", "OWNER" },
                    { "8eb010cd-3ad7-4517-b90b-2766e3757c1a", null, "Support", "SUPPORT" },
                    { "bc4b0006-408e-4cf8-adec-ab6b83861256", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "29fb2b17-60c8-4721-aff3-8e8b924eedf8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "88f276ac-abfc-4213-a03a-0c4f3a5fac50");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8eb010cd-3ad7-4517-b90b-2766e3757c1a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bc4b0006-408e-4cf8-adec-ab6b83861256");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "Features",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "ImageUrls",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "ReviewCount",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "Seats",
                table: "Car");

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
