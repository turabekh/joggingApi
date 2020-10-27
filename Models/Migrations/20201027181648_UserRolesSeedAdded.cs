using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.Migrations
{
    public partial class UserRolesSeedAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId", "Discriminator" },
                values: new object[,]
                {
                    { 2000, 2, "UserRole" },
                    { 2001, 1, "UserRole" },
                    { 2002, 3, "UserRole" }
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { 2000, 2 });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { 2001, 1 });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { 2002, 3 });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "4024f0fc-3dc0-4192-b22e-e7e376b59c69");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "86baea0a-78cf-4b8e-9287-10805d21a0f4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "88f64c2f-f402-4057-bc0f-6b7408a4f1d0");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2000,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "024e772c-9711-4313-99d7-a046e1c872aa", "AQAAAAEAACcQAAAAELLFNSqzAlskY03oQUsTBm1F/ddDu1Ilmbf8Mll/X1SAntHTHnjzr6Yc9TyTdk30rg==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2001,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "0a2d7f23-ee46-4be8-98e1-ae2abc0c1864", "AQAAAAEAACcQAAAAENNaSrUxhh2N+zeJwVsTiNIQs43LQCzleG6t2EG4La07tecwGhH3rn0QZA9ABWNzXg==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2002,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "d9ddd9f3-22e2-4865-b196-9443010ea502", "AQAAAAEAACcQAAAAEIIGho2sFo+EVO+9jNGJroHCpg1AzmdubjcNtLu2B38m9QG/1Omb4kx9Vk2AfMY1ug==" });
        }
    }
}
