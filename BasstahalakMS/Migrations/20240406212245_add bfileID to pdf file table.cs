using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasstahalakMS.Migrations
{
    /// <inheritdoc />
    public partial class addbfileIDtopdffiletable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BfileId",
                table: "PdfFiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PdfFiles_BfileId",
                table: "PdfFiles",
                column: "BfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_PdfFiles_BFiles_BfileId",
                table: "PdfFiles",
                column: "BfileId",
                principalTable: "BFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PdfFiles_BFiles_BfileId",
                table: "PdfFiles");

            migrationBuilder.DropIndex(
                name: "IX_PdfFiles_BfileId",
                table: "PdfFiles");

            migrationBuilder.DropColumn(
                name: "BfileId",
                table: "PdfFiles");
        }
    }
}
