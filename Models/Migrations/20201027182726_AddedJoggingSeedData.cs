using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.Migrations
{
    public partial class AddedJoggingSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "7d56a2a5-541e-44bd-9340-a435584c934f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "d05c7eae-dfe6-425f-8a35-671d77211e9d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "78d5c290-0874-4514-a122-11ab2afb9ee6");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2000,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "8f0dadac-1e98-42f6-945a-adbf87a7af9c", "AQAAAAEAACcQAAAAEKmbZHVb20PlkjFopZj0ukgoqnCNpyqQeOLuY8znWyi3t1OsdThACjP5l7IZ+Dd8yw==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2001,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "0ebf158d-8d36-4b3e-81af-8806aa7b8eee", "AQAAAAEAACcQAAAAECxCLc8S21Rf7m+yGzRheBnhuCeMcdfHK1iLvWjHtVAVSbVndgK4g/PQAPVQh85pNw==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2002,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "24292476-1fcc-4be9-98fa-62723fa5a12d", "AQAAAAEAACcQAAAAEN4I+xkxDDQciWCEkzPElOjc5QwmhOSQ8rVVlIKAXk+QWiXnjHJiXYrG9QTG1CCrKA==" });

            migrationBuilder.InsertData(
                table: "Joggings",
                columns: new[] { "Id", "DateCreated", "DateUpdated", "DistanceInMeters", "JoggingDate", "JoggingDurationInMinutes", "Location", "TemperatureC", "TemperatureF", "UserId", "WeatherCondition", "humidity" },
                values: new object[,]
                {
                    { 1000, new DateTime(2020, 10, 27, 14, 27, 26, 341, DateTimeKind.Local).AddTicks(3562), new DateTime(2020, 10, 27, 14, 27, 26, 341, DateTimeKind.Local).AddTicks(4029), 2000.0, new DateTime(2020, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 20, "London", 20f, 60f, 2001, "Clear", 30f },
                    { 1001, new DateTime(2020, 10, 27, 14, 27, 26, 341, DateTimeKind.Local).AddTicks(4718), new DateTime(2020, 10, 27, 14, 27, 26, 341, DateTimeKind.Local).AddTicks(4743), 3000.0, new DateTime(2020, 10, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 25, "Philadelphia", 20f, 60f, 2001, "Clear", 30f },
                    { 1002, new DateTime(2020, 10, 27, 14, 27, 26, 341, DateTimeKind.Local).AddTicks(4757), new DateTime(2020, 10, 27, 14, 27, 26, 341, DateTimeKind.Local).AddTicks(4760), 3500.0, new DateTime(2020, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 40, "Tashkent", 20f, 60f, 2001, "Clear", 30f },
                    { 1003, new DateTime(2020, 10, 27, 14, 27, 26, 341, DateTimeKind.Local).AddTicks(4765), new DateTime(2020, 10, 27, 14, 27, 26, 341, DateTimeKind.Local).AddTicks(4768), 6000.0, new DateTime(2020, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 60, "London", 20f, 60f, 2001, "Clear", 30f },
                    { 1004, new DateTime(2020, 10, 27, 14, 27, 26, 341, DateTimeKind.Local).AddTicks(4772), new DateTime(2020, 10, 27, 14, 27, 26, 341, DateTimeKind.Local).AddTicks(4775), 800.0, new DateTime(2020, 10, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, "London", 20f, 60f, 2001, "Clear", 30f },
                    { 1005, new DateTime(2020, 10, 27, 14, 27, 26, 341, DateTimeKind.Local).AddTicks(4780), new DateTime(2020, 10, 27, 14, 27, 26, 341, DateTimeKind.Local).AddTicks(4783), 5300.0, new DateTime(2020, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 50, "London", 20f, 60f, 2001, "Clear", 30f },
                    { 1006, new DateTime(2020, 10, 27, 14, 27, 26, 341, DateTimeKind.Local).AddTicks(4787), new DateTime(2020, 10, 27, 14, 27, 26, 341, DateTimeKind.Local).AddTicks(4790), 7000.0, new DateTime(2020, 10, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 80, "London", 20f, 60f, 2001, "Clear", 30f },
                    { 1007, new DateTime(2020, 10, 27, 14, 27, 26, 341, DateTimeKind.Local).AddTicks(4795), new DateTime(2020, 10, 27, 14, 27, 26, 341, DateTimeKind.Local).AddTicks(4798), 500.0, new DateTime(2020, 10, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, "London", 20f, 60f, 2001, "Clear", 30f },
                    { 1008, new DateTime(2020, 10, 27, 14, 27, 26, 341, DateTimeKind.Local).AddTicks(4802), new DateTime(2020, 10, 27, 14, 27, 26, 341, DateTimeKind.Local).AddTicks(4805), 10000.0, new DateTime(2020, 10, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 120, "London", 20f, 60f, 2001, "Clear", 30f }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Joggings",
                keyColumn: "Id",
                keyValue: 1000);

            migrationBuilder.DeleteData(
                table: "Joggings",
                keyColumn: "Id",
                keyValue: 1001);

            migrationBuilder.DeleteData(
                table: "Joggings",
                keyColumn: "Id",
                keyValue: 1002);

            migrationBuilder.DeleteData(
                table: "Joggings",
                keyColumn: "Id",
                keyValue: 1003);

            migrationBuilder.DeleteData(
                table: "Joggings",
                keyColumn: "Id",
                keyValue: 1004);

            migrationBuilder.DeleteData(
                table: "Joggings",
                keyColumn: "Id",
                keyValue: 1005);

            migrationBuilder.DeleteData(
                table: "Joggings",
                keyColumn: "Id",
                keyValue: 1006);

            migrationBuilder.DeleteData(
                table: "Joggings",
                keyColumn: "Id",
                keyValue: 1007);

            migrationBuilder.DeleteData(
                table: "Joggings",
                keyColumn: "Id",
                keyValue: 1008);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "334337a1-ba20-4d5a-b33c-8a407c372acd");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "ec7c3a04-f3b2-435a-8e58-96e1e6e4eca6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "844b0b22-6a18-4455-aca3-5e03f3378d70");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2000,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "fcb1292f-2a3a-4a26-bd68-0919631398bf", "AQAAAAEAACcQAAAAEIGoq9+woUXvqLJXECU1BF8oXtqE8FzE0O4fFPDyGdA4vg2z/Nhkuvu7Q/gfH2KyPg==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2001,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "e5b2c25e-0ce8-4048-a331-d31b0275030f", "AQAAAAEAACcQAAAAEOdJULI0Qa6mhox7rg+s2TthIosk3NhqZPK8qlGwf9QQr0W5KeoMHg+wIEmYQoZ4RQ==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2002,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "3b4176f6-6451-4b49-9cdb-8aa290977069", "AQAAAAEAACcQAAAAEOEplyT1DP271p0x3pI9JgWjH2xIZ+6Vej0/H3DuyGo7ZjzAAHfNGq5mzMQGG4esKg==" });
        }
    }
}
