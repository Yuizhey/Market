using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Market.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addNewEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "UserDescriptions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "AuthorUserDescriptions",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "UserDescriptions");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "AuthorUserDescriptions");
        }
    }
}
