using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BasstahalakMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditRolesAndUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2cdda855-1f15-4e11-9440-cfa84493cbd6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ba51b8f7-2a1d-45c6-9c00-68099eebd485");

            migrationBuilder.AddColumn<string>(
                name: "ArabicRoleName",
                table: "AspNetRoles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetRoles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ArabicRoleName", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2cdda855-1f15-4e11-9440-cfa84493cbd6", "المدير", "2cdda855-1f15-4e11-9440-cfa84493cbd6", "ApplicationRole", "Admin", "ADMIN" },
                    { "ba51b8f7-2a1d-45c6-9c00-68099eebd485", "المدير العام", "ba51b8f7-2a1d-45c6-9c00-68099eebd485", "ApplicationRole", "SuperAdmin", "SUPERADMIN" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "898d9efa-cd60-4446-b9ae-e0c48dd87c49",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "38261248-cd71-4137-8167-84f9ca5f10fb", "AQAAAAIAAYagAAAAELfAksHZfkyK7ZgbHUdAhPoRNGSgPwYxLKCna8nNBjggVt8I1bvBIgy3ttpSEYZC5w==", "38363752-569d-43cc-8906-bb1ef12d4f8d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ecc07b18-f55e-4f6b-95bd-0e84f556135f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "eed207cd-d8e3-4d44-bd8b-bb701cd4e944", "AQAAAAIAAYagAAAAEAxVXBgfi+IPftlT/R0hqR+JCuUyWgsZGXJgLYcfiStaTYVEFyYfRBIUZXDaSisPfA==", "baae3b7a-9cb7-441b-9343-3ebc4bc1ea50" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArabicRoleName",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetRoles");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "898d9efa-cd60-4446-b9ae-e0c48dd87c49",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b4f19f8b-563f-4468-be5a-5e9c3752fd28", "AQAAAAIAAYagAAAAEJt8F1vh3AkuPafhSFKGjuAQ45kFnN6K/nrCavAmO7XMF84jL/sCVuF9C7hwxAOXmw==", "1c9412e4-7346-4110-83ab-970047e441cf" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ecc07b18-f55e-4f6b-95bd-0e84f556135f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "54ac28ca-20dd-401e-a1f6-710e4bc094a9", "AQAAAAIAAYagAAAAENwG/8qbeASo27rXQMk8QU/IkmUzwrO3nW9DcVxlSWysZNba9wX7Ci6cnYxqgO8xXg==", "9c844275-0257-44d0-ac36-6b4e4b49a73c" });
        }
    }
}
