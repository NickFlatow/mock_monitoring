using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mock_monitoring.Migrations
{
    /// <inheritdoc />
    public partial class Bin_Sensor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Sensor",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 11, 19, 2, 44, 425, DateTimeKind.Utc).AddTicks(7389));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Sensor",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 5, 20, 7, 34, 711, DateTimeKind.Utc).AddTicks(6312));
        }
    }
}
