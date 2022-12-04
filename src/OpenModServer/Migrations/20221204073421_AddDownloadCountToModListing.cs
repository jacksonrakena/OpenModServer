using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenModServer.Migrations
{
    /// <inheritdoc />
    public partial class AddDownloadCountToModListing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "download_count",
                table: "mod_listings",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "download_count",
                table: "mod_listings");
        }
    }
}
