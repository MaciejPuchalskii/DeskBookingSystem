using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeskBookingSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeColumnNameDaysNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HowManyDays",
                table: "Reservations",
                newName: "DaysCount");

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BookingDate", "ReservationDate" },
                values: new object[] { new DateTime(2024, 10, 17, 14, 49, 0, 858, DateTimeKind.Local).AddTicks(8066), new DateTime(2024, 10, 18, 14, 49, 0, 858, DateTimeKind.Local).AddTicks(8115) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DaysCount",
                table: "Reservations",
                newName: "HowManyDays");

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BookingDate", "ReservationDate" },
                values: new object[] { new DateTime(2024, 10, 17, 0, 40, 26, 984, DateTimeKind.Local).AddTicks(5315), new DateTime(2024, 10, 18, 0, 40, 26, 984, DateTimeKind.Local).AddTicks(5411) });
        }
    }
}
