﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RRMonitoring.Mining.Infrastructure.Database;

#nullable disable

namespace RRMonitoring.Mining.Infrastructure.Database.Migrations
{
    [DbContext(typeof(MiningContext))]
    [Migration("20250516182455_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("RRMonitoring.Mining.Domain.Entities.Client", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date");

                    b.Property<string>("ExternalId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("external_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_date");

                    b.HasKey("Id")
                        .HasName("pk_client");

                    b.HasIndex("ExternalId")
                        .IsUnique()
                        .HasDatabaseName("ix_client_external_id");

                    b.ToTable("client", (string)null);
                });

            modelBuilder.Entity("RRMonitoring.Mining.Domain.Entities.Coin", b =>
                {
                    b.Property<byte>("Id")
                        .HasColumnType("smallint")
                        .HasColumnName("id");

                    b.Property<string>("ExternalId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("external_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Ticker")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("ticker");

                    b.HasKey("Id")
                        .HasName("pk_coin");

                    b.HasIndex("ExternalId")
                        .IsUnique()
                        .HasDatabaseName("ix_coin_external_id");

                    b.HasIndex("Ticker")
                        .IsUnique()
                        .HasDatabaseName("ix_coin_ticker");

                    b.ToTable("coin", (string)null);

                    b.HasData(
                        new
                        {
                            Id = (byte)1,
                            ExternalId = "89bb611e-7a3c-4adf-a190-89ceaaa6b174",
                            Name = "Conflux",
                            Ticker = "CFX"
                        },
                        new
                        {
                            Id = (byte)2,
                            ExternalId = "728912a7-1a00-4abb-96c0-fba5d4123957",
                            Name = "EthereumPoW",
                            Ticker = "ETHW"
                        },
                        new
                        {
                            Id = (byte)3,
                            ExternalId = "049a7e90-f063-4532-acd7-6fc74ec1ec64",
                            Name = "EthereumFair",
                            Ticker = "ETHF"
                        },
                        new
                        {
                            Id = (byte)4,
                            ExternalId = "7f08ec67-64f4-40ce-b18f-3a659d6614a8",
                            Name = "Nervos Network",
                            Ticker = "CKB"
                        },
                        new
                        {
                            Id = (byte)5,
                            ExternalId = "7d760102-9094-4829-a65a-ba749bddf013",
                            Name = "Ergo",
                            Ticker = "ERG"
                        },
                        new
                        {
                            Id = (byte)6,
                            ExternalId = "2bfc15a8-a452-4c03-9f7a-80b8ed853415",
                            Name = "Handshake",
                            Ticker = "HNS"
                        },
                        new
                        {
                            Id = (byte)7,
                            ExternalId = "975bc446-5f0e-4349-b00a-4a1d5f7b51a3",
                            Name = "Qitmeer Network",
                            Ticker = "MEER"
                        },
                        new
                        {
                            Id = (byte)8,
                            ExternalId = "c440283b-04c4-4f7b-b622-debe5b867c95",
                            Name = "eCash",
                            Ticker = "XEC"
                        },
                        new
                        {
                            Id = (byte)9,
                            ExternalId = "f28930db-d005-43b3-8ffd-5b62b15785df",
                            Name = "Bitcoin",
                            Ticker = "BTC"
                        },
                        new
                        {
                            Id = (byte)10,
                            ExternalId = "223e7ec7-c749-4909-b56f-cf2bee5aaa85",
                            Name = "Ethereum Classic",
                            Ticker = "ETC"
                        },
                        new
                        {
                            Id = (byte)11,
                            ExternalId = "86961652-cb82-4042-b3ae-05131494535b",
                            Name = "Kaspa",
                            Ticker = "KAS"
                        },
                        new
                        {
                            Id = (byte)12,
                            ExternalId = "1326ccaa-5082-4304-9270-0ce4e81fb57d",
                            Name = "Dash",
                            Ticker = "DASH"
                        },
                        new
                        {
                            Id = (byte)13,
                            ExternalId = "d175f805-b52f-4b93-b568-174bc6fca57d",
                            Name = "Litecoin",
                            Ticker = "LTC"
                        },
                        new
                        {
                            Id = (byte)14,
                            ExternalId = "b5bd1ee1-748e-481f-b4bf-f621989113db",
                            Name = "Zcash",
                            Ticker = "ZEC"
                        },
                        new
                        {
                            Id = (byte)15,
                            ExternalId = "3c96d353-adf6-4690-8f93-f85aa173441e",
                            Name = "Horizen",
                            Ticker = "ZEN"
                        },
                        new
                        {
                            Id = (byte)16,
                            ExternalId = "8f80498f-54ef-4b9d-957a-3b6ba4a26df3",
                            Name = "Aleo",
                            Ticker = "ALEO"
                        },
                        new
                        {
                            Id = (byte)17,
                            ExternalId = "cff35eb9-ed0c-4c5f-8c3f-68454bba3343",
                            Name = "Ravencoin",
                            Ticker = "RVN"
                        },
                        new
                        {
                            Id = (byte)18,
                            ExternalId = "25f59887-83d4-463b-8ec3-18a42abad96a",
                            Name = "Kadena",
                            Ticker = "KDA"
                        },
                        new
                        {
                            Id = (byte)19,
                            ExternalId = "5651424e-5055-4989-bfc3-062e717d3bff",
                            Name = "ScPrime",
                            Ticker = "SCP"
                        });
                });

            modelBuilder.Entity("RRMonitoring.Mining.Domain.Entities.Pool", b =>
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

                    b.Property<string>("ExternalId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("external_id");

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
                        .HasName("pk_pool");

                    b.HasIndex("ExternalId")
                        .IsUnique()
                        .HasDatabaseName("ix_pool_external_id");

                    b.ToTable("pool", (string)null);
                });

            modelBuilder.Entity("RRMonitoring.Mining.Domain.Entities.PoolCoinAddress", b =>
                {
                    b.Property<Guid>("PoolId")
                        .HasColumnType("uuid")
                        .HasColumnName("pool_id");

                    b.Property<byte>("CoinId")
                        .HasColumnType("smallint")
                        .HasColumnName("coin_id");

                    b.Property<string>("FirstAddress")
                        .HasColumnType("text")
                        .HasColumnName("first_address");

                    b.Property<string>("SecondAddress")
                        .HasColumnType("text")
                        .HasColumnName("second_address");

                    b.Property<string>("ThirdAddress")
                        .HasColumnType("text")
                        .HasColumnName("third_address");

                    b.HasKey("PoolId", "CoinId")
                        .HasName("pk_pool_coin_address");

                    b.HasIndex("CoinId")
                        .HasDatabaseName("ix_pool_coin_address_coin_id");

                    b.ToTable("pool_coin_address", (string)null);
                });

            modelBuilder.Entity("RRMonitoring.Mining.Domain.Entities.Worker", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid?>("ClientId")
                        .HasColumnType("uuid")
                        .HasColumnName("client_id");

                    b.Property<byte>("CoinId")
                        .HasColumnType("smallint")
                        .HasColumnName("coin_id");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("display_name");

                    b.Property<string>("ExternalId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("external_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("ObserverLink")
                        .HasColumnType("text")
                        .HasColumnName("observer_link");

                    b.Property<Guid>("PoolId")
                        .HasColumnType("uuid")
                        .HasColumnName("pool_id");

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
                        .HasName("pk_worker");

                    b.HasIndex("ClientId")
                        .HasDatabaseName("ix_worker_client_id");

                    b.HasIndex("CoinId")
                        .HasDatabaseName("ix_worker_coin_id");

                    b.HasIndex("ExternalId")
                        .IsUnique()
                        .HasDatabaseName("ix_worker_external_id");

                    b.HasIndex("PoolId")
                        .HasDatabaseName("ix_worker_pool_id");

                    b.HasIndex("StatusId")
                        .HasDatabaseName("ix_worker_status_id");

                    b.HasIndex("Name", "PoolId", "CoinId")
                        .IsUnique()
                        .HasDatabaseName("ix_worker_name_pool_id_coin_id");

                    b.ToTable("worker", (string)null);
                });

            modelBuilder.Entity("RRMonitoring.Mining.Domain.Entities.WorkerStatus", b =>
                {
                    b.Property<byte>("Id")
                        .HasColumnType("smallint")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_worker_status");

                    b.ToTable("worker_status", (string)null);

                    b.HasData(
                        new
                        {
                            Id = (byte)1,
                            Name = "Unrecognized"
                        },
                        new
                        {
                            Id = (byte)2,
                            Name = "Active"
                        },
                        new
                        {
                            Id = (byte)3,
                            Name = "Disabled"
                        });
                });

            modelBuilder.Entity("RRMonitoring.Mining.Domain.Entities.PoolCoinAddress", b =>
                {
                    b.HasOne("RRMonitoring.Mining.Domain.Entities.Coin", "Coin")
                        .WithMany()
                        .HasForeignKey("CoinId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_pool_coin_address_coin_coin_id");

                    b.HasOne("RRMonitoring.Mining.Domain.Entities.Pool", "Pool")
                        .WithMany("CoinAddresses")
                        .HasForeignKey("PoolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_pool_coin_address_pool_pool_id");

                    b.Navigation("Coin");

                    b.Navigation("Pool");
                });

            modelBuilder.Entity("RRMonitoring.Mining.Domain.Entities.Worker", b =>
                {
                    b.HasOne("RRMonitoring.Mining.Domain.Entities.Client", "Client")
                        .WithMany("Workers")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("fk_worker_client_client_id");

                    b.HasOne("RRMonitoring.Mining.Domain.Entities.Coin", "Coin")
                        .WithMany()
                        .HasForeignKey("CoinId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_worker_coin_coin_id");

                    b.HasOne("RRMonitoring.Mining.Domain.Entities.Pool", "Pool")
                        .WithMany("Workers")
                        .HasForeignKey("PoolId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_worker_pool_pool_id");

                    b.HasOne("RRMonitoring.Mining.Domain.Entities.WorkerStatus", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_worker_worker_status_status_id");

                    b.Navigation("Client");

                    b.Navigation("Coin");

                    b.Navigation("Pool");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("RRMonitoring.Mining.Domain.Entities.Client", b =>
                {
                    b.Navigation("Workers");
                });

            modelBuilder.Entity("RRMonitoring.Mining.Domain.Entities.Pool", b =>
                {
                    b.Navigation("CoinAddresses");

                    b.Navigation("Workers");
                });
#pragma warning restore 612, 618
        }
    }
}
