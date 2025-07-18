﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RRMonitoring.Equipment.Infrastructure.Database;

#nullable disable

namespace RRMonitoring.Equipment.Infrastructure.Database.Migrations
{
    [DbContext(typeof(EquipmentContext))]
    partial class EquipmentContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("RRMonitoring.Equipment.Domain.Entities.Equipment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date");

                    b.Property<decimal>("CurrentHashrate")
                        .HasColumnType("numeric")
                        .HasColumnName("current_hashrate");

                    b.Property<decimal>("CurrentPower")
                        .HasColumnType("numeric")
                        .HasColumnName("current_power");

                    b.Property<string>("IpAddress")
                        .HasColumnType("text")
                        .HasColumnName("ip_address");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<string>("MacAddress")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("mac_address");

                    b.Property<byte>("ModeId")
                        .HasColumnType("smallint")
                        .HasColumnName("mode_id");

                    b.Property<Guid>("ModelId")
                        .HasColumnType("uuid")
                        .HasColumnName("model_id");

                    b.Property<string>("SerialNumber")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("serial_number");

                    b.Property<byte>("StatusId")
                        .HasColumnType("smallint")
                        .HasColumnName("status_id");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("updated_by");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_date");

                    b.HasKey("Id")
                        .HasName("pk_equipment");

                    b.HasIndex("IpAddress")
                        .IsUnique()
                        .HasDatabaseName("ix_equipment_ip_address");

                    b.HasIndex("ModeId")
                        .HasDatabaseName("ix_equipment_mode_id");

                    b.HasIndex("ModelId")
                        .HasDatabaseName("ix_equipment_model_id");

                    b.HasIndex("StatusId")
                        .HasDatabaseName("ix_equipment_status_id");

                    b.ToTable("equipment", (string)null);
                });

            modelBuilder.Entity("RRMonitoring.Equipment.Domain.Entities.EquipmentMode", b =>
                {
                    b.Property<byte>("Id")
                        .HasColumnType("smallint")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_equipment_mode");

                    b.ToTable("equipment_mode", (string)null);
                });

            modelBuilder.Entity("RRMonitoring.Equipment.Domain.Entities.EquipmentModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date");

                    b.Property<byte>("HashrateUnitId")
                        .HasColumnType("smallint")
                        .HasColumnName("hashrate_unit_id");

                    b.Property<Guid>("ManufacturerId")
                        .HasColumnType("uuid")
                        .HasColumnName("manufacturer_id");

                    b.Property<int>("MaxMotherBoardTemperature")
                        .HasColumnType("integer")
                        .HasColumnName("max_mother_board_temperature");

                    b.Property<int>("MaxProcessorTemperature")
                        .HasColumnType("integer")
                        .HasColumnName("max_processor_temperature");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<decimal>("NominalHashrate")
                        .HasColumnType("numeric")
                        .HasColumnName("nominal_hashrate");

                    b.Property<int>("NominalPower")
                        .HasColumnType("integer")
                        .HasColumnName("nominal_power");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("updated_by");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_date");

                    b.HasKey("Id")
                        .HasName("pk_equipment_model");

                    b.HasIndex("HashrateUnitId")
                        .HasDatabaseName("ix_equipment_model_hashrate_unit_id");

                    b.HasIndex("ManufacturerId")
                        .HasDatabaseName("ix_equipment_model_manufacturer_id");

                    b.ToTable("equipment_model", (string)null);
                });

            modelBuilder.Entity("RRMonitoring.Equipment.Domain.Entities.EquipmentModelCoin", b =>
                {
                    b.Property<Guid>("EquipmentModelId")
                        .HasColumnType("uuid")
                        .HasColumnName("equipment_model_id");

                    b.Property<byte>("CoinId")
                        .HasColumnType("smallint")
                        .HasColumnName("coin_id");

                    b.HasKey("EquipmentModelId", "CoinId")
                        .HasName("pk_equipment_model_coin");

                    b.ToTable("equipment_model_coin", (string)null);
                });

            modelBuilder.Entity("RRMonitoring.Equipment.Domain.Entities.EquipmentStatus", b =>
                {
                    b.Property<byte>("Id")
                        .HasColumnType("smallint")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_equipment_status");

                    b.ToTable("equipment_status", (string)null);
                });

            modelBuilder.Entity("RRMonitoring.Equipment.Domain.Entities.Firmware", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Comment")
                        .HasColumnType("text")
                        .HasColumnName("comment");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("OriginFileName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("origin_file_name");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("updated_by");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_date");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("version");

                    b.HasKey("Id")
                        .HasName("pk_firmware");

                    b.ToTable("firmware", (string)null);
                });

            modelBuilder.Entity("RRMonitoring.Equipment.Domain.Entities.FirmwareEquipmentModel", b =>
                {
                    b.Property<Guid>("FirmwareId")
                        .HasColumnType("uuid")
                        .HasColumnName("firmware_id");

                    b.Property<Guid>("EquipmentModelId")
                        .HasColumnType("uuid")
                        .HasColumnName("equipment_model_id");

                    b.HasKey("FirmwareId", "EquipmentModelId")
                        .HasName("pk_firmware_equipment_model");

                    b.HasIndex("EquipmentModelId")
                        .HasDatabaseName("ix_firmware_equipment_model_equipment_model_id");

                    b.ToTable("firmware_equipment_model", (string)null);
                });

            modelBuilder.Entity("RRMonitoring.Equipment.Domain.Entities.HashrateUnit", b =>
                {
                    b.Property<byte>("Id")
                        .HasColumnType("smallint")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_hashrate_unit");

                    b.ToTable("hashrate_unit", (string)null);

                    b.HasData(
                        new
                        {
                            Id = (byte)1,
                            Name = "H/s"
                        },
                        new
                        {
                            Id = (byte)2,
                            Name = "K/Sol"
                        },
                        new
                        {
                            Id = (byte)3,
                            Name = "MH/s"
                        },
                        new
                        {
                            Id = (byte)4,
                            Name = "GH/s"
                        },
                        new
                        {
                            Id = (byte)5,
                            Name = "TH/s"
                        },
                        new
                        {
                            Id = (byte)6,
                            Name = "PH/s"
                        });
                });

            modelBuilder.Entity("RRMonitoring.Equipment.Domain.Entities.Manufacturer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("updated_by");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_date");

                    b.HasKey("Id")
                        .HasName("pk_manufacturer");

                    b.ToTable("manufacturer", (string)null);
                });

            modelBuilder.Entity("RRMonitoring.Equipment.Domain.Entities.Scan", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date");

                    b.Property<string>("IpRangeDefinition")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("ip_range_definition");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<byte>("StatusId")
                        .HasColumnType("smallint")
                        .HasColumnName("status_id");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("updated_by");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_date");

                    b.HasKey("Id")
                        .HasName("pk_scan");

                    b.HasIndex("StatusId")
                        .HasDatabaseName("ix_scan_status_id");

                    b.ToTable("scan", (string)null);
                });

            modelBuilder.Entity("RRMonitoring.Equipment.Domain.Entities.ScanResult", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date");

                    b.Property<string>("DetectedModel")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("detected_model");

                    b.Property<Guid>("EquipmentId")
                        .HasColumnType("uuid")
                        .HasColumnName("equipment_id");

                    b.Property<string>("FirmwareVersion")
                        .HasColumnType("text")
                        .HasColumnName("firmware_version");

                    b.Property<string>("IpAddress")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("ip_address");

                    b.Property<bool>("IsNewEquipment")
                        .HasColumnType("boolean")
                        .HasColumnName("is_new_equipment");

                    b.Property<string>("MacAddress")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("mac_address");

                    b.Property<Guid>("ScanId")
                        .HasColumnType("uuid")
                        .HasColumnName("scan_id");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("updated_by");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_date");

                    b.HasKey("Id")
                        .HasName("pk_scan_result");

                    b.HasIndex("EquipmentId")
                        .HasDatabaseName("ix_scan_result_equipment_id");

                    b.HasIndex("ScanId")
                        .HasDatabaseName("ix_scan_result_scan_id");

                    b.ToTable("scan_result", (string)null);
                });

            modelBuilder.Entity("RRMonitoring.Equipment.Domain.Entities.ScanStatus", b =>
                {
                    b.Property<byte>("Id")
                        .HasColumnType("smallint")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_scan_status");

                    b.ToTable("scan_status", (string)null);
                });

            modelBuilder.Entity("RRMonitoring.Equipment.Domain.Entities.Equipment", b =>
                {
                    b.HasOne("RRMonitoring.Equipment.Domain.Entities.EquipmentMode", "Mode")
                        .WithMany()
                        .HasForeignKey("ModeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_equipment_equipment_mode_mode_id");

                    b.HasOne("RRMonitoring.Equipment.Domain.Entities.EquipmentModel", "Model")
                        .WithMany("Equipment")
                        .HasForeignKey("ModelId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_equipment_equipment_model_model_id");

                    b.HasOne("RRMonitoring.Equipment.Domain.Entities.EquipmentStatus", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_equipment_equipment_status_status_id");

                    b.Navigation("Mode");

                    b.Navigation("Model");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("RRMonitoring.Equipment.Domain.Entities.EquipmentModel", b =>
                {
                    b.HasOne("RRMonitoring.Equipment.Domain.Entities.HashrateUnit", "HashrateUnit")
                        .WithMany()
                        .HasForeignKey("HashrateUnitId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_equipment_model_hashrate_unit_hashrate_unit_id");

                    b.HasOne("RRMonitoring.Equipment.Domain.Entities.Manufacturer", "Manufacturer")
                        .WithMany("EquipmentModels")
                        .HasForeignKey("ManufacturerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_equipment_model_manufacturer_manufacturer_id");

                    b.Navigation("HashrateUnit");

                    b.Navigation("Manufacturer");
                });

            modelBuilder.Entity("RRMonitoring.Equipment.Domain.Entities.EquipmentModelCoin", b =>
                {
                    b.HasOne("RRMonitoring.Equipment.Domain.Entities.EquipmentModel", "EquipmentModel")
                        .WithMany("EquipmentModelCoins")
                        .HasForeignKey("EquipmentModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_equipment_model_coin_equipment_model_equipment_model_id");

                    b.Navigation("EquipmentModel");
                });

            modelBuilder.Entity("RRMonitoring.Equipment.Domain.Entities.FirmwareEquipmentModel", b =>
                {
                    b.HasOne("RRMonitoring.Equipment.Domain.Entities.EquipmentModel", "EquipmentModel")
                        .WithMany("FirmwareEquipmentModels")
                        .HasForeignKey("EquipmentModelId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_firmware_equipment_model_equipment_model_equipment_model_id");

                    b.HasOne("RRMonitoring.Equipment.Domain.Entities.Firmware", "Firmware")
                        .WithMany("FirmwareEquipmentModels")
                        .HasForeignKey("FirmwareId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_firmware_equipment_model_firmware_firmware_id");

                    b.Navigation("EquipmentModel");

                    b.Navigation("Firmware");
                });

            modelBuilder.Entity("RRMonitoring.Equipment.Domain.Entities.Scan", b =>
                {
                    b.HasOne("RRMonitoring.Equipment.Domain.Entities.ScanStatus", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_scan_scan_status_status_id");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("RRMonitoring.Equipment.Domain.Entities.ScanResult", b =>
                {
                    b.HasOne("RRMonitoring.Equipment.Domain.Entities.Equipment", "Equipment")
                        .WithMany("ScanResults")
                        .HasForeignKey("EquipmentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_scan_result_equipment_equipment_id");

                    b.HasOne("RRMonitoring.Equipment.Domain.Entities.Scan", "Scan")
                        .WithMany("ScanResults")
                        .HasForeignKey("ScanId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_scan_result_scan_scan_id");

                    b.Navigation("Equipment");

                    b.Navigation("Scan");
                });

            modelBuilder.Entity("RRMonitoring.Equipment.Domain.Entities.Equipment", b =>
                {
                    b.Navigation("ScanResults");
                });

            modelBuilder.Entity("RRMonitoring.Equipment.Domain.Entities.EquipmentModel", b =>
                {
                    b.Navigation("Equipment");

                    b.Navigation("EquipmentModelCoins");

                    b.Navigation("FirmwareEquipmentModels");
                });

            modelBuilder.Entity("RRMonitoring.Equipment.Domain.Entities.Firmware", b =>
                {
                    b.Navigation("FirmwareEquipmentModels");
                });

            modelBuilder.Entity("RRMonitoring.Equipment.Domain.Entities.Manufacturer", b =>
                {
                    b.Navigation("EquipmentModels");
                });

            modelBuilder.Entity("RRMonitoring.Equipment.Domain.Entities.Scan", b =>
                {
                    b.Navigation("ScanResults");
                });
#pragma warning restore 612, 618
        }
    }
}
