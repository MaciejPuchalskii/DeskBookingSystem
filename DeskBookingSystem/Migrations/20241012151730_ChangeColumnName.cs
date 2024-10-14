using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeskBookingSystem.Migrations
{
    /// <inheritdoc />
    public partial class ChangeColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Users_EmployeeId",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Reservations",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_EmployeeId",
                table: "Reservations",
                newName: "IX_Reservations_UserId");

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BookingDate", "ReservationDate" },
                values: new object[] { new DateTime(2024, 10, 12, 17, 17, 30, 745, DateTimeKind.Local).AddTicks(195), new DateTime(2024, 10, 13, 17, 17, 30, 745, DateTimeKind.Local).AddTicks(246) });

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Users_UserId",
                table: "Reservations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Users_UserId",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Reservations",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_UserId",
                table: "Reservations",
                newName: "IX_Reservations_EmployeeId");

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BookingDate", "ReservationDate" },
                values: new object[] { new DateTime(2024, 10, 12, 17, 15, 36, 454, DateTimeKind.Local).AddTicks(5773), new DateTime(2024, 10, 13, 17, 15, 36, 454, DateTimeKind.Local).AddTicks(5822) });

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Users_EmployeeId",
                table: "Reservations",
                column: "EmployeeId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
