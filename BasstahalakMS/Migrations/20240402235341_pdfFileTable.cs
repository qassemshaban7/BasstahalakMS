using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasstahalakMS.Migrations
{
    /// <inheritdoc />
    public partial class pdfFileTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PdfFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PDFPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PdfFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PdfFiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pdfNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReciveUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PdfId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pdfNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_pdfNotes_AspNetUsers_ReciveUserId",
                        column: x => x.ReciveUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_pdfNotes_PdfFiles_PdfId",
                        column: x => x.PdfId,
                        principalTable: "PdfFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PdfFiles_UserId",
                table: "PdfFiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_pdfNotes_PdfId",
                table: "pdfNotes",
                column: "PdfId");

            migrationBuilder.CreateIndex(
                name: "IX_pdfNotes_ReciveUserId",
                table: "pdfNotes",
                column: "ReciveUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pdfNotes");

            migrationBuilder.DropTable(
                name: "PdfFiles");
        }
    }
}