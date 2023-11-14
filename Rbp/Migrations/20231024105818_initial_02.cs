using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TraceyDawley.Migrations
{
    /// <inheritdoc />
    public partial class initial_02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "encEmail",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ContentFile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(355)", maxLength: 355, nullable: true),
                    FileExtension = table.Column<string>(type: "nvarchar(355)", maxLength: 355, nullable: true),
                    FileSize = table.Column<double>(type: "float", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    UploadedBy = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentFile_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UserId", "encEmail" },
                values: new object[] { new DateTime(2023, 10, 24, 15, 58, 18, 694, DateTimeKind.Local).AddTicks(4806), new Guid("1456e8a6-6962-4b42-8974-fedcd3fff823"), null });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UserId", "encEmail" },
                values: new object[] { new DateTime(2023, 10, 24, 15, 58, 18, 694, DateTimeKind.Local).AddTicks(4824), new Guid("978d730e-5023-4de7-8d37-7ae79fc0246a"), null });

            migrationBuilder.CreateIndex(
                name: "IX_ContentFile_UserId",
                table: "ContentFile",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContentFile");

            migrationBuilder.DropColumn(
                name: "encEmail",
                table: "User");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UserId" },
                values: new object[] { new DateTime(2023, 10, 24, 11, 54, 20, 480, DateTimeKind.Local).AddTicks(8745), new Guid("2c516c66-a794-4e14-8a0c-4ccdf00cfbc0") });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UserId" },
                values: new object[] { new DateTime(2023, 10, 24, 11, 54, 20, 480, DateTimeKind.Local).AddTicks(8767), new Guid("9d23a508-6a7c-4881-b231-c37c757cacfe") });
        }
    }
}
