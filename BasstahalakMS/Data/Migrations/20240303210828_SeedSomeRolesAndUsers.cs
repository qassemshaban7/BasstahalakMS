using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BasstahalakMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedSomeRolesAndUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    { "325a3e6f-b33e-43d6-8cee-f6b0ad00f620", 0, "3ee145c4-4708-45b9-ae5d-a711e4063249", "ApplicationUser", "malek@gmail.com", true, "مالك ايهاب", false, null, "MALEK@GMAIL.COM", "MALEK", "AQAAAAIAAYagAAAAEBMfl6QrYyVrTNDTAQIciKRH8tSNFMC8k82OgehLJqwee8EWIC3yxsEsQIhroXITfA==", "1234567890", false, "ca7abd73-c181-4152-ab2b-17b725dd94ae", false, "malek" },
                    { "898d9efa-cd60-4446-b9ae-e0c48dd87c49", 0, "65fc7d5b-d9e5-4c57-9ebc-dd0c9721eab5", "ApplicationUser", "ehab@gmail.com", true, "ايهاب ابراهيم ", false, null, "EHAB@GMAIL.COM", "EHAB", "AQAAAAIAAYagAAAAEB0NSKHbFyRxx1hdgCeY1TjzQG3tpa/FUxIsGWfNB0FT6gp0Pk0lSco5Dud/H7kfBg==", "1234567890", false, "677cab3d-fdb4-426e-9aba-71a8b50ab626", false, "ehab" },
                    { "c2d7916d-74c1-4588-b2f2-6616b0e687f0", 0, "cb5947b3-c171-4506-81cc-e976cfdeda09", "ApplicationUser", "shaban@gmail.com", true, "شعبان ابراهيم", false, null, "SHABAN@GMAIL.COM", "SHABAN", "AQAAAAIAAYagAAAAEOhm4B6W391zl+SiEm3XO8Pu2+3EtrkS/LRe1LOIQ55MT/uEi1J/eY03b9ZeIWajuQ==", "1234567890", false, "7675bf90-ee9f-4a86-b535-35f3ee3139c4", false, "shaban" },
                    { "ecc07b18-f55e-4f6b-95bd-0e84f556135f", 0, "f023c90e-45ae-46d6-811f-84c2e10716df", "ApplicationUser", "mohamedsalah@gmail.com", true, "محمد صلاح", false, null, "MOHAMEDSALAH@GMAIL.COM", "MOHAMEDSALAH", "AQAAAAIAAYagAAAAEGq/Db/H14qK8airyWY7WVxJcbx/AWUzxoJ//RHHbslSev5snCt9pWp30Ya5X456qQ==", "1234567890", false, "067b46b3-4ff1-414d-9634-61bfbf83934f", false, "mohamedsalah" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "f770a463-640a-43f6-b9f6-a1317fe2c214", "325a3e6f-b33e-43d6-8cee-f6b0ad00f620" },
                    { "2cdda855-1f15-4e11-9440-cfa84493cbd6", "898d9efa-cd60-4446-b9ae-e0c48dd87c49" },
                    { "4be32c82-c795-4db6-89ac-8cc33b11d012", "c2d7916d-74c1-4588-b2f2-6616b0e687f0" },
                    { "ba51b8f7-2a1d-45c6-9c00-68099eebd485", "ecc07b18-f55e-4f6b-95bd-0e84f556135f" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "f770a463-640a-43f6-b9f6-a1317fe2c214", "325a3e6f-b33e-43d6-8cee-f6b0ad00f620" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2cdda855-1f15-4e11-9440-cfa84493cbd6", "898d9efa-cd60-4446-b9ae-e0c48dd87c49" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "4be32c82-c795-4db6-89ac-8cc33b11d012", "c2d7916d-74c1-4588-b2f2-6616b0e687f0" });

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
                keyValue: "325a3e6f-b33e-43d6-8cee-f6b0ad00f620");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "898d9efa-cd60-4446-b9ae-e0c48dd87c49");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c2d7916d-74c1-4588-b2f2-6616b0e687f0");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ecc07b18-f55e-4f6b-95bd-0e84f556135f");
        }
    }
}
