﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BasstahalakMS.Migrations
{
    /// <inheritdoc />
    public partial class AddfilecontentcolumntoBfileTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.AddColumn<string>(
                name: "fileContent",
                table: "BFiles",
                type: "nvarchar(max)",
                nullable: true);

            
            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "fileContent",
                table: "BFiles");

          
        }
    }
}
