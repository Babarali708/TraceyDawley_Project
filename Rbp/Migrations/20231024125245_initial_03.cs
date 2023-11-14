using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TraceyDawley.Migrations
{
    /// <inheritdoc />
    public partial class initial_03 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UserId" },
                values: new object[] { new DateTime(2023, 10, 24, 17, 52, 44, 906, DateTimeKind.Local).AddTicks(6410), new Guid("29817847-2dac-428d-a2a2-700375530e20") });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UserId" },
                values: new object[] { new DateTime(2023, 10, 24, 17, 52, 44, 906, DateTimeKind.Local).AddTicks(6433), new Guid("3da85078-3002-4c9f-b073-6c9706419ce2") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UserId" },
                values: new object[] { new DateTime(2023, 10, 24, 15, 58, 18, 694, DateTimeKind.Local).AddTicks(4806), new Guid("1456e8a6-6962-4b42-8974-fedcd3fff823") });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UserId" },
                values: new object[] { new DateTime(2023, 10, 24, 15, 58, 18, 694, DateTimeKind.Local).AddTicks(4824), new Guid("978d730e-5023-4de7-8d37-7ae79fc0246a") });
        }
    }
}
