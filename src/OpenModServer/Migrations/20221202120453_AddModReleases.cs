using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OpenModServer.Migrations
{
    /// <inheritdoc />
    public partial class AddModReleases : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "mod_releases",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    listingid = table.Column<Guid>(name: "listing_id", type: "uuid", nullable: false),
                    downloadcount = table.Column<int>(name: "download_count", type: "integer", nullable: false),
                    changelog = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    releasetype = table.Column<int>(name: "release_type", type: "integer", nullable: false),
                    createdat = table.Column<DateTime>(name: "created_at", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mod_releases", x => x.id);
                    table.ForeignKey(
                        name: "FK_mod_releases_mod_listings_listing_id",
                        column: x => x.listingid,
                        principalTable: "mod_listings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mod_release_approvals",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    modreleaseid = table.Column<Guid>(name: "mod_release_id", type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    PreviousStatus = table.Column<int>(type: "integer", nullable: false),
                    CurrentState = table.Column<int>(type: "integer", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false),
                    ModeratorResponsibleId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mod_release_approvals", x => x.id);
                    table.ForeignKey(
                        name: "FK_mod_release_approvals_mod_releases_mod_release_id",
                        column: x => x.modreleaseid,
                        principalTable: "mod_releases",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateIndex(
                name: "IX_mod_release_approvals_mod_release_id",
                table: "mod_release_approvals",
                column: "mod_release_id");

            migrationBuilder.CreateIndex(
                name: "IX_mod_releases_listing_id",
                table: "mod_releases",
                column: "listing_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mod_release_approvals");
            
            migrationBuilder.DropTable(
                name: "mod_releases");
        }
    }
}
