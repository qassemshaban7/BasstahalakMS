using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BasstahalakMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedSomeRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ArabicRoleName", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4be32c82-c795-4db6-89ac-8cc33b11d012", "الاعداد", "4be32c82-c795-4db6-89ac-8cc33b11d012", "ApplicationRole", "Prepare", "PREPARE" },
                    { "f770a463-640a-43f6-b9f6-a1317fe2c214", "المراجعة", "f770a463-640a-43f6-b9f6-a1317fe2c214", "ApplicationRole", "Review", "REVIEW" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4be32c82-c795-4db6-89ac-8cc33b11d012");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f770a463-640a-43f6-b9f6-a1317fe2c214");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ArabicRoleName", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2cdda855-1f15-4e11-9440-cfa84493cbd6", "المدير", "2cdda855-1f15-4e11-9440-cfa84493cbd6", "ApplicationRole", "Admin", "ADMIN" },
                    { "ba51b8f7-2a1d-45c6-9c00-68099eebd485", "المدير العام", "ba51b8f7-2a1d-45c6-9c00-68099eebd485", "ApplicationRole", "SuperAdmin", "SUPERADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "898d9efa-cd60-4446-b9ae-e0c48dd87c49", 0, "38261248-cd71-4137-8167-84f9ca5f10fb", "ApplicationUser", "ehab@gmail.com", true, "ايهاب ابراهيم ", false, null, "EHAB@GMAIL.COM", "EHAB", "AQAAAAIAAYagAAAAELfAksHZfkyK7ZgbHUdAhPoRNGSgPwYxLKCna8nNBjggVt8I1bvBIgy3ttpSEYZC5w==", "1234567890", false, "38363752-569d-43cc-8906-bb1ef12d4f8d", false, "ehab" },
                    { "ecc07b18-f55e-4f6b-95bd-0e84f556135f", 0, "eed207cd-d8e3-4d44-bd8b-bb701cd4e944", "ApplicationUser", "mohamedsalah@gmail.com", true, "محمد صلاح", false, null, "MOHAMEDSALAH@GMAIL.COM", "MOHAMEDSALAH", "AQAAAAIAAYagAAAAEAxVXBgfi+IPftlT/R0hqR+JCuUyWgsZGXJgLYcfiStaTYVEFyYfRBIUZXDaSisPfA==", "1234567890", false, "baae3b7a-9cb7-441b-9343-3ebc4bc1ea50", false, "mohamedsalah" }
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
    }
}
