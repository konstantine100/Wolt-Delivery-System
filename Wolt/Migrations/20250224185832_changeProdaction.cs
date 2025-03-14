using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wolt.Migrations
{
    /// <inheritdoc />
    public partial class changeProdaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductionId",
                table: "ProdCategories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductionId",
                table: "ProdCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
