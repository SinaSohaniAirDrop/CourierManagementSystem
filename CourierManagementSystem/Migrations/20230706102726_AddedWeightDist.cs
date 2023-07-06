using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CourierManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddedWeightDist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9205db34-e7f2-41b7-bf4c-c82f1d0edcfc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d571587b-cef2-436b-85e4-8f932e54dcac");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f94ceca1-60e4-4b56-94fa-81b75d35e701");

            migrationBuilder.CreateTable(
                name: "WeightDist",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MinWeight = table.Column<double>(type: "float", nullable: false),
                    MaxWeight = table.Column<double>(type: "float", nullable: false),
                    NeighboringProvince = table.Column<double>(type: "float", nullable: false),
                    OtherProvince = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeightDist", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "45deb828-4a28-4e37-9e37-7e2ffc83fe70", "1", "Admin", "Admin" },
                    { "af8df4f4-d071-41df-a140-175fded646ee", "2", "Developer", "Developer" },
                    { "e3288261-5c9a-4b7e-86e8-3c8992811dfb", "3", "User", "User" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeightDist");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "45deb828-4a28-4e37-9e37-7e2ffc83fe70");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "af8df4f4-d071-41df-a140-175fded646ee");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e3288261-5c9a-4b7e-86e8-3c8992811dfb");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9205db34-e7f2-41b7-bf4c-c82f1d0edcfc", "3", "User", "User" },
                    { "d571587b-cef2-436b-85e4-8f932e54dcac", "1", "Admin", "Admin" },
                    { "f94ceca1-60e4-4b56-94fa-81b75d35e701", "2", "Developer", "Developer" }
                });
        }
    }
}
