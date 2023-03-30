﻿// <auto-generated />
using System;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230330004805_Initial_Migration1")]
    partial class Initial_Migration1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.NOnbir.Attribute", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<long>("InternalId")
                        .HasColumnType("bigint");

                    b.Property<bool>("Mandatory")
                        .HasColumnType("boolean");

                    b.Property<bool>("MultipleSelect")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasMaxLength(400)
                        .HasColumnType("character varying(400)");

                    b.Property<double>("Priority")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Attribute", "NOnbir");
                });

            modelBuilder.Entity("Domain.Entities.NOnbir.AttributeValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AttributeId")
                        .HasColumnType("integer");

                    b.Property<string>("DependedName")
                        .HasMaxLength(400)
                        .HasColumnType("character varying(400)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("character varying(400)");

                    b.HasKey("Id");

                    b.HasIndex("AttributeId");

                    b.ToTable("AttributeValue", "NOnbir");
                });

            modelBuilder.Entity("Domain.Entities.NOnbir.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("HasAttribute")
                        .HasColumnType("boolean");

                    b.Property<bool>("HasError")
                        .HasColumnType("boolean");

                    b.Property<long>("InternalId")
                        .HasColumnType("bigint");

                    b.Property<long?>("InternalParentId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsDeepest")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("character varying(400)");

                    b.Property<int?>("ParentId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("Category", "NOnbir");
                });

            modelBuilder.Entity("Domain.Entities.TodoItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<bool>("Done")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.Property<int>("ListId")
                        .HasColumnType("integer");

                    b.Property<string>("Note")
                        .HasColumnType("text");

                    b.Property<int>("Priority")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("Reminder")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.HasKey("Id");

                    b.HasIndex("ListId");

                    b.ToTable("TodoItems");
                });

            modelBuilder.Entity("Domain.Entities.TodoList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.HasKey("Id");

                    b.ToTable("TodoLists");
                });

            modelBuilder.Entity("Domain.Entities.Trendyol.Attribute", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("AllowCustom")
                        .HasColumnType("boolean");

                    b.Property<int>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<string>("DisplayName")
                        .HasMaxLength(400)
                        .HasColumnType("character varying(400)");

                    b.Property<long>("InternalId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("character varying(400)");

                    b.Property<bool>("Required")
                        .HasColumnType("boolean");

                    b.Property<bool>("Slicer")
                        .HasColumnType("boolean");

                    b.Property<bool>("Varianter")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId")
                        .HasDatabaseName("IX_Attribute_CategoryId1");

                    b.ToTable("Attribute", "Trendyol");
                });

            modelBuilder.Entity("Domain.Entities.Trendyol.AttributeValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AttributeId")
                        .HasColumnType("integer");

                    b.Property<int>("InternalId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("character varying(400)");

                    b.HasKey("Id");

                    b.HasIndex("AttributeId")
                        .HasDatabaseName("IX_AttributeValue_AttributeId1");

                    b.ToTable("AttributeValue", "Trendyol");
                });

            modelBuilder.Entity("Domain.Entities.Trendyol.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("HasAttribute")
                        .HasColumnType("boolean");

                    b.Property<int>("InternalId")
                        .HasColumnType("integer");

                    b.Property<int?>("InternalParentId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsDeepest")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("character varying(400)");

                    b.Property<int?>("ParentId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ParentId")
                        .HasDatabaseName("IX_Category_ParentId1");

                    b.ToTable("Category", "Trendyol");
                });

            modelBuilder.Entity("Domain.Entities.NOnbir.Attribute", b =>
                {
                    b.HasOne("Domain.Entities.NOnbir.Category", "Category")
                        .WithMany("Attributes")
                        .HasForeignKey("CategoryId");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Domain.Entities.NOnbir.AttributeValue", b =>
                {
                    b.HasOne("Domain.Entities.NOnbir.Attribute", "Attribute")
                        .WithMany("AttributeValues")
                        .HasForeignKey("AttributeId");

                    b.Navigation("Attribute");
                });

            modelBuilder.Entity("Domain.Entities.NOnbir.Category", b =>
                {
                    b.HasOne("Domain.Entities.NOnbir.Category", "Parent")
                        .WithMany("SubCategories")
                        .HasForeignKey("ParentId");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("Domain.Entities.TodoItem", b =>
                {
                    b.HasOne("Domain.Entities.TodoList", "List")
                        .WithMany("Items")
                        .HasForeignKey("ListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("List");
                });

            modelBuilder.Entity("Domain.Entities.TodoList", b =>
                {
                    b.OwnsOne("Domain.ValueObjects.Colour", "Colour", b1 =>
                        {
                            b1.Property<int>("TodoListId")
                                .HasColumnType("integer");

                            b1.Property<string>("Code")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.HasKey("TodoListId");

                            b1.ToTable("TodoLists");

                            b1.WithOwner()
                                .HasForeignKey("TodoListId");
                        });

                    b.Navigation("Colour")
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.Trendyol.Attribute", b =>
                {
                    b.HasOne("Domain.Entities.Trendyol.Category", "Category")
                        .WithMany("Attributes")
                        .HasForeignKey("CategoryId");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Domain.Entities.Trendyol.AttributeValue", b =>
                {
                    b.HasOne("Domain.Entities.Trendyol.Attribute", "Attribute")
                        .WithMany("AttributeValues")
                        .HasForeignKey("AttributeId");

                    b.Navigation("Attribute");
                });

            modelBuilder.Entity("Domain.Entities.Trendyol.Category", b =>
                {
                    b.HasOne("Domain.Entities.Trendyol.Category", "Parent")
                        .WithMany("SubCategories")
                        .HasForeignKey("ParentId");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("Domain.Entities.NOnbir.Attribute", b =>
                {
                    b.Navigation("AttributeValues");
                });

            modelBuilder.Entity("Domain.Entities.NOnbir.Category", b =>
                {
                    b.Navigation("Attributes");

                    b.Navigation("SubCategories");
                });

            modelBuilder.Entity("Domain.Entities.TodoList", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("Domain.Entities.Trendyol.Attribute", b =>
                {
                    b.Navigation("AttributeValues");
                });

            modelBuilder.Entity("Domain.Entities.Trendyol.Category", b =>
                {
                    b.Navigation("Attributes");

                    b.Navigation("SubCategories");
                });
#pragma warning restore 612, 618
        }
    }
}