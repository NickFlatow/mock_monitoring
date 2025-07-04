using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mock_monitoring.Migrations
{
    /// <inheritdoc />
    public partial class Event_Sensor_SensorLog_FKs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Sensor",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 16, 16, 31, 55, 232, DateTimeKind.Utc).AddTicks(1516));

            migrationBuilder.CreateIndex(
                name: "IX_Event_SensorId",
                table: "Event",
                column: "SensorId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_SensorLogId",
                table: "Event",
                column: "SensorLogId");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_SensorLog_SensorLogId",
                table: "Event",
                column: "SensorLogId",
                principalTable: "SensorLog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Sensor_SensorId",
                table: "Event",
                column: "SensorId",
                principalTable: "Sensor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_SensorLog_SensorLogId",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_Sensor_SensorId",
                table: "Event");

            migrationBuilder.DropIndex(
                name: "IX_Event_SensorId",
                table: "Event");

            migrationBuilder.DropIndex(
                name: "IX_Event_SensorLogId",
                table: "Event");

            migrationBuilder.UpdateData(
                table: "Sensor",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 14, 21, 16, 48, 293, DateTimeKind.Utc).AddTicks(6101));
        }
    }
}
