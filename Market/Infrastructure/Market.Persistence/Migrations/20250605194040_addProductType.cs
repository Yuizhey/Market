using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Market.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addProductType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductType",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductType",
                table: "Products");
        }
    }
}
