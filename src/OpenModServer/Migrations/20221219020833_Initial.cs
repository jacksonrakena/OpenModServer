using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OpenModServer.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalizedname = table.Column<string>(name: "normalized_name", type: "character varying(256)", maxLength: 256, nullable: true),
                    concurrencystamp = table.Column<string>(name: "concurrency_stamp", type: "text", nullable: true),
                    discriminator = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    countryisocode = table.Column<string>(name: "country_iso_code", type: "text", nullable: true),
                    city = table.Column<string>(type: "text", nullable: true),
                    website = table.Column<string>(type: "text", nullable: true),
                    facebookpagename = table.Column<string>(name: "facebook_page_name", type: "text", nullable: true),
                    twitterusername = table.Column<string>(name: "twitter_username", type: "text", nullable: true),
                    steamcommunityname = table.Column<string>(name: "steam_community_name", type: "text", nullable: true),
                    githubname = table.Column<string>(name: "git_hub_name", type: "text", nullable: true),
                    discordinvitecode = table.Column<string>(name: "discord_invite_code", type: "text", nullable: true),
                    username = table.Column<string>(name: "user_name", type: "character varying(256)", maxLength: 256, nullable: true),
                    normalizedusername = table.Column<string>(name: "normalized_user_name", type: "character varying(256)", maxLength: 256, nullable: true),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalizedemail = table.Column<string>(name: "normalized_email", type: "character varying(256)", maxLength: 256, nullable: true),
                    emailconfirmed = table.Column<bool>(name: "email_confirmed", type: "boolean", nullable: false),
                    passwordhash = table.Column<string>(name: "password_hash", type: "text", nullable: true),
                    securitystamp = table.Column<string>(name: "security_stamp", type: "text", nullable: true),
                    concurrencystamp = table.Column<string>(name: "concurrency_stamp", type: "text", nullable: true),
                    phonenumber = table.Column<string>(name: "phone_number", type: "text", nullable: true),
                    phonenumberconfirmed = table.Column<bool>(name: "phone_number_confirmed", type: "boolean", nullable: false),
                    twofactorenabled = table.Column<bool>(name: "two_factor_enabled", type: "boolean", nullable: false),
                    lockoutend = table.Column<DateTimeOffset>(name: "lockout_end", type: "timestamp with time zone", nullable: true),
                    lockoutenabled = table.Column<bool>(name: "lockout_enabled", type: "boolean", nullable: false),
                    accessfailedcount = table.Column<int>(name: "access_failed_count", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role_claims",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    roleid = table.Column<Guid>(name: "role_id", type: "uuid", nullable: false),
                    claimtype = table.Column<string>(name: "claim_type", type: "text", nullable: true),
                    claimvalue = table.Column<string>(name: "claim_value", type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_role_claims_roles_role_id",
                        column: x => x.roleid,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mod_listings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    creatorid = table.Column<Guid>(name: "creator_id", type: "uuid", nullable: false),
                    gameidentifier = table.Column<string>(name: "game_identifier", type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    tagline = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    description = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    createdat = table.Column<DateTime>(name: "created_at", type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updatedat = table.Column<DateTime>(name: "updated_at", type: "timestamp with time zone", nullable: true),
                    downloadcount = table.Column<int>(name: "download_count", type: "integer", nullable: false),
                    pinnedcomment = table.Column<Guid>(name: "pinned_comment", type: "uuid", nullable: true),
                    pinnedcommentid = table.Column<Guid>(name: "pinned_comment_id", type: "uuid", nullable: true),
                    tags = table.Column<List<string>>(type: "text[]", nullable: false, defaultValue: new List<string>())
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mod_listings", x => x.id);
                    table.ForeignKey(
                        name: "fk_mod_listings_users_creator_id",
                        column: x => x.creatorid,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_claims",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userid = table.Column<Guid>(name: "user_id", type: "uuid", nullable: false),
                    claimtype = table.Column<string>(name: "claim_type", type: "text", nullable: true),
                    claimvalue = table.Column<string>(name: "claim_value", type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_claims_users_user_id",
                        column: x => x.userid,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_logins",
                columns: table => new
                {
                    loginprovider = table.Column<string>(name: "login_provider", type: "character varying(128)", maxLength: 128, nullable: false),
                    providerkey = table.Column<string>(name: "provider_key", type: "character varying(128)", maxLength: 128, nullable: false),
                    providerdisplayname = table.Column<string>(name: "provider_display_name", type: "text", nullable: true),
                    userid = table.Column<Guid>(name: "user_id", type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_logins", x => new { x.loginprovider, x.providerkey });
                    table.ForeignKey(
                        name: "fk_user_logins_users_user_id",
                        column: x => x.userid,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                columns: table => new
                {
                    userid = table.Column<Guid>(name: "user_id", type: "uuid", nullable: false),
                    roleid = table.Column<Guid>(name: "role_id", type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_roles", x => new { x.userid, x.roleid });
                    table.ForeignKey(
                        name: "fk_user_roles_roles_role_id",
                        column: x => x.roleid,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_roles_users_user_id",
                        column: x => x.userid,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_tokens",
                columns: table => new
                {
                    userid = table.Column<Guid>(name: "user_id", type: "uuid", nullable: false),
                    loginprovider = table.Column<string>(name: "login_provider", type: "character varying(128)", maxLength: 128, nullable: false),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_tokens", x => new { x.userid, x.loginprovider, x.name });
                    table.ForeignKey(
                        name: "fk_user_tokens_users_user_id",
                        column: x => x.userid,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "comments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    listingid = table.Column<Guid>(name: "listing_id", type: "uuid", nullable: false),
                    content = table.Column<string>(type: "character varying(560)", maxLength: 560, nullable: false),
                    authorid = table.Column<Guid>(name: "author_id", type: "uuid", nullable: false),
                    parentcommentid = table.Column<Guid>(name: "parent_comment_id", type: "uuid", nullable: true),
                    createdat = table.Column<DateTime>(name: "created_at", type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updatedat = table.Column<DateTime>(name: "updated_at", type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_comments", x => x.id);
                    table.ForeignKey(
                        name: "fk_comments_comments_parent_comment_id",
                        column: x => x.parentcommentid,
                        principalTable: "comments",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_comments_mod_listings_listing_id",
                        column: x => x.listingid,
                        principalTable: "mod_listings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_comments_users_author_id",
                        column: x => x.authorid,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mod_releases",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    listingid = table.Column<Guid>(name: "listing_id", type: "uuid", nullable: false),
                    downloadcount = table.Column<int>(name: "download_count", type: "integer", nullable: false),
                    changelog = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    releasetype = table.Column<int>(name: "release_type", type: "integer", nullable: false),
                    createdat = table.Column<DateTime>(name: "created_at", type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    currentstatus = table.Column<int>(name: "current_status", type: "integer", nullable: false),
                    filepath = table.Column<string>(name: "file_path", type: "text", nullable: false),
                    filename = table.Column<string>(name: "file_name", type: "text", nullable: false),
                    filesizekilobytes = table.Column<decimal>(name: "file_size_kilobytes", type: "numeric(20,0)", nullable: false),
                    vtanalysisid = table.Column<string>(name: "vt_analysis_id", type: "text", nullable: true),
                    vtsubmittedat = table.Column<DateTime>(name: "vt_submitted_at", type: "timestamp with time zone", nullable: true),
                    vtlastupdated = table.Column<DateTime>(name: "vt_last_updated", type: "timestamp with time zone", nullable: true),
                    vtnumfails = table.Column<int>(name: "vt_num_fails", type: "integer", nullable: true),
                    vtnumharmless = table.Column<int>(name: "vt_num_harmless", type: "integer", nullable: true),
                    vtnummalicious = table.Column<int>(name: "vt_num_malicious", type: "integer", nullable: true),
                    vtnumsus = table.Column<int>(name: "vt_num_sus", type: "integer", nullable: true),
                    vtscanresult = table.Column<int>(name: "vt_scan_result", type: "integer", nullable: true),
                    omsuserid = table.Column<Guid>(name: "oms_user_id", type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mod_releases", x => x.id);
                    table.ForeignKey(
                        name: "fk_mod_releases_mod_listings_listing_id",
                        column: x => x.listingid,
                        principalTable: "mod_listings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_mod_releases_users_oms_user_id",
                        column: x => x.omsuserid,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "mod_release_approvals",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    modreleaseid = table.Column<Guid>(name: "mod_release_id", type: "uuid", nullable: false),
                    createdat = table.Column<DateTime>(name: "created_at", type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    previousstatus = table.Column<int>(name: "previous_status", type: "integer", nullable: false),
                    currentstate = table.Column<int>(name: "current_state", type: "integer", nullable: false),
                    isprivatenote = table.Column<bool>(name: "is_private_note", type: "boolean", nullable: false),
                    reason = table.Column<string>(type: "text", nullable: false),
                    moderatorresponsibleid = table.Column<Guid>(name: "moderator_responsible_id", type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mod_release_approvals", x => x.id);
                    table.ForeignKey(
                        name: "fk_mod_release_approvals_mod_releases_mod_release_id",
                        column: x => x.modreleaseid,
                        principalTable: "mod_releases",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_mod_release_approvals_users_moderator_responsible_id",
                        column: x => x.moderatorresponsibleid,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_comments_author_id",
                table: "comments",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "ix_comments_listing_id",
                table: "comments",
                column: "listing_id");

            migrationBuilder.CreateIndex(
                name: "ix_comments_parent_comment_id",
                table: "comments",
                column: "parent_comment_id");

            migrationBuilder.CreateIndex(
                name: "ix_mod_listings_creator_id",
                table: "mod_listings",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "ix_mod_listings_game_identifier",
                table: "mod_listings",
                column: "game_identifier");

            migrationBuilder.CreateIndex(
                name: "ix_mod_listings_tags",
                table: "mod_listings",
                column: "tags")
                .Annotation("Npgsql:IndexMethod", "gin");

            migrationBuilder.CreateIndex(
                name: "ix_mod_release_approvals_mod_release_id",
                table: "mod_release_approvals",
                column: "mod_release_id");

            migrationBuilder.CreateIndex(
                name: "ix_mod_release_approvals_moderator_responsible_id",
                table: "mod_release_approvals",
                column: "moderator_responsible_id");

            migrationBuilder.CreateIndex(
                name: "ix_mod_releases_listing_id",
                table: "mod_releases",
                column: "listing_id");

            migrationBuilder.CreateIndex(
                name: "ix_mod_releases_oms_user_id",
                table: "mod_releases",
                column: "oms_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_role_claims_role_id",
                table: "role_claims",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "roles",
                column: "normalized_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_claims_user_id",
                table: "user_claims",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_logins_user_id",
                table: "user_logins",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_roles_role_id",
                table: "user_roles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "users",
                column: "normalized_email");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "users",
                column: "normalized_user_name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "comments");

            migrationBuilder.DropTable(
                name: "mod_release_approvals");

            migrationBuilder.DropTable(
                name: "role_claims");

            migrationBuilder.DropTable(
                name: "user_claims");

            migrationBuilder.DropTable(
                name: "user_logins");

            migrationBuilder.DropTable(
                name: "user_roles");

            migrationBuilder.DropTable(
                name: "user_tokens");

            migrationBuilder.DropTable(
                name: "mod_releases");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "mod_listings");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
