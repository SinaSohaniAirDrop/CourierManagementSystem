using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CourierManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddedComCost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "48956e9a-144b-4ed7-963f-ce462f2319c9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8a003550-c102-4fc3-873d-2a1abf46b192");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a8924321-3c2e-4d87-865b-12a5d9a12991");

            migrationBuilder.CreateTable(
                name: "ComCost",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FixedCost = table.Column<double>(type: "float", nullable: false),
                    tax = table.Column<double>(type: "float", nullable: false),
                    HQCost = table.Column<double>(type: "float", nullable: false),
                    InsiderFee = table.Column<double>(type: "float", nullable: false),
                    OutsiderFee = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComCost", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComCost");

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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "48956e9a-144b-4ed7-963f-ce462f2319c9", "2", "Developer", "Developer" },
                    { "8a003550-c102-4fc3-873d-2a1abf46b192", "3", "User", "User" },
                    { "a8924321-3c2e-4d87-865b-12a5d9a12991", "1", "Admin", "Admin" }
                });
        }
    }
}
