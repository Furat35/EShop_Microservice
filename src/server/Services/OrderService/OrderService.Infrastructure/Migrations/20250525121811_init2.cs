using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductName",
                schema: "ordering",
                table: "orderItems",
                newName: "ItemName");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                schema: "ordering",
                table: "orderItems",
                newName: "ItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ItemName",
                schema: "ordering",
                table: "orderItems",
                newName: "ProductName");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                schema: "ordering",
                table: "orderItems",
                newName: "ProductId");
        }
    }
}
