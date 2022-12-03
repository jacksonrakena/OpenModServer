using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenModServer.Migrations
{
    /// <inheritdoc />
    public partial class AddManualRemoval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "mod_releases",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_mod_listings_creator_id",
                table: "mod_listings",
                column: "creator_id");

            migrationBuilder.AddForeignKey(
                name: "FK_mod_listings_AspNetUsers_creator_id",
                table: "mod_listings",
                column: "creator_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mod_listings_AspNetUsers_creator_id",
                table: "mod_listings");

            migrationBuilder.DropIndex(
                name: "IX_mod_listings_creator_id",
                table: "mod_listings");

            migrationBuilder.DropColumn(
                name: "name",
                table: "mod_releases");
        }
    }
}
