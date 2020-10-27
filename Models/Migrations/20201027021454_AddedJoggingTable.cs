using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.Migrations
{
    public partial class AddedJoggingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Joggings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JoggingDate = table.Column<DateTime>(nullable: false),
                    DistanceInMeters = table.Column<double>(nullable: false),
                    Location = table.Column<string>(maxLength: 500, nullable: false),
                    JoggingDurationInMinutes = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    TemperatureC = table.Column<float>(nullable: true),
                    TemperatureF = table.Column<float>(nullable: true),
                    humidity = table.Column<float>(nullable: true),
                    WeatherCondition = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Joggings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Joggings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Joggings_UserId",
                table: "Joggings",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Joggings");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "7e5dea42-99cb-47b0-9413-0d3f238dcebf");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "37f52f52-8a75-4016-832f-c39142796c34");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "589b8888-7dbb-4f65-a3ed-ecf4bdd42888");
        }
    }
}
