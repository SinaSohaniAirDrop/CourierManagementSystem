using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CourierManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddedInsurance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "884826ef-5724-4a01-8323-963199239d8d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9cdad601-7a42-45e4-bba7-0715e2884159");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d69edb53-691a-4ad6-b9f0-1fb9bb17fef3");

            migrationBuilder.CreateTable(
                name: "Insurance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MinVal = table.Column<double>(type: "float", nullable: false),
                    MaxVal = table.Column<double>(type: "float", nullable: false),
                    Tariff = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Insurance", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Insurance");

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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "884826ef-5724-4a01-8323-963199239d8d", "3", "User", "User" },
                    { "9cdad601-7a42-45e4-bba7-0715e2884159", "2", "Developer", "Developer" },
                    { "d69edb53-691a-4ad6-b9f0-1fb9bb17fef3", "1", "Admin", "Admin" }
                });
        }
    }
}
