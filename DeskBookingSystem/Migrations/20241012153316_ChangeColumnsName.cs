using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeskBookingSystem.Migrations
{
    /// <inheritdoc />
    public partial class ChangeColumnsName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Users",
                newName: "Password");

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BookingDate", "ReservationDate" },
                values: new object[] { new DateTime(2024, 10, 12, 17, 33, 16, 803, DateTimeKind.Local).AddTicks(4138), new DateTime(2024, 10, 13, 17, 33, 16, 803, DateTimeKind.Local).AddTicks(4193) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "PasswordHash");

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BookingDate", "ReservationDate" },
                values: new object[] { new DateTime(2024, 10, 12, 17, 17, 30, 745, DateTimeKind.Local).AddTicks(195), new DateTime(2024, 10, 13, 17, 17, 30, 745, DateTimeKind.Local).AddTicks(246) });
        }
    }
}
