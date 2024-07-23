using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class orderTableBugFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OrderNumber",
                table: "Orders",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(16)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "OrderNumber",
                table: "Orders",
                type: "nvarchar(16)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
