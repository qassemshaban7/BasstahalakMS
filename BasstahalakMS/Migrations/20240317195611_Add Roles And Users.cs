using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BasstahalakMS.Migrations
{
    /// <inheritdoc />
    public partial class AddRolesAndUsers : Migration
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
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "Type", "UserName" },
                values: new object[,]
                {
                    { "325a3e6f-b33e-43d6-8cee-f6b0ad00f620", 0, null, "cc69f0c9-35ed-4a67-bfd8-e8d8cf9c9923", "ApplicationUser", "malek@gmail.com", true, "مالك ايهاب", false, null, "MALEK@GMAIL.COM", "MALEK", "AQAAAAIAAYagAAAAEHg4Be/cfS9BUmlpp05JgnfA3//WyevP1rrPbbgtDyYR/JVgrb4OAseM4V/0gz6KaA==", "1234567890", false, "5060c41f-8a9c-45e0-981e-27d7fca8bb1a", false, null, "malek" },
                    { "898d9efa-cd60-4446-b9ae-e0c48dd87c49", 0, null, "29136026-6d6c-442c-9b1f-392a01a093fb", "ApplicationUser", "ehab@gmail.com", true, "ايهاب ابراهيم ", false, null, "EHAB@GMAIL.COM", "EHAB", "AQAAAAIAAYagAAAAEBkCAlYq65N2x2LwXw7S1XYAbZamMccYIb3Vy+RhkblotVFHuPK/QzIryLDyIVUnsw==", "1234567890", false, "751c986b-6f29-424e-ad8f-e42a3b4cb32f", false, null, "ehab" },
                    { "c2d7916d-74c1-4588-b2f2-6616b0e687f0", 0, null, "9fadfc16-ff4d-443e-b159-6ab82d9333b7", "ApplicationUser", "shaban@gmail.com", true, "شعبان ابراهيم", false, null, "SHABAN@GMAIL.COM", "SHABAN", "AQAAAAIAAYagAAAAEO+M/ggz1qc1AFOpl96gY+MqFkgvV+jYa8FS1Hqq3/9fGQ3r8xMAfrhPYaWaovXxsg==", "1234567890", false, "2d77bd4b-6e0b-4c61-b89c-3b77563cd6eb", false, null, "shaban" },
                    { "ecc07b18-f55e-4f6b-95bd-0e84f556135f", 0, null, "de42f7af-a83d-4999-a57a-40ccaca05d5f", "ApplicationUser", "mohamedsalah@gmail.com", true, "محمد صلاح", false, null, "MOHAMEDSALAH@GMAIL.COM", "MOHAMEDSALAH", "AQAAAAIAAYagAAAAEMW/WDuJlHbahEY0z+8mvlhg0HkSVK+zkAULrPkdizdaRF/wgoL04+gYa3HDV7RDzA==", "1234567890", false, "11592d66-f898-428b-afb9-e45281936c50", false, null, "mohamedsalah" }
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
