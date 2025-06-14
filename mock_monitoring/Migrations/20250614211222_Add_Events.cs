using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mock_monitoring.Migrations
{
    /// <inheritdoc />
    public partial class Add_Events : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Sensor",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 14, 21, 12, 21, 262, DateTimeKind.Utc).AddTicks(3742));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Sensor",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 13, 16, 37, 20, 243, DateTimeKind.Utc).AddTicks(6360));
        }
    }
}
