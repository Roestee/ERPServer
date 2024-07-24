using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class stockMovementTableUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StockChangeAmount",
                table: "StockMovements",
                newName: "NumberOfOutputs");

            migrationBuilder.AddColumn<decimal>(
                name: "NumberOfEntries",
                table: "StockMovements",
                type: "decimal(7,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfEntries",
                table: "StockMovements");

            migrationBuilder.RenameColumn(
                name: "NumberOfOutputs",
                table: "StockMovements",
                newName: "StockChangeAmount");
        }
    }
}
