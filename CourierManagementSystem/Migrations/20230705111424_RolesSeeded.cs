using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CourierManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class RolesSeeded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7606ae8f-fbf6-4d19-a784-ed659c4f337b", "2", "Developer", "Developer" },
                    { "88dde547-6ead-43c4-987b-1fe30c71121a", "3", "User", "User" },
                    { "aba18e43-abac-4ad0-a913-3d85a8e03218", "1", "Admin", "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7606ae8f-fbf6-4d19-a784-ed659c4f337b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "88dde547-6ead-43c4-987b-1fe30c71121a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "aba18e43-abac-4ad0-a913-3d85a8e03218");
        }
    }
}
