﻿// <auto-generated />
using System;
using BookingApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BookingApi.Infrastructure.Data.Migrations
{
    [DbContext(typeof(BookingDbContext))]
    [Migration("20250228163603_AddRoomRentalTime")]
    partial class AddRoomRentalTime
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BookingApi.Domain.Entities.Booking", b =>
                {
                    b.Property<Guid>("BookingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("BookingEndTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("BookingNote")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("BookingStartTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("BookingStatusId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BookingTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("DepositAmount")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("PaymentStatusId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PetId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("RoomRentalTime")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalAmount")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("TotalEstimateTime")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BookingId");

                    b.HasIndex("BookingStatusId");

                    b.HasIndex("BookingTypeId");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("BookingApi.Domain.Entities.BookingStatus", b =>
                {
                    b.Property<Guid>("BookingStatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BookingStatusName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.HasKey("BookingStatusId");

                    b.ToTable("BookingStatuses");
                });

            modelBuilder.Entity("BookingApi.Domain.Entities.BookingType", b =>
                {
                    b.Property<Guid>("BookingTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BookingTypeDesc")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BookingTypeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.HasKey("BookingTypeId");

                    b.ToTable("BookingTypes");
                });

            modelBuilder.Entity("BookingApi.Domain.Entities.RoomBookingItem", b =>
                {
                    b.Property<Guid>("BookingId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("ItemPrice")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("BookingId", "RoomId");

                    b.HasIndex("BookingId")
                        .IsUnique();

                    b.ToTable("RoomBookingItems");
                });

            modelBuilder.Entity("BookingApi.Domain.Entities.ServiceBookingDetail", b =>
                {
                    b.Property<Guid>("BookingId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ServiceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BreedId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PetWeightRange")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("BookingItemPrice")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ServiceItemName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BookingId", "ServiceId", "BreedId", "PetWeightRange");

                    b.ToTable("ServiceBookingDetails");
                });

            modelBuilder.Entity("BookingApi.Domain.Entities.ServiceComboBookingDetail", b =>
                {
                    b.Property<Guid>("BookingId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ServiceComboId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("BookingItemPrice")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("BreedId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PetWeightRange")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ServiceComboItemName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BookingId", "ServiceComboId");

                    b.ToTable("ServiceComboBookingDetails");
                });

            modelBuilder.Entity("BookingApi.Domain.Entities.Booking", b =>
                {
                    b.HasOne("BookingApi.Domain.Entities.BookingStatus", "BookingStatus")
                        .WithMany("Bookings")
                        .HasForeignKey("BookingStatusId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("BookingApi.Domain.Entities.BookingType", "BookingType")
                        .WithMany("Bookings")
                        .HasForeignKey("BookingTypeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("BookingStatus");

                    b.Navigation("BookingType");
                });

            modelBuilder.Entity("BookingApi.Domain.Entities.RoomBookingItem", b =>
                {
                    b.HasOne("BookingApi.Domain.Entities.Booking", "Booking")
                        .WithOne("RoomBookingItem")
                        .HasForeignKey("BookingApi.Domain.Entities.RoomBookingItem", "BookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Booking");
                });

            modelBuilder.Entity("BookingApi.Domain.Entities.ServiceBookingDetail", b =>
                {
                    b.HasOne("BookingApi.Domain.Entities.Booking", "Booking")
                        .WithMany("ServiceBookingDetails")
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Booking");
                });

            modelBuilder.Entity("BookingApi.Domain.Entities.ServiceComboBookingDetail", b =>
                {
                    b.HasOne("BookingApi.Domain.Entities.Booking", "Booking")
                        .WithMany("ServiceComboBookingDetails")
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Booking");
                });

            modelBuilder.Entity("BookingApi.Domain.Entities.Booking", b =>
                {
                    b.Navigation("RoomBookingItem");

                    b.Navigation("ServiceBookingDetails");

                    b.Navigation("ServiceComboBookingDetails");
                });

            modelBuilder.Entity("BookingApi.Domain.Entities.BookingStatus", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("BookingApi.Domain.Entities.BookingType", b =>
                {
                    b.Navigation("Bookings");
                });
#pragma warning restore 612, 618
        }
    }
}
