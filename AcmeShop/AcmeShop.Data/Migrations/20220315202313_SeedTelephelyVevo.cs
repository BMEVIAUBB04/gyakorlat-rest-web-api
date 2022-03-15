using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcmeShop.Data.Migrations
{
    public partial class SeedTelephelyVevo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Telephely",
                keyColumn: "Id",
                keyValue: 1,
                column: "VevoId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Telephely",
                keyColumn: "Id",
                keyValue: 2,
                column: "VevoId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Telephely",
                keyColumn: "Id",
                keyValue: 3,
                column: "VevoId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Telephely",
                keyColumn: "Id",
                keyValue: 4,
                column: "VevoId",
                value: 3);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Telephely",
                keyColumn: "Id",
                keyValue: 1,
                column: "VevoId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Telephely",
                keyColumn: "Id",
                keyValue: 2,
                column: "VevoId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Telephely",
                keyColumn: "Id",
                keyValue: 3,
                column: "VevoId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Telephely",
                keyColumn: "Id",
                keyValue: 4,
                column: "VevoId",
                value: null);
        }
    }
}
