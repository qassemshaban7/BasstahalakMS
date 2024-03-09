using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BasstahalakMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class seedRolesAndUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2cdda855-1f15-4e11-9440-cfa84493cbd6", "2cdda855-1f15-4e11-9440-cfa84493cbd6", "Admin", "ADMIN" },
                    { "ba51b8f7-2a1d-45c6-9c00-68099eebd485", "ba51b8f7-2a1d-45c6-9c00-68099eebd485", "SuperAdmin", "SUPERADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "898d9efa-cd60-4446-b9ae-e0c48dd87c49", 0, "b4f19f8b-563f-4468-be5a-5e9c3752fd28", "ApplicationUser", "ehab@gmail.com", true, "ايهاب ابراهيم ", false, null, "EHAB@GMAIL.COM", "EHAB", "AQAAAAIAAYagAAAAEJt8F1vh3AkuPafhSFKGjuAQ45kFnN6K/nrCavAmO7XMF84jL/sCVuF9C7hwxAOXmw==", "1234567890", false, "1c9412e4-7346-4110-83ab-970047e441cf", false, "ehab" },
                    { "ecc07b18-f55e-4f6b-95bd-0e84f556135f", 0, "54ac28ca-20dd-401e-a1f6-710e4bc094a9", "ApplicationUser", "mohamedsalah@gmail.com", true, "محمد صلاح", false, null, "MOHAMEDSALAH@GMAIL.COM", "MOHAMEDSALAH", "AQAAAAIAAYagAAAAENwG/8qbeASo27rXQMk8QU/IkmUzwrO3nW9DcVxlSWysZNba9wX7Ci6cnYxqgO8xXg==", "1234567890", false, "9c844275-0257-44d0-ac36-6b4e4b49a73c", false, "mohamedsalah" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "2cdda855-1f15-4e11-9440-cfa84493cbd6", "898d9efa-cd60-4446-b9ae-e0c48dd87c49" },
                    { "ba51b8f7-2a1d-45c6-9c00-68099eebd485", "ecc07b18-f55e-4f6b-95bd-0e84f556135f" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2cdda855-1f15-4e11-9440-cfa84493cbd6", "898d9efa-cd60-4446-b9ae-e0c48dd87c49" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "ba51b8f7-2a1d-45c6-9c00-68099eebd485", "ecc07b18-f55e-4f6b-95bd-0e84f556135f" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2cdda855-1f15-4e11-9440-cfa84493cbd6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ba51b8f7-2a1d-45c6-9c00-68099eebd485");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "898d9efa-cd60-4446-b9ae-e0c48dd87c49");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ecc07b18-f55e-4f6b-95bd-0e84f556135f");
        }
    }
}
