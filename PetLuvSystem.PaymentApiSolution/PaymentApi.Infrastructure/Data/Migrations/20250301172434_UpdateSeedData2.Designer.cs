﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PaymentApi.Infrastructure.Data;

#nullable disable

namespace PaymentApi.Infrastructure.Data.Migrations
{
    [DbContext(typeof(PaymentDbContext))]
    [Migration("20250301172434_UpdateSeedData2")]
    partial class UpdateSeedData2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PaymentApi.Domain.Entities.Payment", b =>
                {
                    b.Property<Guid>("PaymentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PaymentMethodId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PaymentStatusId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ResponseData")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TransactionRef")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("PaymentId");

                    b.HasIndex("PaymentMethodId");

                    b.HasIndex("PaymentStatusId");

                    b.ToTable("Payment");
                });

            modelBuilder.Entity("PaymentApi.Domain.Entities.PaymentHistory", b =>
                {
                    b.Property<Guid>("PaymentHistoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PaymentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PaymentStatusId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("PaymentHistoryId");

                    b.HasIndex("PaymentId");

                    b.HasIndex("PaymentStatusId");

                    b.ToTable("PaymentHistory");
                });

            modelBuilder.Entity("PaymentApi.Domain.Entities.PaymentMethod", b =>
                {
                    b.Property<Guid>("PaymentMethodId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.Property<string>("PaymentMethodName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PaymentMethodId");

                    b.ToTable("PaymentMethod");

                    b.HasData(
                        new
                        {
                            PaymentMethodId = new Guid("dc35c302-fd44-43e4-a007-b16e8a6234ef"),
                            IsVisible = true,
                            PaymentMethodName = "Thanh toán qua VNPay"
                        });
                });

            modelBuilder.Entity("PaymentApi.Domain.Entities.PaymentStatus", b =>
                {
                    b.Property<Guid>("PaymentStatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.Property<string>("PaymentStatusName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PaymentStatusId");

                    b.ToTable("PaymentStatus");

                    b.HasData(
                        new
                        {
                            PaymentStatusId = new Guid("87fb356a-980c-49bd-b21e-539ddc0746fd"),
                            IsVisible = true,
                            PaymentStatusName = "Chờ thanh toán"
                        });
                });

            modelBuilder.Entity("PaymentApi.Domain.Entities.Payment", b =>
                {
                    b.HasOne("PaymentApi.Domain.Entities.PaymentMethod", "PaymentMethod")
                        .WithMany("Payments")
                        .HasForeignKey("PaymentMethodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PaymentApi.Domain.Entities.PaymentStatus", "PaymentStatus")
                        .WithMany("Payments")
                        .HasForeignKey("PaymentStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PaymentMethod");

                    b.Navigation("PaymentStatus");
                });

            modelBuilder.Entity("PaymentApi.Domain.Entities.PaymentHistory", b =>
                {
                    b.HasOne("PaymentApi.Domain.Entities.Payment", "Payment")
                        .WithMany("PaymentHistories")
                        .HasForeignKey("PaymentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("PaymentApi.Domain.Entities.PaymentStatus", "PaymentStatus")
                        .WithMany("PaymentHistories")
                        .HasForeignKey("PaymentStatusId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Payment");

                    b.Navigation("PaymentStatus");
                });

            modelBuilder.Entity("PaymentApi.Domain.Entities.Payment", b =>
                {
                    b.Navigation("PaymentHistories");
                });

            modelBuilder.Entity("PaymentApi.Domain.Entities.PaymentMethod", b =>
                {
                    b.Navigation("Payments");
                });

            modelBuilder.Entity("PaymentApi.Domain.Entities.PaymentStatus", b =>
                {
                    b.Navigation("PaymentHistories");

                    b.Navigation("Payments");
                });
#pragma warning restore 612, 618
        }
    }
}
