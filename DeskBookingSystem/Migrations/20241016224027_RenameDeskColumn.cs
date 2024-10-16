using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeskBookingSystem.Migrations
{
    /// <inheritdoc />
    public partial class RenameDeskColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsAvailable",
                table: "Desks",
                newName: "IsOperational");

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BookingDate", "ReservationDate" },
                values: new object[] { new DateTime(2024, 10, 17, 0, 40, 26, 984, DateTimeKind.Local).AddTicks(5315), new DateTime(2024, 10, 18, 0, 40, 26, 984, DateTimeKind.Local).AddTicks(5411) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsOperational",
                table: "Desks",
                newName: "IsAvailable");

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BookingDate", "ReservationDate" },
                values: new object[] { new DateTime(2024, 10, 12, 20, 28, 21, 461, DateTimeKind.Local).AddTicks(4553), new DateTime(2024, 10, 13, 20, 28, 21, 461, DateTimeKind.Local).AddTicks(4607) });
        }
    }
}
