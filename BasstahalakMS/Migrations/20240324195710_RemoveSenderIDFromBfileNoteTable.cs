using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasstahalakMS.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSenderIDFromBfileNoteTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BfileNotes_AspNetUsers_SendUserId",
                table: "BfileNotes");

            migrationBuilder.DropIndex(
                name: "IX_BfileNotes_SendUserId",
                table: "BfileNotes");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "BfileNotes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SendUserId",
                table: "BfileNotes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BfileNotes_AspNetUsers_UserId",
                table: "BfileNotes");

            migrationBuilder.DropIndex(
                name: "IX_BfileNotes_UserId",
                table: "BfileNotes");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "BfileNotes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "SendUserId",
                table: "BfileNotes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BfileNotes_SendUserId",
                table: "BfileNotes",
                column: "SendUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BfileNotes_AspNetUsers_SendUserId",
                table: "BfileNotes",
                column: "SendUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
