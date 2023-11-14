using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TraceyDawley.Migrations
{
    /// <inheritdoc />
    public partial class initial_005 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "User",
                type: "nvarchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DateOfBirth",
                table: "User",
                type: "nvarchar(255)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Address", "CreatedAt", "DateOfBirth", "UserId" },
                values: new object[] { null, new DateTime(2023, 10, 27, 12, 3, 49, 779, DateTimeKind.Local).AddTicks(1321), null, new Guid("7686864c-6728-4a69-877a-5ab790262a12") });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Address", "CreatedAt", "DateOfBirth", "UserId" },
                values: new object[] { null, new DateTime(2023, 10, 27, 12, 3, 49, 779, DateTimeKind.Local).AddTicks(1336), null, new Guid("ca328a43-ca62-4737-960d-6b9ad580515d") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "User");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "User");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UserId" },
                values: new object[] { new DateTime(2023, 10, 26, 12, 59, 19, 662, DateTimeKind.Local).AddTicks(5302), new Guid("febb2d84-985a-4a0b-b93d-c6449da621a2") });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UserId" },
                values: new object[] { new DateTime(2023, 10, 26, 12, 59, 19, 662, DateTimeKind.Local).AddTicks(5338), new Guid("e3ebc057-c968-44d3-8da2-f6219adbe708") });
        }
    }
}
