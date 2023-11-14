using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TraceyDawley.Migrations
{
    /// <inheritdoc />
    public partial class initial_004 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UserId" },
                values: new object[] { new DateTime(2023, 10, 25, 17, 32, 6, 157, DateTimeKind.Local).AddTicks(4019), new Guid("a59f1fa5-f51b-4d05-ad27-3ddaeaaf49bf") });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UserId" },
                values: new object[] { new DateTime(2023, 10, 25, 17, 32, 6, 157, DateTimeKind.Local).AddTicks(4044), new Guid("199f84be-2212-4e10-80e9-638ea07be12e") });
        }
    }
}
