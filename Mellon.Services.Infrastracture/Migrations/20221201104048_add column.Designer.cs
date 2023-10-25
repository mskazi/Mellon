﻿// <auto-generated />
using System;
using Mellon.Services.Infrastracture.Context;
using Mellon.Services.Infrastracture.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Mellon.Services.Infrastracture.Migrations
{
    [DbContext(typeof(MellonContext))]
    [Migration("20221201104048_add column")]
    partial class addcolumn
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Mellon.Services.Infrastracture.Models.Approval", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ApprovalOrderId")
                        .HasColumnType("int");

                    b.Property<string>("ApprovalProcessComment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ApprovalRequestComment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ApprovalResponsible")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ApprovalSequence")
                        .HasColumnType("int");

                    b.Property<string>("DocumentNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DocumentOwner")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DocumentOwnerEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DocumentToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DocumentType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ERPCompany")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ERPCountry")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ERPTimeStamp")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RequestNo")
                        .HasColumnType("int");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ApprovalOrderId");

                    b.ToTable("Approvals");
                });

            modelBuilder.Entity("Mellon.Services.Infrastracture.Models.ApprovalLine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("ApprovalOrderId")
                        .HasColumnType("int");

                    b.Property<string>("BL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BLName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BU")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BUName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DocumentNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DocumentType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ERPCompany")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ERPTimeStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ergo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ErgoName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("LineAmount")
                        .HasColumnType("float");

                    b.Property<string>("LineNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PLLines")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PLLinesName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Quantity")
                        .HasColumnType("float");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<double?>("UnitPrice")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("ApprovalOrderId");

                    b.ToTable("ApprovalLines");
                });

            modelBuilder.Entity("Mellon.Services.Infrastracture.Models.ApprovalNotification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DocumentNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DocumentOwner")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DocumentToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DocumentType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("NotificationSend")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ApprovalNotifications");
                });

            modelBuilder.Entity("Mellon.Services.Infrastracture.Models.ApprovalOrder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("BL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BLName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BUName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Bu")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Currency")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DocumentNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DocumentType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ERPCompany")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ERPCountry")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ERPTimeStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ergo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ErgoName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NotificationMail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PLLines")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PLLinesName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("SourceName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ApprovalOrders");
                });

            modelBuilder.Entity("Mellon.Services.Infrastracture.Models.Approval", b =>
                {
                    b.HasOne("Mellon.Services.Infrastracture.Models.ApprovalOrder", null)
                        .WithMany("Approvals")
                        .HasForeignKey("ApprovalOrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Mellon.Services.Infrastracture.Models.ApprovalLine", b =>
                {
                    b.HasOne("Mellon.Services.Infrastracture.Models.ApprovalOrder", null)
                        .WithMany("ApprovalLines")
                        .HasForeignKey("ApprovalOrderId");
                });

            modelBuilder.Entity("Mellon.Services.Infrastracture.Models.ApprovalOrder", b =>
                {
                    b.Navigation("ApprovalLines");

                    b.Navigation("Approvals");
                });
#pragma warning restore 612, 618
        }
    }
}
