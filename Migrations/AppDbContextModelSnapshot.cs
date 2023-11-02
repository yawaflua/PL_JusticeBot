﻿// <auto-generated />
using DiscordApp.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DiscordApp.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DiscordApp.Database.Tables.ArtsPatents", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<long>("Date")
                        .HasColumnType("bigint");

                    b.Property<string>("Employee")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int[]>("Number")
                        .IsRequired()
                        .HasColumnType("integer[]");

                    b.Property<string>("Size")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("isAllowedToResell")
                        .HasColumnType("boolean");

                    b.Property<int>("passportId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("passportId");

                    b.ToTable("ArtsPatent", "public");
                });

            modelBuilder.Entity("DiscordApp.Database.Tables.Autobranches", b =>
                {
                    b.Property<decimal>("ChannelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("BranchName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ChannelId");

                    b.ToTable("Autobranches", "public");
                });

            modelBuilder.Entity("DiscordApp.Database.Tables.Bizness", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ApplicantId")
                        .HasColumnType("integer");

                    b.Property<int[]>("BiznessEmployes")
                        .IsRequired()
                        .HasColumnType("integer[]");

                    b.Property<string>("BiznessName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("BiznessType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("CardNumber")
                        .HasColumnType("integer");

                    b.Property<long>("Date")
                        .HasColumnType("bigint");

                    b.Property<string>("Employee")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ApplicantId");

                    b.ToTable("Bizness", "public");
                });

            modelBuilder.Entity("DiscordApp.Database.Tables.BooksPatents", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Annotation")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("Date")
                        .HasColumnType("bigint");

                    b.Property<string>("Employee")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Janre")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("isAllowedToResell")
                        .HasColumnType("boolean");

                    b.Property<int>("passportId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("passportId");

                    b.ToTable("BooksPatent", "public");
                });

            modelBuilder.Entity("DiscordApp.Database.Tables.Passport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Applicant")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("Date")
                        .HasColumnType("bigint");

                    b.Property<decimal>("Employee")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RpName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Support")
                        .HasColumnType("integer");

                    b.Property<long>("birthDate")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Passport", "public");
                });

            modelBuilder.Entity("DiscordApp.Database.Tables.Reports", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Employee")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Reports", "public");
                });

            modelBuilder.Entity("DiscordApp.Database.Tables.ArtsPatents", b =>
                {
                    b.HasOne("DiscordApp.Database.Tables.Passport", "passport")
                        .WithMany()
                        .HasForeignKey("passportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("passport");
                });

            modelBuilder.Entity("DiscordApp.Database.Tables.Bizness", b =>
                {
                    b.HasOne("DiscordApp.Database.Tables.Passport", "Applicant")
                        .WithMany()
                        .HasForeignKey("ApplicantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Applicant");
                });

            modelBuilder.Entity("DiscordApp.Database.Tables.BooksPatents", b =>
                {
                    b.HasOne("DiscordApp.Database.Tables.Passport", "passport")
                        .WithMany()
                        .HasForeignKey("passportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("passport");
                });
#pragma warning restore 612, 618
        }
    }
}
