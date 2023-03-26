using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial_Migration12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DependedName",
                schema: "NOnbir",
                table: "AttributeValue",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<double>(
                name: "Priority",
                schema: "NOnbir",
                table: "Attribute",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DependedName",
                schema: "NOnbir",
                table: "AttributeValue");

            migrationBuilder.AlterColumn<string>(
                name: "Priority",
                schema: "NOnbir",
                table: "Attribute",
                type: "text",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");
        }
    }
}
