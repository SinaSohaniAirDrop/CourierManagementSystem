using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CourierManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddedOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8e34538a-80e8-4fc7-9a89-d67fcd6cb42d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d5b4580a-52f9-4063-9b39-7873fb3a99be");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ee619590-e975-42f6-aff2-7a4322dddbba");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Package");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Request",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<double>(
                name: "Value",
                table: "Package",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PackageId = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<double>(type: "float", nullable: false),
                    Discount = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_Package_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Package",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "af92bd6d-7c15-4772-a56c-74975e46b3cc", "2", "Developer", "Developer" },
                    { "e48a6b23-be2b-4f72-a265-935ab0c752b9", "1", "Admin", "Admin" },
                    { "f7720783-8e60-490b-8d5e-e562b7dd4da4", "3", "User", "User" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_PackageId",
                table: "Order",
                column: "PackageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "af92bd6d-7c15-4772-a56c-74975e46b3cc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e48a6b23-be2b-4f72-a265-935ab0c752b9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f7720783-8e60-490b-8d5e-e562b7dd4da4");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Package");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Request",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Package",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8e34538a-80e8-4fc7-9a89-d67fcd6cb42d", "3", "User", "User" },
                    { "d5b4580a-52f9-4063-9b39-7873fb3a99be", "2", "Developer", "Developer" },
                    { "ee619590-e975-42f6-aff2-7a4322dddbba", "1", "Admin", "Admin" }
                });
        }
    }
}
