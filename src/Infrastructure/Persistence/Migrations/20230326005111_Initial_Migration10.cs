using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CleanArchitecture.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial_Migration10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_AttributeValue_AttributeId",
                schema: "Trendyol",
                table: "AttributeValue",
                newName: "IX_AttributeValue_AttributeId1");

            migrationBuilder.RenameIndex(
                name: "IX_Attribute_CategoryId",
                schema: "Trendyol",
                table: "Attribute",
                newName: "IX_Attribute_CategoryId1");

            migrationBuilder.CreateTable(
                name: "Attribute",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InternalId = table.Column<long>(type: "bigint", nullable: false),
                    Mandatory = table.Column<bool>(type: "boolean", nullable: false),
                    MultipleSelect = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Priority = table.Column<string>(type: "text", nullable: false),
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttributeValue",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AttributeId = table.Column<int>(type: "integer", nullable: false),
                    InternalId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributeValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttributeValue_Attribute_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "Attribute",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attribute_CategoryId",
                table: "Attribute",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AttributeValue_AttributeId",
                table: "AttributeValue",
                column: "AttributeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttributeValue");

            migrationBuilder.DropTable(
                name: "Attribute");

            migrationBuilder.RenameIndex(
                name: "IX_AttributeValue_AttributeId1",
                schema: "Trendyol",
                table: "AttributeValue",
                newName: "IX_AttributeValue_AttributeId");

            migrationBuilder.RenameIndex(
                name: "IX_Attribute_CategoryId1",
                schema: "Trendyol",
                table: "Attribute",
                newName: "IX_Attribute_CategoryId");
        }
    }
}
