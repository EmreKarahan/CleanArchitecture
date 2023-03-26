using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial_Migration11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attribute_Category_CategoryId",
                table: "Attribute");

            migrationBuilder.DropForeignKey(
                name: "FK_AttributeValue_Attribute_AttributeId",
                table: "AttributeValue");

            migrationBuilder.RenameTable(
                name: "AttributeValue",
                newName: "AttributeValue",
                newSchema: "NOnbir");

            migrationBuilder.RenameTable(
                name: "Attribute",
                newName: "Attribute",
                newSchema: "NOnbir");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "NOnbir",
                table: "AttributeValue",
                type: "character varying(400)",
                maxLength: 400,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "NOnbir",
                table: "Attribute",
                type: "character varying(400)",
                maxLength: 400,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_Attribute_Category_CategoryId",
                schema: "NOnbir",
                table: "Attribute",
                column: "CategoryId",
                principalSchema: "NOnbir",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AttributeValue_Attribute_AttributeId",
                schema: "NOnbir",
                table: "AttributeValue",
                column: "AttributeId",
                principalSchema: "NOnbir",
                principalTable: "Attribute",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attribute_Category_CategoryId",
                schema: "NOnbir",
                table: "Attribute");

            migrationBuilder.DropForeignKey(
                name: "FK_AttributeValue_Attribute_AttributeId",
                schema: "NOnbir",
                table: "AttributeValue");

            migrationBuilder.RenameTable(
                name: "AttributeValue",
                schema: "NOnbir",
                newName: "AttributeValue");

            migrationBuilder.RenameTable(
                name: "Attribute",
                schema: "NOnbir",
                newName: "Attribute");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AttributeValue",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(400)",
                oldMaxLength: 400);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Attribute",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(400)",
                oldMaxLength: 400,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Attribute_Category_CategoryId",
                table: "Attribute",
                column: "CategoryId",
                principalSchema: "NOnbir",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AttributeValue_Attribute_AttributeId",
                table: "AttributeValue",
                column: "AttributeId",
                principalTable: "Attribute",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
