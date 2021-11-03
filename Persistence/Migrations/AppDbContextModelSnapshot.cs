﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Persistence;

namespace Persistence.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.10");

            modelBuilder.Entity("Domain.Entities.Contractor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("Created")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Contractors");
                });

            modelBuilder.Entity("Domain.Entities.MeasurementBookAggregate.MBItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("Created")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("TEXT");

                    b.Property<float>("CummulativeQuantity")
                        .HasColumnType("REAL");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("TEXT");

                    b.Property<int?>("MeasurementBookId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("No")
                        .HasColumnType("INTEGER");

                    b.Property<float>("PoQuantity")
                        .HasColumnType("REAL");

                    b.Property<double>("Rate")
                        .HasColumnType("REAL");

                    b.Property<int>("UomId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("MeasurementBookId");

                    b.HasIndex("UomId");

                    b.ToTable("MBItem");
                });

            modelBuilder.Entity("Domain.Entities.MeasurementBookAggregate.MeasurementBook", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("Created")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("TEXT");

                    b.Property<string>("MeasurementOfficer")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ValidatingOfficer")
                        .HasColumnType("TEXT");

                    b.Property<int>("WorkOrderId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("WorkOrderId");

                    b.ToTable("MeasurementBooks");
                });

            modelBuilder.Entity("Domain.Entities.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("Created")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("Domain.Entities.Uom", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("Created")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("TEXT");

                    b.Property<int>("Dimension")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Uoms");
                });

            modelBuilder.Entity("Domain.Entities.WorkOrderAggregate.WorkOrder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("AgreementDate")
                        .HasColumnType("INTEGER");

                    b.Property<string>("AgreementNo")
                        .HasColumnType("TEXT");

                    b.Property<int>("ContractorId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("Created")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("TEXT");

                    b.Property<string>("EngineerInCharge")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("TEXT");

                    b.Property<long>("OrderDate")
                        .HasColumnType("INTEGER");

                    b.Property<string>("OrderNo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ProjectId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ContractorId");

                    b.HasIndex("ProjectId");

                    b.ToTable("WorkOrders");
                });

            modelBuilder.Entity("Domain.Entities.WorkOrderAggregate.WorkOrderItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("Created")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ItemNo")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("TEXT");

                    b.Property<float>("PoQuantity")
                        .HasColumnType("REAL");

                    b.Property<double>("UnitRate")
                        .HasColumnType("REAL");

                    b.Property<int>("UomId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("WorkOrderId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UomId");

                    b.HasIndex("WorkOrderId");

                    b.ToTable("WorkOrderItem");
                });

            modelBuilder.Entity("Domain.Entities.MeasurementBookAggregate.MBItem", b =>
                {
                    b.HasOne("Domain.Entities.MeasurementBookAggregate.MeasurementBook", null)
                        .WithMany("LineItems")
                        .HasForeignKey("MeasurementBookId");

                    b.HasOne("Domain.Entities.Uom", "Uom")
                        .WithMany()
                        .HasForeignKey("UomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Uom");
                });

            modelBuilder.Entity("Domain.Entities.MeasurementBookAggregate.MeasurementBook", b =>
                {
                    b.HasOne("Domain.Entities.WorkOrderAggregate.WorkOrder", "WorkOrder")
                        .WithMany("MeasurementBooks")
                        .HasForeignKey("WorkOrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WorkOrder");
                });

            modelBuilder.Entity("Domain.Entities.WorkOrderAggregate.WorkOrder", b =>
                {
                    b.HasOne("Domain.Entities.Contractor", "Contractor")
                        .WithMany()
                        .HasForeignKey("ContractorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Contractor");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("Domain.Entities.WorkOrderAggregate.WorkOrderItem", b =>
                {
                    b.HasOne("Domain.Entities.Uom", "Uom")
                        .WithMany()
                        .HasForeignKey("UomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.WorkOrderAggregate.WorkOrder", null)
                        .WithMany("Items")
                        .HasForeignKey("WorkOrderId");

                    b.Navigation("Uom");
                });

            modelBuilder.Entity("Domain.Entities.MeasurementBookAggregate.MeasurementBook", b =>
                {
                    b.Navigation("LineItems");
                });

            modelBuilder.Entity("Domain.Entities.WorkOrderAggregate.WorkOrder", b =>
                {
                    b.Navigation("Items");

                    b.Navigation("MeasurementBooks");
                });
#pragma warning restore 612, 618
        }
    }
}
