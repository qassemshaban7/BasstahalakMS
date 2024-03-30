using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasstahalakMS.Migrations
{
    /// <inheritdoc />
    public partial class reciveUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BfileNotes_AspNetUsers_UserId",
                table: "BfileNotes");

            migrationBuilder.DropIndex(
                name: "IX_BfileNotes_UserId",
                table: "BfileNotes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BfileNotes");

            migrationBuilder.AlterColumn<string>(
                name: "ReciveUserId",
                table: "BfileNotes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BfileNotes_ReciveUserId",
                table: "BfileNotes",
                column: "ReciveUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BfileNotes_AspNetUsers_ReciveUserId",
                table: "BfileNotes",
                column: "ReciveUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BfileNotes_AspNetUsers_ReciveUserId",
                table: "BfileNotes");

            migrationBuilder.DropIndex(
                name: "IX_BfileNotes_ReciveUserId",
                table: "BfileNotes");

            migrationBuilder.AlterColumn<string>(
                name: "ReciveUserId",
                table: "BfileNotes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "BfileNotes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_BfileNotes_UserId",
                table: "BfileNotes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BfileNotes_AspNetUsers_UserId",
                table: "BfileNotes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
