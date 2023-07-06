using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CourierManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddedUnique_Package : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AlterColumn<string>(
                name: "Size",
                table: "Packaging",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Package",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PickupDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PickupCity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeliveryCity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PickupLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeliveryLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsNeighboringCity = table.Column<bool>(type: "bit", nullable: false),
                    SenderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ReceiverId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Package", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Package_AspNetUsers_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Package_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8e34538a-80e8-4fc7-9a89-d67fcd6cb42d", "3", "User", "User" },
                    { "d5b4580a-52f9-4063-9b39-7873fb3a99be", "2", "Developer", "Developer" },
                    { "ee619590-e975-42f6-aff2-7a4322dddbba", "1", "Admin", "Admin" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeightDist_MaxWeight",
                table: "WeightDist",
                column: "MaxWeight",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WeightDist_MinWeight",
                table: "WeightDist",
                column: "MinWeight",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Packaging_Size",
                table: "Packaging",
                column: "Size",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Insurance_MaxVal",
                table: "Insurance",
                column: "MaxVal",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Insurance_MinVal",
                table: "Insurance",
                column: "MinVal",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Package_ReceiverId",
                table: "Package",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Package_SenderId",
                table: "Package",
                column: "SenderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Package");

            migrationBuilder.DropIndex(
                name: "IX_WeightDist_MaxWeight",
                table: "WeightDist");

            migrationBuilder.DropIndex(
                name: "IX_WeightDist_MinWeight",
                table: "WeightDist");

            migrationBuilder.DropIndex(
                name: "IX_Packaging_Size",
                table: "Packaging");

            migrationBuilder.DropIndex(
                name: "IX_Insurance_MaxVal",
                table: "Insurance");

            migrationBuilder.DropIndex(
                name: "IX_Insurance_MinVal",
                table: "Insurance");

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

            migrationBuilder.AlterColumn<string>(
                name: "Size",
                table: "Packaging",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

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
    }
}
