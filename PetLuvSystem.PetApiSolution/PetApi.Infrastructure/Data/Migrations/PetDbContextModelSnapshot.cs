﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PetApi.Infrastructure.Data;

#nullable disable

namespace PetApi.Infrastructure.Data.Migrations
{
    [DbContext(typeof(PetDbContext))]
    partial class PetDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PetApi.Domain.Entities.Pet", b =>
                {
                    b.Property<Guid>("PetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BreedId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("nvarchar(13)");

                    b.Property<Guid?>("FatherId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.Property<Guid?>("MotherId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("PetDateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("PetDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PetFamilyRole")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PetFurColor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PetGender")
                        .HasColumnType("bit");

                    b.Property<string>("PetName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("PetWeight")
                        .HasColumnType("float");

                    b.HasKey("PetId");

                    b.HasIndex("BreedId");

                    b.HasIndex("FatherId");

                    b.HasIndex("MotherId");

                    b.ToTable("Pets");

                    b.HasDiscriminator().HasValue("Pet");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("PetApi.Domain.Entities.PetBreed", b =>
                {
                    b.Property<Guid>("BreedId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BreedDesc")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BreedName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IllustrationImage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.Property<Guid>("PetTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("BreedId");

                    b.HasIndex("PetTypeId");

                    b.ToTable("PetBreeds");
                });

            modelBuilder.Entity("PetApi.Domain.Entities.PetHealthBook", b =>
                {
                    b.Property<Guid>("PetHealthBookId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("PetHealthBookId");

                    b.ToTable("PetHealthBooks");
                });

            modelBuilder.Entity("PetApi.Domain.Entities.PetHealthBookDetail", b =>
                {
                    b.Property<Guid>("HealthBookDetailId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("HealthBookId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PetHealthNote")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TreatmentDesc")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TreatmentName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TreatmentProof")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("VetDegree")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VetName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("HealthBookDetailId", "HealthBookId");

                    b.HasIndex("HealthBookId");

                    b.ToTable("PetHealthBookDetails");
                });

            modelBuilder.Entity("PetApi.Domain.Entities.PetImage", b =>
                {
                    b.Property<string>("PetImagePath")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("PetId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("PetImagePath");

                    b.HasIndex("PetId");

                    b.ToTable("PetImages");
                });

            modelBuilder.Entity("PetApi.Domain.Entities.PetType", b =>
                {
                    b.Property<Guid>("PetTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.Property<string>("PetTypeDesc")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PetTypeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PetTypeId");

                    b.ToTable("PetTypes");
                });

            modelBuilder.Entity("PetApi.Domain.Entities.SellingPet", b =>
                {
                    b.HasBaseType("PetApi.Domain.Entities.Pet");

                    b.Property<decimal>("SellingPrice")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.HasDiscriminator().HasValue("SellingPet");
                });

            modelBuilder.Entity("PetApi.Domain.Entities.Pet", b =>
                {
                    b.HasOne("PetApi.Domain.Entities.PetBreed", "PetBreed")
                        .WithMany("Pets")
                        .HasForeignKey("BreedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PetApi.Domain.Entities.Pet", "Father")
                        .WithMany("ChildrenFromFather")
                        .HasForeignKey("FatherId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("PetApi.Domain.Entities.Pet", "Mother")
                        .WithMany("ChildrenFromMother")
                        .HasForeignKey("MotherId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Father");

                    b.Navigation("Mother");

                    b.Navigation("PetBreed");
                });

            modelBuilder.Entity("PetApi.Domain.Entities.PetBreed", b =>
                {
                    b.HasOne("PetApi.Domain.Entities.PetType", "PetType")
                        .WithMany("PetBreeds")
                        .HasForeignKey("PetTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PetType");
                });

            modelBuilder.Entity("PetApi.Domain.Entities.PetHealthBook", b =>
                {
                    b.HasOne("PetApi.Domain.Entities.Pet", "Pet")
                        .WithOne("PetHealthBook")
                        .HasForeignKey("PetApi.Domain.Entities.PetHealthBook", "PetHealthBookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pet");
                });

            modelBuilder.Entity("PetApi.Domain.Entities.PetHealthBookDetail", b =>
                {
                    b.HasOne("PetApi.Domain.Entities.PetHealthBook", "PetHealthBook")
                        .WithMany("PetHealthBookDetails")
                        .HasForeignKey("HealthBookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PetHealthBook");
                });

            modelBuilder.Entity("PetApi.Domain.Entities.PetImage", b =>
                {
                    b.HasOne("PetApi.Domain.Entities.Pet", null)
                        .WithMany("PetImagePaths")
                        .HasForeignKey("PetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PetApi.Domain.Entities.Pet", b =>
                {
                    b.Navigation("ChildrenFromFather");

                    b.Navigation("ChildrenFromMother");

                    b.Navigation("PetHealthBook")
                        .IsRequired();

                    b.Navigation("PetImagePaths");
                });

            modelBuilder.Entity("PetApi.Domain.Entities.PetBreed", b =>
                {
                    b.Navigation("Pets");
                });

            modelBuilder.Entity("PetApi.Domain.Entities.PetHealthBook", b =>
                {
                    b.Navigation("PetHealthBookDetails");
                });

            modelBuilder.Entity("PetApi.Domain.Entities.PetType", b =>
                {
                    b.Navigation("PetBreeds");
                });
#pragma warning restore 612, 618
        }
    }
}
