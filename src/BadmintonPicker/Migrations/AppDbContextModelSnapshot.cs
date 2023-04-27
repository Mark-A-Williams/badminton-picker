﻿// <auto-generated />
using System;
using BadmintonPicker.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BadmintonPicker.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BadmintonPicker.Entities.Player", b =>
                {
                    b.Property<string>("Initials")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Initials");

                    b.HasIndex("FirstName", "LastName")
                        .IsUnique();

                    b.ToTable("Players");
                });

            modelBuilder.Entity("BadmintonPicker.Entities.PlayerSession", b =>
                {
                    b.Property<string>("PlayerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("SessionId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("PlayerId", "SessionId");

                    b.HasIndex("SessionId");

                    b.ToTable("PlayerSessions");
                });

            modelBuilder.Entity("BadmintonPicker.Entities.Session", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("date");

                    b.HasKey("Id");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("BadmintonPicker.Entities.PlayerSession", b =>
                {
                    b.HasOne("BadmintonPicker.Entities.Player", "Player")
                        .WithMany("PlayerSessions")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BadmintonPicker.Entities.Session", "Session")
                        .WithMany("PlayerSessions")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");

                    b.Navigation("Session");
                });

            modelBuilder.Entity("BadmintonPicker.Entities.Player", b =>
                {
                    b.Navigation("PlayerSessions");
                });

            modelBuilder.Entity("BadmintonPicker.Entities.Session", b =>
                {
                    b.Navigation("PlayerSessions");
                });
#pragma warning restore 612, 618
        }
    }
}
