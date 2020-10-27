using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.Migrations
{
    public partial class UserSeedAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { 2000, 0, "024e772c-9711-4313-99d7-a046e1c872aa", "adminuser@gmail.com", true, null, null, false, null, null, null, "AQAAAAEAACcQAAAAELLFNSqzAlskY03oQUsTBm1F/ddDu1Ilmbf8Mll/X1SAntHTHnjzr6Yc9TyTdk30rg==", null, false, "", false, "adminuser" },
                    { 2001, 0, "0a2d7f23-ee46-4be8-98e1-ae2abc0c1864", "manageruser@gmail.com", true, null, null, false, null, null, null, "AQAAAAEAACcQAAAAENNaSrUxhh2N+zeJwVsTiNIQs43LQCzleG6t2EG4La07tecwGhH3rn0QZA9ABWNzXg==", null, false, "", false, "manageruser" },
                    { 2002, 0, "d9ddd9f3-22e2-4865-b196-9443010ea502", "joggeruser@gmail.com", true, null, null, false, null, null, null, "AQAAAAEAACcQAAAAEIIGho2sFo+EVO+9jNGJroHCpg1AzmdubjcNtLu2B38m9QG/1Omb4kx9Vk2AfMY1ug==", null, false, "", false, "joggeruser" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2000);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2001);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2002);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "a23c6363-34a9-4eb3-b528-710aacc730f0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "7b95d691-d274-4d1c-8a69-db9fc6937967");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "2289217b-147a-4858-98e8-8fe35658c24a");
        }
    }
}
