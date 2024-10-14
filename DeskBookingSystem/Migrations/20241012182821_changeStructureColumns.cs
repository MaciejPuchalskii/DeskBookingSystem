using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeskBookingSystem.Migrations
{
    /// <inheritdoc />
    public partial class changeStructureColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "PasswordHash");

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BookingDate", "ReservationDate" },
                values: new object[] { new DateTime(2024, 10, 12, 20, 28, 21, 461, DateTimeKind.Local).AddTicks(4553), new DateTime(2024, 10, 13, 20, 28, 21, 461, DateTimeKind.Local).AddTicks(4607) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "Users");

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
    }
}
