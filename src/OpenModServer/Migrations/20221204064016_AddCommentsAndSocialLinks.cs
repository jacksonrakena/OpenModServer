using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenModServer.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentsAndSocialLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OmsUserId",
                table: "mod_releases",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PinnedComment",
                table: "mod_listings",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PinnedCommentId",
                table: "mod_listings",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiscordInviteCode",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FacebookPageName",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GitHubName",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SteamCommunityName",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwitterUsername",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    listingid = table.Column<Guid>(name: "listing_id", type: "uuid", nullable: false),
                    content = table.Column<string>(type: "character varying(560)", maxLength: 560, nullable: false),
                    authorid = table.Column<Guid>(name: "author_id", type: "uuid", nullable: false),
                    parentcommentid = table.Column<Guid>(name: "parent_comment_id", type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.id);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_author_id",
                        column: x => x.authorid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Comments_parent_comment_id",
                        column: x => x.parentcommentid,
                        principalTable: "Comments",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Comments_mod_listings_listing_id",
                        column: x => x.listingid,
                        principalTable: "mod_listings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_mod_releases_OmsUserId",
                table: "mod_releases",
                column: "OmsUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_author_id",
                table: "Comments",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_listing_id",
                table: "Comments",
                column: "listing_id");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_parent_comment_id",
                table: "Comments",
                column: "parent_comment_id");

            migrationBuilder.AddForeignKey(
                name: "FK_mod_releases_AspNetUsers_OmsUserId",
                table: "mod_releases",
                column: "OmsUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mod_releases_AspNetUsers_OmsUserId",
                table: "mod_releases");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_mod_releases_OmsUserId",
                table: "mod_releases");

            migrationBuilder.DropColumn(
                name: "OmsUserId",
                table: "mod_releases");

            migrationBuilder.DropColumn(
                name: "PinnedComment",
                table: "mod_listings");

            migrationBuilder.DropColumn(
                name: "PinnedCommentId",
                table: "mod_listings");

            migrationBuilder.DropColumn(
                name: "DiscordInviteCode",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FacebookPageName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GitHubName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SteamCommunityName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TwitterUsername",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "AspNetUsers");
        }
    }
}
