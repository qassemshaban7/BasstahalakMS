using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasstahalakMS.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusColumnToBFilesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "BFiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "325a3e6f-b33e-43d6-8cee-f6b0ad00f620",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5234aa90-321e-4706-a01b-979d4c713000", "AQAAAAIAAYagAAAAEKaqe/rnNx18jF4/KcmgE5yVtkMbXtowgJqob3sWCFdnPqVPnCh1mr40LkGQuS3XEw==", "36bc617f-1373-4a2d-9dd8-c8b60881fd40" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "898d9efa-cd60-4446-b9ae-e0c48dd87c49",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3a82faa7-b548-461a-8570-74fd2df08d78", "AQAAAAIAAYagAAAAEJv9Ic/2ND8/OBiayi9OQv6ZPnuiUKmVURGPLYRbd0BMNx9HWF/eRILJC8GhrpgFgg==", "b9238191-2a48-4a43-8758-b141380371d4" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c2d7916d-74c1-4588-b2f2-6616b0e687f0",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0eb44923-33b7-4f8e-9430-b18adb7eb3bd", "AQAAAAIAAYagAAAAEIlyRcIkBSabqoYKv5AI8yoDzxej4t1+y/t0739fRxWLrWw7lwvlztPX457OVT7DVQ==", "d05ec512-32f8-4929-94f3-0517f9922e7e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ecc07b18-f55e-4f6b-95bd-0e84f556135f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ad60afef-4fd7-4a30-aee5-263d548d6ad9", "AQAAAAIAAYagAAAAEPuQg1h5QXf8ErJTG7SnohTgv4M1y3eipdSju0CSKmxXj8tBQFZG5yvLomvbVUJ3OQ==", "b5c4c3d0-69ac-4d53-8e3e-8a600dc8c307" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "BFiles");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "325a3e6f-b33e-43d6-8cee-f6b0ad00f620",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "998ab9a8-9f05-4ce7-b08c-45345371c163", "AQAAAAIAAYagAAAAEPFuZBEio1VbFSSUGXY2ngDRUU2xDyvkrPqLnyEO2MlT8YKoiyfdwlwBRsR7OZkcIw==", "f40be0ff-5c66-4f2f-bb97-3514d37d3c8b" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "898d9efa-cd60-4446-b9ae-e0c48dd87c49",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "75792527-2437-45ee-8467-5eb791ff9513", "AQAAAAIAAYagAAAAEIec3UDTXqPIb+3i/aZZzINGOTIose8qv5j8Cs1rUdbhkP3Zwba04E1Oefsvn92BRQ==", "06731447-49b1-4a57-b3bd-cdb6405885a5" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c2d7916d-74c1-4588-b2f2-6616b0e687f0",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "11f71e12-657f-4741-b2a8-29c26298e02c", "AQAAAAIAAYagAAAAENiEPIQdgjIXA+4XzyafO0ilH3jiUJV1b7K+oe9oexBWM/7LXchOd/yOjCGg13bukw==", "231154fa-49e1-4f2e-9677-e2411127b0de" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ecc07b18-f55e-4f6b-95bd-0e84f556135f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5c908666-2f10-4fe5-b118-62f027ff6b6b", "AQAAAAIAAYagAAAAEJDpBqN2x0JBnWxbLEf5xXctVGSQMD42dVQ+NLzYer6vmAk1rapUSwe6DRn0fsQv7w==", "0e54fcdf-0c75-4816-8bce-abac4dc28660" });
        }
    }
}
