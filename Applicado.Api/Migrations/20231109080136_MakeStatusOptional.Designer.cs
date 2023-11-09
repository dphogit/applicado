﻿// <auto-generated />
using System;
using Applicado.Api.Data;
using Applicado.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Applicado.Api.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20231109080136_MakeStatusOptional")]
    partial class MakeStatusOptional
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "status", new[] { "open", "applied", "offer", "rejected", "accepted", "closed" });
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Applicado.Api.Models.JobApplication", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset?>("CloseAtDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("close_at_date_time");

                    b.Property<string>("Company")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("company");

                    b.Property<DateTimeOffset>("CreatedAtDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at_date_time");

                    b.Property<string>("Link")
                        .HasColumnType("text")
                        .HasColumnName("link");

                    b.Property<string>("Notes")
                        .HasColumnType("text")
                        .HasColumnName("notes");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("role");

                    b.Property<Status?>("Status")
                        .HasColumnType("status")
                        .HasColumnName("status");

                    b.Property<DateTimeOffset>("UpdatedAtDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at_date_time");

                    b.HasKey("Id")
                        .HasName("pk_job_applications");

                    b.ToTable("job_applications", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
