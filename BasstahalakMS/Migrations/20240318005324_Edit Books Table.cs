using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasstahalakMS.Migrations
{
    /// <inheritdoc />
    public partial class EditBooksTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "325a3e6f-b33e-43d6-8cee-f6b0ad00f620",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cc69f0c9-35ed-4a67-bfd8-e8d8cf9c9923", "AQAAAAIAAYagAAAAEHg4Be/cfS9BUmlpp05JgnfA3//WyevP1rrPbbgtDyYR/JVgrb4OAseM4V/0gz6KaA==", "5060c41f-8a9c-45e0-981e-27d7fca8bb1a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "898d9efa-cd60-4446-b9ae-e0c48dd87c49",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "29136026-6d6c-442c-9b1f-392a01a093fb", "AQAAAAIAAYagAAAAEBkCAlYq65N2x2LwXw7S1XYAbZamMccYIb3Vy+RhkblotVFHuPK/QzIryLDyIVUnsw==", "751c986b-6f29-424e-ad8f-e42a3b4cb32f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c2d7916d-74c1-4588-b2f2-6616b0e687f0",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9fadfc16-ff4d-443e-b159-6ab82d9333b7", "AQAAAAIAAYagAAAAEO+M/ggz1qc1AFOpl96gY+MqFkgvV+jYa8FS1Hqq3/9fGQ3r8xMAfrhPYaWaovXxsg==", "2d77bd4b-6e0b-4c61-b89c-3b77563cd6eb" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ecc07b18-f55e-4f6b-95bd-0e84f556135f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "de42f7af-a83d-4999-a57a-40ccaca05d5f", "AQAAAAIAAYagAAAAEMW/WDuJlHbahEY0z+8mvlhg0HkSVK+zkAULrPkdizdaRF/wgoL04+gYa3HDV7RDzA==", "11592d66-f898-428b-afb9-e45281936c50" });
        }
    }
}
