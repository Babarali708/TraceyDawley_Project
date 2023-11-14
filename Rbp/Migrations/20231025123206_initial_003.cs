using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TraceyDawley.Migrations
{
    /// <inheritdoc />
    public partial class initial_003 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SurveyResponseData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question1 = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Question2 = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Question3 = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Question4 = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Question5 = table.Column<string>(type: "nvarchar(355)", nullable: true),
                    Question6 = table.Column<string>(type: "nvarchar(355)", nullable: true),
                    Question7 = table.Column<string>(type: "nvarchar(355)", nullable: true),
                    Question8 = table.Column<string>(type: "nvarchar(355)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyResponseData", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SurveyResponseData");

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
    }
}
