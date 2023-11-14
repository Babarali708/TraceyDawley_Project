using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TraceyDawley.Migrations
{
    /// <inheritdoc />
    public partial class initial_0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Contact = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(355)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Contact", "CreatedAt", "DeletedAt", "Email", "FirstName", "IsActive", "LastName", "MiddleName", "ModifiedAt", "Password", "Role", "UserId" },
                values: new object[,]
                {
                    { 1, "00000000000", new DateTime(2023, 10, 24, 11, 54, 20, 480, DateTimeKind.Local).AddTicks(8745), null, "superadmin@gmail.com", "Super", 1, "Admin", "", null, "123", 0, new Guid("2c516c66-a794-4e14-8a0c-4ccdf00cfbc0") },
                    { 2, "00000000000", new DateTime(2023, 10, 24, 11, 54, 20, 480, DateTimeKind.Local).AddTicks(8767), null, "admin@gmail.com", "Admin", 1, "", "", null, "123", 1, new Guid("9d23a508-6a7c-4881-b231-c37c757cacfe") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
