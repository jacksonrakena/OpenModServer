using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenModServer.Migrations
{
    /// <inheritdoc />
    public partial class AddModeratorResponsibleRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_mod_release_approvals_ModeratorResponsibleId",
                table: "mod_release_approvals",
                column: "ModeratorResponsibleId");

            migrationBuilder.AddForeignKey(
                name: "FK_mod_release_approvals_AspNetUsers_ModeratorResponsibleId",
                table: "mod_release_approvals",
                column: "ModeratorResponsibleId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mod_release_approvals_AspNetUsers_ModeratorResponsibleId",
                table: "mod_release_approvals");

            migrationBuilder.DropIndex(
                name: "IX_mod_release_approvals_ModeratorResponsibleId",
                table: "mod_release_approvals");
        }
    }
}
