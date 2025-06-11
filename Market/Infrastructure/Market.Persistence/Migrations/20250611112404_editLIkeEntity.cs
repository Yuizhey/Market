using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Market.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class editLIkeEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Likes",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Likes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Likes",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Likes",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Likes");
        }
    }
}
