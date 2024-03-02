﻿// <auto-generated />
using System;
using Database.MySql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Database.MySql.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Database.MySql.Models.Game", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("GameName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("HostUserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("HostUserId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Database.MySql.Models.InventoryType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("GameId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("InventoryTypes");
                });

            modelBuilder.Entity("Database.MySql.Models.InventoryTypeOption", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("InventoryTypeId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("InventoryTypeId");

                    b.ToTable("InventoryTypeOptions");
                });

            modelBuilder.Entity("Database.MySql.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("GameUser", b =>
                {
                    b.Property<Guid>("GamesId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("PlayersId")
                        .HasColumnType("char(36)");

                    b.HasKey("GamesId", "PlayersId");

                    b.HasIndex("PlayersId");

                    b.ToTable("GameUser");
                });

            modelBuilder.Entity("Database.MySql.Models.Game", b =>
                {
                    b.HasOne("Database.MySql.Models.User", "HostUser")
                        .WithMany("HostGames")
                        .HasForeignKey("HostUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("HostUser");
                });

            modelBuilder.Entity("Database.MySql.Models.InventoryType", b =>
                {
                    b.HasOne("Database.MySql.Models.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");
                });

            modelBuilder.Entity("Database.MySql.Models.InventoryTypeOption", b =>
                {
                    b.HasOne("Database.MySql.Models.InventoryType", "InventoryType")
                        .WithMany("Options")
                        .HasForeignKey("InventoryTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("InventoryType");
                });

            modelBuilder.Entity("GameUser", b =>
                {
                    b.HasOne("Database.MySql.Models.Game", null)
                        .WithMany()
                        .HasForeignKey("GamesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Database.MySql.Models.User", null)
                        .WithMany()
                        .HasForeignKey("PlayersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Database.MySql.Models.InventoryType", b =>
                {
                    b.Navigation("Options");
                });

            modelBuilder.Entity("Database.MySql.Models.User", b =>
                {
                    b.Navigation("HostGames");
                });
#pragma warning restore 612, 618
        }
    }
}
