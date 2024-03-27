using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasstahalakMS.Migrations
{
    /// <inheritdoc />
    public partial class TeamStatusColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeamStatus",
                table: "BFiles",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamStatus",
                table: "BFiles");
        }
    }
}
