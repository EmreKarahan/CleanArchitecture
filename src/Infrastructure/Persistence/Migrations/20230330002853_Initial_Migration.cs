using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial_Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "NOnbir");

            migrationBuilder.EnsureSchema(
                name: "Trendyol");

            migrationBuilder.CreateTable(
                name: "Category",
                schema: "NOnbir",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InternalId = table.Column<long>(type: "bigint", nullable: false),
                    InternalParentId = table.Column<long>(type: "bigint", nullable: true),
                    ParentId = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    HasAttribute = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeepest = table.Column<bool>(type: "boolean", nullable: false),
                    HasError = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Category_Category_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "NOnbir",
                        principalTable: "Category",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Category",
                schema: "Trendyol",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InternalId = table.Column<int>(type: "integer", nullable: false),
                    InternalParentId = table.Column<int>(type: "integer", nullable: true),
                    ParentId = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    IsDeepest = table.Column<bool>(type: "boolean", nullable: false),
                    HasAttribute = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Category_Category_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Trendyol",
                        principalTable: "Category",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TodoLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Colour_Code = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Attribute",
                schema: "NOnbir",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InternalId = table.Column<long>(type: "bigint", nullable: false),
                    Mandatory = table.Column<bool>(type: "boolean", nullable: false),
                    MultipleSelect = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true),
                    Priority = table.Column<double>(type: "double precision", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attribute", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attribute_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "NOnbir",
                        principalTable: "Category",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Attribute",
                schema: "Trendyol",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InternalId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    Required = table.Column<bool>(type: "boolean", nullable: false),
                    AllowCustom = table.Column<bool>(type: "boolean", nullable: false),
                    Varianter = table.Column<bool>(type: "boolean", nullable: false),
                    Slicer = table.Column<bool>(type: "boolean", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attribute", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attribute_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "Trendyol",
                        principalTable: "Category",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TodoItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ListId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Reminder = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Done = table.Column<bool>(type: "boolean", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TodoItems_TodoLists_ListId",
                        column: x => x.ListId,
                        principalTable: "TodoLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttributeValue",
                schema: "NOnbir",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AttributeId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    DependedName = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributeValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttributeValue_Attribute_AttributeId",
                        column: x => x.AttributeId,
                        principalSchema: "NOnbir",
                        principalTable: "Attribute",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AttributeValue",
                schema: "Trendyol",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InternalId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    AttributeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributeValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttributeValue_Attribute_AttributeId",
                        column: x => x.AttributeId,
                        principalSchema: "Trendyol",
                        principalTable: "Attribute",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attribute_CategoryId",
                schema: "NOnbir",
                table: "Attribute",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Attribute_CategoryId1",
                schema: "Trendyol",
                table: "Attribute",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AttributeValue_AttributeId",
                schema: "NOnbir",
                table: "AttributeValue",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_AttributeValue_AttributeId1",
                schema: "Trendyol",
                table: "AttributeValue",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_Category_ParentId",
                schema: "NOnbir",
                table: "Category",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Category_ParentId1",
                schema: "Trendyol",
                table: "Category",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_ListId",
                table: "TodoItems",
                column: "ListId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttributeValue",
                schema: "NOnbir");

            migrationBuilder.DropTable(
                name: "AttributeValue",
                schema: "Trendyol");

            migrationBuilder.DropTable(
                name: "TodoItems");

            migrationBuilder.DropTable(
                name: "Attribute",
                schema: "NOnbir");

            migrationBuilder.DropTable(
                name: "Attribute",
                schema: "Trendyol");

            migrationBuilder.DropTable(
                name: "TodoLists");

            migrationBuilder.DropTable(
                name: "Category",
                schema: "NOnbir");

            migrationBuilder.DropTable(
                name: "Category",
                schema: "Trendyol");
        }
    }
}
