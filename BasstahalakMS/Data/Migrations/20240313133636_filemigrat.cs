using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasstahalakMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class filemigrat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "325a3e6f-b33e-43d6-8cee-f6b0ad00f620",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a2bdfd70-ba34-4804-8913-c9e36b88608a", "AQAAAAIAAYagAAAAECGCX2C/GHZaI6V21uCsXlTQ1qXbYzOz+TZUOxGB/4yocg4eFfQCemQ+mTBnvoJi9Q==", "10597bd3-4308-4810-b496-c0183c3f760a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "898d9efa-cd60-4446-b9ae-e0c48dd87c49",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0c4e2cb1-7fd6-478d-9c7e-2ddc06f655b6", "AQAAAAIAAYagAAAAEK2f7qoBlN3cWHIong2pXqQIYujJwSmX6dGjunlFi3LhYbJfJzo+/3prcVmH/NDw7g==", "cbd88fd7-f826-42fa-aee0-d908ad671e2a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c2d7916d-74c1-4588-b2f2-6616b0e687f0",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4c61b03c-51c7-4e8d-a022-7cbb4dab6ffd", "AQAAAAIAAYagAAAAEOqor5rdTK+PpeYAwHRyCazmCwWpUN5NA4RCogunz/dY6ZO4wlG7p/3DlEML8BV7gw==", "261b56fd-e9bc-4090-9074-a9a4ad24ce02" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ecc07b18-f55e-4f6b-95bd-0e84f556135f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "29f31e88-a666-42fa-9e4e-7ea439438869", "AQAAAAIAAYagAAAAEB9eQlO1nPjE/6PYgWZLU0CG2kCR1z5BxNETD7orqxq1J8cZXrrY0Y/vLNLALuhQmQ==", "f6d0f14e-3c39-4ed3-ad27-f472de8cb4e4" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "325a3e6f-b33e-43d6-8cee-f6b0ad00f620",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "dba51b2e-58e2-452d-83d6-41f8ca3cb686", "AQAAAAIAAYagAAAAEHIh0uMlWHSd0VKpFAXk6Ki/ttycBk4zEjnFd0yUj7onL3ASl++qF7lE4TNGL2wmeQ==", "723d54aa-cfce-48cd-b9f2-0928d4146648" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "898d9efa-cd60-4446-b9ae-e0c48dd87c49",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "bb574c19-0d92-4f83-8636-165d8217aa97", "AQAAAAIAAYagAAAAEJGPlThdQGg5GdfjGhTEYKK3SxUXXuP4arIVaj+B+9J1Y8fw7XC3UsOpvwB2xUBi5w==", "4cfe1e7e-9360-4cfa-a6b6-48f2fe441e2a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c2d7916d-74c1-4588-b2f2-6616b0e687f0",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1b83deed-d939-4610-b7a8-0c98695340eb", "AQAAAAIAAYagAAAAEH8pHcpolC1d3CijHPUnLUUNPcD89ApApEExJaknvhE+2oHd2dYxO+NMp8XoPryzhg==", "cf369b09-4c55-4453-8e13-d2a76f531da7" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ecc07b18-f55e-4f6b-95bd-0e84f556135f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "23fbfab2-5adb-4ab4-9eb7-dde755f69173", "AQAAAAIAAYagAAAAEObGu+4TVqDwGwzcvgZQLWcqXt5r0dyUgUa98xilTZgLlkIiBZ+Kn9m6d+7GOzSM3Q==", "cbfdb6bb-99a8-46f2-8d0d-bebff26f1e33" });
        }
    }
}
