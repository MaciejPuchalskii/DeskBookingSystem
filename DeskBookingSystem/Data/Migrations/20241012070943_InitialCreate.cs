using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DeskBookingSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Surname = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    IsAdmin = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Desks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsAvailable = table.Column<bool>(type: "INTEGER", nullable: false),
                    LocationId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Desks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Desks_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BookingDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReservationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    HowManyDays = table.Column<int>(type: "INTEGER", nullable: false),
                    DeskId = table.Column<int>(type: "INTEGER", nullable: false),
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_Desks_DeskId",
                        column: x => x.DeskId,
                        principalTable: "Desks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservations_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Email", "IsAdmin", "Name", "Surname" },
                values: new object[,]
                {
                    { 1, "jan.kowalski@mail.com", true, "Jan", "Kowalski" },
                    { 2, "anna.kowalska@example.com", false, "Anna", "Kowalska" }
                });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Biuro Główne - Kraków" },
                    { 2, "Biuro - Wrocław" },
                    { 3, "Biuro - Warszawa" },
                    { 4, "Biuro - Rzeszów" },
                    { 5, "Biuro - Bielsko-Biała" }
                });

            migrationBuilder.InsertData(
                table: "Desks",
                columns: new[] { "Id", "IsAvailable", "LocationId" },
                values: new object[,]
                {
                    { 1, true, 1 },
                    { 2, false, 1 },
                    { 3, true, 1 },
                    { 4, true, 2 },
                    { 5, true, 3 },
                    { 6, false, 3 },
                    { 7, false, 4 },
                    { 8, false, 4 },
                    { 9, true, 5 }
                });

            migrationBuilder.InsertData(
                table: "Reservations",
                columns: new[] { "Id", "BookingDate", "DeskId", "EmployeeId", "HowManyDays", "ReservationDate" },
                values: new object[] { 1, new DateTime(2024, 10, 12, 9, 9, 43, 371, DateTimeKind.Local).AddTicks(3056), 1, 2, 2, new DateTime(2024, 10, 13, 9, 9, 43, 371, DateTimeKind.Local).AddTicks(3113) });

            migrationBuilder.CreateIndex(
                name: "IX_Desks_LocationId",
                table: "Desks",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_DeskId",
                table: "Reservations",
                column: "DeskId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_EmployeeId",
                table: "Reservations",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Desks");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Locations");
        }
    }
}