using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenModServer.Migrations
{
    /// <inheritdoc />
    public partial class AddVisibilityToListings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<List<string>>(
                name: "tags",
                table: "mod_listings",
                type: "text[]",
                nullable: false,
                defaultValue: new List<string>(),
                oldClrType: typeof(List<string>),
                oldType: "text[]",
                oldDefaultValue: new List<string>());

            migrationBuilder.AddColumn<bool>(
                name: "is_visible_to_public",
                table: "mod_listings",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_visible_to_public",
                table: "mod_listings");

            migrationBuilder.AlterColumn<List<string>>(
                name: "tags",
                table: "mod_listings",
                type: "text[]",
                nullable: false,
                defaultValue: new List<string>(),
                oldClrType: typeof(List<string>),
                oldType: "text[]",
                oldDefaultValue: new List<string>());
        }
    }
}
