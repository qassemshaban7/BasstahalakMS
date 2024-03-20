using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasstahalakMS.Migrations
{
    /// <inheritdoc />
    public partial class printingRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ArabicRoleName", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
                values: new object[] { "488fea2d-57d4-4963-81d2-7e2e02a2b100", "مطبعة", "488fea2d-57d4-4963-81d2-7e2e02a2b100", "ApplicationRole", "Printing", "PRINTING" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "488fea2d-57d4-4963-81d2-7e2e02a2b100");
        }
    }
}
