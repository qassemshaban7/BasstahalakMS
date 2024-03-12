using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BasstahalakMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class filetable : Migration
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
                    { "4be32c82-c795-4db6-89ac-8cc33b11d012", "الاعداد", "4be32c82-c795-4db6-89ac-8cc33b11d012", "ApplicationRole", "Prepare", "PREPARE" },
                    { "ba51b8f7-2a1d-45c6-9c00-68099eebd485", "المدير العام", "ba51b8f7-2a1d-45c6-9c00-68099eebd485", "ApplicationRole", "SuperAdmin", "SUPERADMIN" },
                    { "f770a463-640a-43f6-b9f6-a1317fe2c214", "المراجعة", "f770a463-640a-43f6-b9f6-a1317fe2c214", "ApplicationRole", "Review", "REVIEW" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "325a3e6f-b33e-43d6-8cee-f6b0ad00f620", 0, "240f3b41-3871-412c-8a84-6ed154bd2b4c", "ApplicationUser", "malek@gmail.com", true, "مالك ايهاب", false, null, "MALEK@GMAIL.COM", "MALEK", "AQAAAAIAAYagAAAAEGkPQxnnnw7QDStj0rKkflkdENizevHwdAlVUTRRIYomIntXTeKiFSiY40Sp14wUjQ==", "1234567890", false, "39786852-35fc-4f34-9e97-ba80ca8c5e1c", false, "malek" },
                    { "898d9efa-cd60-4446-b9ae-e0c48dd87c49", 0, "09a75c94-c92f-4fbf-8b96-ff03373fe003", "ApplicationUser", "ehab@gmail.com", true, "ايهاب ابراهيم ", false, null, "EHAB@GMAIL.COM", "EHAB", "AQAAAAIAAYagAAAAEIQM+PD8dTUictiidowaaQwWEqgt7tCAkex1G1kONJbOZ1bKUu/784+GZc20W6IoOA==", "1234567890", false, "14eb1e11-cfba-4b34-a0fe-825f63542801", false, "ehab" },
                    { "c2d7916d-74c1-4588-b2f2-6616b0e687f0", 0, "3db12ebf-0e94-460e-8759-9c798b06881b", "ApplicationUser", "shaban@gmail.com", true, "شعبان ابراهيم", false, null, "SHABAN@GMAIL.COM", "SHABAN", "AQAAAAIAAYagAAAAEDU+oSz8X8Rgb45xiybgtA4+vfMw1oVeDnygrKyd65xvTd4ws4IOWUFh/3hriKQnGw==", "1234567890", false, "42fde197-73bb-42ff-a778-dd733cab341b", false, "shaban" },
                    { "ecc07b18-f55e-4f6b-95bd-0e84f556135f", 0, "1d996ebd-bd72-4693-b06c-f7ae59571630", "ApplicationUser", "mohamedsalah@gmail.com", true, "محمد صلاح", false, null, "MOHAMEDSALAH@GMAIL.COM", "MOHAMEDSALAH", "AQAAAAIAAYagAAAAEBxhrUiPzbAFJG/fDku2UMPpJqdj4C6AJ0FUc9KaALdNqLzXXuACywktHgo2NYNX3Q==", "1234567890", false, "e0554e8c-48d8-4c93-8c3d-c9c0c6f88d4d", false, "mohamedsalah" }
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
                keyValue: "4be32c82-c795-4db6-89ac-8cc33b11d012");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ba51b8f7-2a1d-45c6-9c00-68099eebd485");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f770a463-640a-43f6-b9f6-a1317fe2c214");

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
