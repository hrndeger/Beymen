using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beymen.OrderService.Entity.Migrations
{
    /// <inheritdoc />
    public partial class orderupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descriptiom",
                table: "Orders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Descriptiom",
                table: "Orders",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
