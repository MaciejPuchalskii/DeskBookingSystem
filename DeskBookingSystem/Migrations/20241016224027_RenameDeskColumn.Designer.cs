﻿// <auto-generated />
using System;
using DeskBookingSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DeskBookingSystem.Migrations
{
    [DbContext(typeof(BookingContext))]
    [Migration("20241016224027_RenameDeskColumn")]
    partial class RenameDeskColumn
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.20");

            modelBuilder.Entity("DeskBookingSystem.Models.Desk", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsOperational")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LocationId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.ToTable("Desks");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IsOperational = true,
                            LocationId = 1
                        },
                        new
                        {
                            Id = 2,
                            IsOperational = false,
                            LocationId = 1
                        },
                        new
                        {
                            Id = 3,
                            IsOperational = true,
                            LocationId = 1
                        },
                        new
                        {
                            Id = 4,
                            IsOperational = true,
                            LocationId = 2
                        },
                        new
                        {
                            Id = 5,
                            IsOperational = true,
                            LocationId = 3
                        },
                        new
                        {
                            Id = 6,
                            IsOperational = false,
                            LocationId = 3
                        },
                        new
                        {
                            Id = 7,
                            IsOperational = false,
                            LocationId = 4
                        },
                        new
                        {
                            Id = 8,
                            IsOperational = false,
                            LocationId = 4
                        },
                        new
                        {
                            Id = 9,
                            IsOperational = true,
                            LocationId = 5
                        });
                });

            modelBuilder.Entity("DeskBookingSystem.Models.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Locations");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Biuro Główne - Kraków"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Biuro - Wrocław"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Biuro - Warszawa"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Biuro - Rzeszów"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Biuro - Bielsko-Biała"
                        });
                });

            modelBuilder.Entity("DeskBookingSystem.Models.Reservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("BookingDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("DeskId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("HowManyDays")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ReservationDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("DeskId");

                    b.HasIndex("UserId");

                    b.ToTable("Reservations");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BookingDate = new DateTime(2024, 10, 17, 0, 40, 26, 984, DateTimeKind.Local).AddTicks(5315),
                            DeskId = 1,
                            HowManyDays = 2,
                            ReservationDate = new DateTime(2024, 10, 18, 0, 40, 26, 984, DateTimeKind.Local).AddTicks(5411),
                            UserId = 2
                        });
                });

            modelBuilder.Entity("DeskBookingSystem.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DeskBookingSystem.Models.Desk", b =>
                {
                    b.HasOne("DeskBookingSystem.Models.Location", "Location")
                        .WithMany("Desks")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Location");
                });

            modelBuilder.Entity("DeskBookingSystem.Models.Reservation", b =>
                {
                    b.HasOne("DeskBookingSystem.Models.Desk", "Desk")
                        .WithMany("Reservations")
                        .HasForeignKey("DeskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DeskBookingSystem.Models.User", "User")
                        .WithMany("Reservations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Desk");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DeskBookingSystem.Models.Desk", b =>
                {
                    b.Navigation("Reservations");
                });

            modelBuilder.Entity("DeskBookingSystem.Models.Location", b =>
                {
                    b.Navigation("Desks");
                });

            modelBuilder.Entity("DeskBookingSystem.Models.User", b =>
                {
                    b.Navigation("Reservations");
                });
#pragma warning restore 612, 618
        }
    }
}
