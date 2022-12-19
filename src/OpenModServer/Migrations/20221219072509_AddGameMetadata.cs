using System;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenModServer.Migrations
{
    /// <inheritdoc />
    public partial class AddGameMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "pinned_comment",
                table: "mod_listings");

            migrationBuilder.DropColumn(
                name: "pinned_comment_id",
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

            migrationBuilder.AddColumn<JsonNode>(
                name: "game_metadata",
                table: "mod_listings",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{}'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "game_metadata",
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

            migrationBuilder.AddColumn<Guid>(
                name: "pinned_comment",
                table: "mod_listings",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "pinned_comment_id",
                table: "mod_listings",
                type: "uuid",
                nullable: true);
        }
    }
}
