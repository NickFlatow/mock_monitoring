using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mock_monitoring.Migrations
{
    /// <inheritdoc />
    public partial class Update_Delete_Behavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_SensorLog_SensorLogId",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_Sensor_SensorId",
                table: "Event");

            migrationBuilder.UpdateData(
                table: "Sensor",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 5, 19, 16, 1, 397, DateTimeKind.Utc).AddTicks(1553));

            migrationBuilder.AddForeignKey(
                name: "FK_Event_SensorLog_SensorLogId",
                table: "Event",
                column: "SensorLogId",
                principalTable: "SensorLog",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Sensor_SensorId",
                table: "Event",
                column: "SensorId",
                principalTable: "Sensor",
                principalColumn: "Id");
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

            migrationBuilder.UpdateData(
                table: "Sensor",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 4, 21, 6, 46, 728, DateTimeKind.Utc).AddTicks(8103));

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
    }
}
