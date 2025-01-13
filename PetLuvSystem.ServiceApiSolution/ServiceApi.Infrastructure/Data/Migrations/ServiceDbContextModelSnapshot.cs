﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ServiceApi.Infrastructure.Data;

#nullable disable

namespace ServiceApi.Infrastructure.Data.Migrations
{
    [DbContext(typeof(ServiceDbContext))]
    partial class ServiceDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ServiceApi.Domain.Entities.Service", b =>
                {
                    b.Property<Guid>("ServiceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.Property<string>("ServiceDesc")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("ServiceName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<Guid>("ServiceTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ServiceId");

                    b.HasIndex("ServiceTypeId");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("ServiceApi.Domain.Entities.ServiceCombo", b =>
                {
                    b.Property<Guid>("ServiceComboId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.Property<string>("ServiceComboDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ServiceComboName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ServiceComboId");

                    b.ToTable("ServiceCombos");
                });

            modelBuilder.Entity("ServiceApi.Domain.Entities.ServiceComboMapping", b =>
                {
                    b.Property<Guid>("ServiceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ServiceComboId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ServiceId", "ServiceComboId");

                    b.HasIndex("ServiceComboId");

                    b.ToTable("ServiceComboMappings");
                });

            modelBuilder.Entity("ServiceApi.Domain.Entities.ServiceComboVariant", b =>
                {
                    b.Property<Guid>("ServiceComboId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BreedId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("WeightRange")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("ComboPrice")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ServiceComboId", "BreedId", "WeightRange");

                    b.ToTable("ServiceComboPrices");
                });

            modelBuilder.Entity("ServiceApi.Domain.Entities.ServiceImage", b =>
                {
                    b.Property<string>("ServiceImagePath")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("ServiceId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ServiceImagePath");

                    b.HasIndex("ServiceId");

                    b.ToTable("ServiceImages");
                });

            modelBuilder.Entity("ServiceApi.Domain.Entities.ServiceType", b =>
                {
                    b.Property<Guid>("ServiceTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.Property<string>("ServiceTypeDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ServiceTypeName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ServiceTypeId");

                    b.ToTable("ServiceTypes");

                    b.HasData(
                        new
                        {
                            ServiceTypeId = new Guid("8fceba7d-2595-4cc3-b036-5062588c7591"),
                            IsVisible = false,
                            ServiceTypeDesc = "Cung cấp các dịch vụ spa cho thú cưng trực tiếp tại cửa hàng",
                            ServiceTypeName = "Dịch vụ spa"
                        },
                        new
                        {
                            ServiceTypeId = new Guid("cc4de076-eaca-4d91-90fe-c14727a2513a"),
                            IsVisible = false,
                            ServiceTypeDesc = "Chúng tôi sẵn sàng trực tiếp đến nhà chăm sóc cho \"boss\" của bạn nếu bạn đang quá bận",
                            ServiceTypeName = "Dịch vụ spa tại nhà"
                        },
                        new
                        {
                            ServiceTypeId = new Guid("b8f5c2d2-b651-4341-870b-a6acc7c5187a"),
                            IsVisible = false,
                            ServiceTypeDesc = "Nếu bạn đang quá bận để giúp cún cưng của mình giải tỏa năng lượng, hãy để chúng tôi giúp bạn làm điều đó",
                            ServiceTypeName = "Dịch vụ dắt chó đi dạo"
                        });
                });

            modelBuilder.Entity("ServiceApi.Domain.Entities.ServiceVariant", b =>
                {
                    b.Property<Guid>("ServiceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BreedId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PetWeightRange")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("Price")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ServiceId", "BreedId", "PetWeightRange");

                    b.ToTable("ServicePrices");
                });

            modelBuilder.Entity("ServiceApi.Domain.Entities.WalkDogServiceVariant", b =>
                {
                    b.Property<Guid>("ServiceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("PricePerPeriod")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ServiceId");

                    b.ToTable("WalkDogServicePrices");
                });

            modelBuilder.Entity("ServiceApi.Domain.Entities.Service", b =>
                {
                    b.HasOne("ServiceApi.Domain.Entities.ServiceType", "ServiceType")
                        .WithMany("Services")
                        .HasForeignKey("ServiceTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ServiceType");
                });

            modelBuilder.Entity("ServiceApi.Domain.Entities.ServiceComboMapping", b =>
                {
                    b.HasOne("ServiceApi.Domain.Entities.ServiceCombo", "ServiceCombo")
                        .WithMany("ServiceComboMappings")
                        .HasForeignKey("ServiceComboId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ServiceApi.Domain.Entities.Service", "Service")
                        .WithMany("ServiceComboMappings")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Service");

                    b.Navigation("ServiceCombo");
                });

            modelBuilder.Entity("ServiceApi.Domain.Entities.ServiceComboVariant", b =>
                {
                    b.HasOne("ServiceApi.Domain.Entities.ServiceCombo", "ServiceCombo")
                        .WithMany("ServiceComboVariants")
                        .HasForeignKey("ServiceComboId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ServiceCombo");
                });

            modelBuilder.Entity("ServiceApi.Domain.Entities.ServiceImage", b =>
                {
                    b.HasOne("ServiceApi.Domain.Entities.Service", "Service")
                        .WithMany("ServiceImages")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Service");
                });

            modelBuilder.Entity("ServiceApi.Domain.Entities.ServiceVariant", b =>
                {
                    b.HasOne("ServiceApi.Domain.Entities.Service", "Service")
                        .WithMany("ServiceVariants")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Service");
                });

            modelBuilder.Entity("ServiceApi.Domain.Entities.WalkDogServiceVariant", b =>
                {
                    b.HasOne("ServiceApi.Domain.Entities.Service", "Service")
                        .WithMany("WalkDogServiceVariants")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Service");
                });

            modelBuilder.Entity("ServiceApi.Domain.Entities.Service", b =>
                {
                    b.Navigation("ServiceComboMappings");

                    b.Navigation("ServiceImages");

                    b.Navigation("ServiceVariants");

                    b.Navigation("WalkDogServiceVariants");
                });

            modelBuilder.Entity("ServiceApi.Domain.Entities.ServiceCombo", b =>
                {
                    b.Navigation("ServiceComboMappings");

                    b.Navigation("ServiceComboVariants");
                });

            modelBuilder.Entity("ServiceApi.Domain.Entities.ServiceType", b =>
                {
                    b.Navigation("Services");
                });
#pragma warning restore 612, 618
        }
    }
}
