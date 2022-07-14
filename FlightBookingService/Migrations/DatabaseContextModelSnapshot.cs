﻿// <auto-generated />
using System;
using FlightBookingService.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FlightBookingService.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FlightBookingService.Database.BookingDetails", b =>
                {
                    b.Property<int>("BookingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AirlineName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BookingClass")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FlightId")
                        .HasColumnType("int");

                    b.Property<string>("Journeydate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastChangedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastChangedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NoOfSeats")
                        .HasColumnType("int");

                    b.Property<string>("OptForMeal")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PNR")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TicketStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("TotalPrice")
                        .HasColumnType("float");

                    b.HasKey("BookingId");

                    b.ToTable("BookingDetails");
                });

            modelBuilder.Entity("FlightBookingService.Database.DiscountDetails", b =>
                {
                    b.Property<int>("DiscountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("DiscountAmount")
                        .HasColumnType("float");

                    b.Property<string>("DiscountCode")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DiscountId");

                    b.ToTable("DiscountDetails");
                });

            modelBuilder.Entity("FlightBookingService.Database.FlightDetails", b =>
                {
                    b.Property<int>("SearchSeqNo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AirlineName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("BizClassTicketPrice")
                        .HasColumnType("float");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("DepartureTime")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("FlightId")
                        .HasColumnType("bigint");

                    b.Property<string>("FlightNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FlightStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FromPlace")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InstrumentUsed")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastChangedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastChangedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("NoOfBizClassSeats")
                        .HasColumnType("int");

                    b.Property<int>("NoOfNonBizClassSeats")
                        .HasColumnType("int");

                    b.Property<int>("NoOfRows")
                        .HasColumnType("int");

                    b.Property<double>("NonBizClassTicketPrice")
                        .HasColumnType("float");

                    b.Property<string>("OptedForMeal")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReachTime")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ScheduledDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ToPlace")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SearchSeqNo");

                    b.ToTable("FlightDetails");
                });

            modelBuilder.Entity("FlightBookingService.Database.PassengerList", b =>
                {
                    b.Property<int>("PassengerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<int>("BookingId")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastChangedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastChangedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SeatNo")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PassengerId");

                    b.HasIndex("BookingId");

                    b.ToTable("PassengerList");
                });

            modelBuilder.Entity("FlightBookingService.Database.PassengerList", b =>
                {
                    b.HasOne("FlightBookingService.Database.BookingDetails", "BookingDetails")
                        .WithMany("PassengerList")
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BookingDetails");
                });

            modelBuilder.Entity("FlightBookingService.Database.BookingDetails", b =>
                {
                    b.Navigation("PassengerList");
                });
#pragma warning restore 612, 618
        }
    }
}
