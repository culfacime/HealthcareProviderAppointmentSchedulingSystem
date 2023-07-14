using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Healthcare.Core.Migrations
{
    /// <inheritdoc />
    public partial class UserTestData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "UserId", "Active", "Password", "UserName" },
                values: new object[] { new Guid("1b2ec0b9-84c6-49b1-8b5d-23bc605fef5f"), true, "123456", "Test" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "UserId",
                keyValue: new Guid("1b2ec0b9-84c6-49b1-8b5d-23bc605fef5f"));
        }
    }
}
