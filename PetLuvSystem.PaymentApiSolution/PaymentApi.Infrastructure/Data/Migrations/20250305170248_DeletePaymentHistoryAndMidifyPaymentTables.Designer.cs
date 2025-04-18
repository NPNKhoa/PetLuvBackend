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
    [Migration("20250305170248_DeletePaymentHistoryAndMidifyPaymentTables")]
    partial class DeletePaymentHistoryAndMidifyPaymentTables
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

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PaymentMethodId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PaymentStatusId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("TransactionRef")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("PaymentId");

                    b.HasIndex("PaymentMethodId");

                    b.HasIndex("PaymentStatusId");

                    b.ToTable("Payment");
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
                            PaymentMethodId = new Guid("3f599ae2-329c-4ea3-9a0f-17e923639181"),
                            IsVisible = true,
                            PaymentMethodName = "Thanh toán qua VNPay"
                        },
                        new
                        {
                            PaymentMethodId = new Guid("975225aa-2c71-4fe7-91c2-4038ee7abcd0"),
                            IsVisible = true,
                            PaymentMethodName = "Thanh toán tại cửa hàng"
                        },
                        new
                        {
                            PaymentMethodId = new Guid("357f52f1-c6c5-417e-af87-4dd869eb9aa3"),
                            IsVisible = true,
                            PaymentMethodName = "Thanh toán khi nhận hàng"
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
                            PaymentStatusId = new Guid("e03badbf-c5a3-4468-b84e-fa2f8bd3cc9e"),
                            IsVisible = true,
                            PaymentStatusName = "Chờ thanh toán"
                        },
                        new
                        {
                            PaymentStatusId = new Guid("d1f7edff-afdc-4b05-9c2f-3020b068eb7c"),
                            IsVisible = true,
                            PaymentStatusName = "Đã đặt cọc"
                        },
                        new
                        {
                            PaymentStatusId = new Guid("14cdd281-43ad-4c82-afdd-f73f70f6974a"),
                            IsVisible = true,
                            PaymentStatusName = "Đã thanh toán"
                        },
                        new
                        {
                            PaymentStatusId = new Guid("a89d9447-9c08-4c2c-8067-ee3cced8cf9b"),
                            IsVisible = true,
                            PaymentStatusName = "Thanh toán thất bại"
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

            modelBuilder.Entity("PaymentApi.Domain.Entities.PaymentMethod", b =>
                {
                    b.Navigation("Payments");
                });

            modelBuilder.Entity("PaymentApi.Domain.Entities.PaymentStatus", b =>
                {
                    b.Navigation("Payments");
                });
#pragma warning restore 612, 618
        }
    }
}
