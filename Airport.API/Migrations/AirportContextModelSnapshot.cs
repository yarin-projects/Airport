﻿// <auto-generated />
using System;
using Airport.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Airport.API.Migrations
{
    [DbContext(typeof(AirportContext))]
    partial class AirportContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.7");

            modelBuilder.Entity("Airport.API.Models.Flight", b =>
                {
                    b.Property<int>("FlightId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("FlightStatus")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDone")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("LegId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Model")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("Number")
                        .HasColumnType("TEXT");

                    b.Property<int>("PassengersCount")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("TimeCompleted")
                        .HasColumnType("TEXT");

                    b.HasKey("FlightId");

                    b.ToTable("Flights");
                });

            modelBuilder.Entity("Airport.API.Models.Leg", b =>
                {
                    b.Property<int>("LegId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<TimeSpan>("CrossingTime")
                        .HasColumnType("TEXT");

                    b.Property<int?>("FlightId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LegType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("TEXT")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.HasKey("LegId");

                    b.HasIndex("FlightId")
                        .IsUnique();

                    b.ToTable("Legs");

                    b.HasData(
                        new
                        {
                            LegId = 1,
                            CrossingTime = new TimeSpan(0, 0, 0, 10, 0),
                            LegType = 1
                        },
                        new
                        {
                            LegId = 2,
                            CrossingTime = new TimeSpan(0, 0, 0, 9, 0),
                            LegType = 1
                        },
                        new
                        {
                            LegId = 3,
                            CrossingTime = new TimeSpan(0, 0, 0, 9, 0),
                            LegType = 1
                        },
                        new
                        {
                            LegId = 4,
                            CrossingTime = new TimeSpan(0, 0, 0, 8, 0),
                            LegType = 3
                        },
                        new
                        {
                            LegId = 5,
                            CrossingTime = new TimeSpan(0, 0, 0, 12, 0),
                            LegType = 1
                        },
                        new
                        {
                            LegId = 6,
                            CrossingTime = new TimeSpan(0, 0, 0, 4, 0),
                            LegType = 3
                        },
                        new
                        {
                            LegId = 7,
                            CrossingTime = new TimeSpan(0, 0, 0, 11, 0),
                            LegType = 3
                        },
                        new
                        {
                            LegId = 8,
                            CrossingTime = new TimeSpan(0, 0, 0, 11, 0),
                            LegType = 2
                        },
                        new
                        {
                            LegId = 9,
                            CrossingTime = new TimeSpan(0, 0, 0, 6, 0),
                            LegType = 2
                        });
                });

            modelBuilder.Entity("Airport.API.Models.LegConnection", b =>
                {
                    b.Property<int>("LegId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NextLegId")
                        .HasColumnType("INTEGER");

                    b.HasKey("LegId", "NextLegId");

                    b.HasIndex("NextLegId");

                    b.ToTable("LegConnections");

                    b.HasData(
                        new
                        {
                            LegId = 1,
                            NextLegId = 2
                        },
                        new
                        {
                            LegId = 2,
                            NextLegId = 3
                        },
                        new
                        {
                            LegId = 3,
                            NextLegId = 4
                        },
                        new
                        {
                            LegId = 4,
                            NextLegId = 5
                        },
                        new
                        {
                            LegId = 4,
                            NextLegId = 9
                        },
                        new
                        {
                            LegId = 5,
                            NextLegId = 6
                        },
                        new
                        {
                            LegId = 5,
                            NextLegId = 7
                        },
                        new
                        {
                            LegId = 6,
                            NextLegId = 8
                        },
                        new
                        {
                            LegId = 7,
                            NextLegId = 8
                        },
                        new
                        {
                            LegId = 8,
                            NextLegId = 4
                        });
                });

            modelBuilder.Entity("Airport.API.Models.Log", b =>
                {
                    b.Property<int>("LogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("FlightId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FlightNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsWaitingList")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LegId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("TimeEntered")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("TimeExit")
                        .HasColumnType("TEXT");

                    b.HasKey("LogId");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("Airport.API.Models.WaitingFlight", b =>
                {
                    b.Property<int>("WaitingFlightId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("FlightId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FlightStatus")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LegIdToEnter")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Model")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("Number")
                        .HasColumnType("TEXT");

                    b.Property<int>("PassengersCount")
                        .HasColumnType("INTEGER");

                    b.HasKey("WaitingFlightId");

                    b.ToTable("WaitingFlights");
                });

            modelBuilder.Entity("Airport.API.Models.Leg", b =>
                {
                    b.HasOne("Airport.API.Models.Flight", "Flight")
                        .WithOne("Leg")
                        .HasForeignKey("Airport.API.Models.Leg", "FlightId");

                    b.Navigation("Flight");
                });

            modelBuilder.Entity("Airport.API.Models.LegConnection", b =>
                {
                    b.HasOne("Airport.API.Models.Leg", "Leg")
                        .WithMany("NextLegs")
                        .HasForeignKey("LegId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Airport.API.Models.Leg", "NextLeg")
                        .WithMany()
                        .HasForeignKey("NextLegId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Leg");

                    b.Navigation("NextLeg");
                });

            modelBuilder.Entity("Airport.API.Models.Flight", b =>
                {
                    b.Navigation("Leg");
                });

            modelBuilder.Entity("Airport.API.Models.Leg", b =>
                {
                    b.Navigation("NextLegs");
                });
#pragma warning restore 612, 618
        }
    }
}
