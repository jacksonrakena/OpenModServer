﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using OpenModServer.Data;

#nullable disable

namespace OpenModServer.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20221219055117_AddVisibilityToListings")]
    partial class AddVisibilityToListings
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text")
                        .HasColumnName("concurrency_stamp");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("discriminator");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("name");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("normalized_name");

                    b.HasKey("Id")
                        .HasName("pk_roles");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("roles", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityRole<Guid>");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text")
                        .HasColumnName("claim_type");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text")
                        .HasColumnName("claim_value");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid")
                        .HasColumnName("role_id");

                    b.HasKey("Id")
                        .HasName("pk_role_claims");

                    b.HasIndex("RoleId")
                        .HasDatabaseName("ix_role_claims_role_id");

                    b.ToTable("role_claims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text")
                        .HasColumnName("claim_type");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text")
                        .HasColumnName("claim_value");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_user_claims");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_user_claims_user_id");

                    b.ToTable("user_claims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("login_provider");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("provider_key");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text")
                        .HasColumnName("provider_display_name");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("LoginProvider", "ProviderKey")
                        .HasName("pk_user_logins");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_user_logins_user_id");

                    b.ToTable("user_logins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid")
                        .HasColumnName("role_id");

                    b.HasKey("UserId", "RoleId")
                        .HasName("pk_user_roles");

                    b.HasIndex("RoleId")
                        .HasDatabaseName("ix_user_roles_role_id");

                    b.ToTable("user_roles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("login_provider");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("name");

                    b.Property<string>("Value")
                        .HasColumnType("text")
                        .HasColumnName("value");

                    b.HasKey("UserId", "LoginProvider", "Name")
                        .HasName("pk_user_tokens");

                    b.ToTable("user_tokens", (string)null);
                });

            modelBuilder.Entity("OpenModServer.Data.Comments.ModComment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid")
                        .HasColumnName("author_id");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(560)
                        .HasColumnType("character varying(560)")
                        .HasColumnName("content");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("now()");

                    b.Property<Guid>("ModListingId")
                        .HasColumnType("uuid")
                        .HasColumnName("listing_id");

                    b.Property<Guid?>("ParentCommentId")
                        .HasColumnType("uuid")
                        .HasColumnName("parent_comment_id");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_comments");

                    b.HasIndex("AuthorId")
                        .HasDatabaseName("ix_comments_author_id");

                    b.HasIndex("ModListingId")
                        .HasDatabaseName("ix_comments_listing_id");

                    b.HasIndex("ParentCommentId")
                        .HasDatabaseName("ix_comments_parent_comment_id");

                    b.ToTable("comments", (string)null);
                });

            modelBuilder.Entity("OpenModServer.Data.Identity.OmsUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer")
                        .HasColumnName("access_failed_count");

                    b.Property<string>("City")
                        .HasColumnType("text")
                        .HasColumnName("city");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text")
                        .HasColumnName("concurrency_stamp");

                    b.Property<string>("CountryIsoCode")
                        .HasColumnType("text")
                        .HasColumnName("country_iso_code");

                    b.Property<string>("DiscordInviteCode")
                        .HasColumnType("text")
                        .HasColumnName("discord_invite_code");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("email");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean")
                        .HasColumnName("email_confirmed");

                    b.Property<string>("FacebookPageName")
                        .HasColumnType("text")
                        .HasColumnName("facebook_page_name");

                    b.Property<string>("GitHubName")
                        .HasColumnType("text")
                        .HasColumnName("git_hub_name");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean")
                        .HasColumnName("lockout_enabled");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("lockout_end");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("normalized_email");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("normalized_user_name");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text")
                        .HasColumnName("password_hash");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text")
                        .HasColumnName("phone_number");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean")
                        .HasColumnName("phone_number_confirmed");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text")
                        .HasColumnName("security_stamp");

                    b.Property<string>("SteamCommunityName")
                        .HasColumnType("text")
                        .HasColumnName("steam_community_name");

                    b.Property<string>("TwitterUsername")
                        .HasColumnType("text")
                        .HasColumnName("twitter_username");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean")
                        .HasColumnName("two_factor_enabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("user_name");

                    b.Property<string>("Website")
                        .HasColumnType("text")
                        .HasColumnName("website");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("OpenModServer.Data.ModListing", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("now()");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uuid")
                        .HasColumnName("creator_id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)")
                        .HasColumnName("description");

                    b.Property<int>("DownloadCount")
                        .HasColumnType("integer")
                        .HasColumnName("download_count");

                    b.Property<string>("GameIdentifier")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("game_identifier");

                    b.Property<bool>("IsVisibleToPublic")
                        .HasColumnType("boolean")
                        .HasColumnName("is_visible_to_public");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("name");

                    b.Property<Guid?>("PinnedComment")
                        .HasColumnType("uuid")
                        .HasColumnName("pinned_comment");

                    b.Property<Guid?>("PinnedCommentId")
                        .HasColumnType("uuid")
                        .HasColumnName("pinned_comment_id");

                    b.Property<string>("Tagline")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("tagline");

                    b.Property<List<string>>("Tags")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text[]")
                        .HasDefaultValue(new List<string>())
                        .HasColumnName("tags");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_mod_listings");

                    b.HasIndex("CreatorId")
                        .HasDatabaseName("ix_mod_listings_creator_id");

                    b.HasIndex("GameIdentifier")
                        .HasDatabaseName("ix_mod_listings_game_identifier");

                    b.HasIndex("Tags")
                        .HasDatabaseName("ix_mod_listings_tags");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex("Tags"), "gin");

                    b.ToTable("mod_listings", (string)null);
                });

            modelBuilder.Entity("OpenModServer.Data.Releases.Approvals.ModReleaseApprovalChange", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("now()");

                    b.Property<int>("CurrentState")
                        .HasColumnType("integer")
                        .HasColumnName("current_state");

                    b.Property<bool>("IsPrivateNote")
                        .HasColumnType("boolean")
                        .HasColumnName("is_private_note");

                    b.Property<Guid>("ModReleaseId")
                        .HasColumnType("uuid")
                        .HasColumnName("mod_release_id");

                    b.Property<Guid?>("ModeratorResponsibleId")
                        .HasColumnType("uuid")
                        .HasColumnName("moderator_responsible_id");

                    b.Property<int>("PreviousStatus")
                        .HasColumnType("integer")
                        .HasColumnName("previous_status");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("reason");

                    b.HasKey("Id")
                        .HasName("pk_mod_release_approvals");

                    b.HasIndex("ModReleaseId")
                        .HasDatabaseName("ix_mod_release_approvals_mod_release_id");

                    b.HasIndex("ModeratorResponsibleId")
                        .HasDatabaseName("ix_mod_release_approvals_moderator_responsible_id");

                    b.ToTable("mod_release_approvals", (string)null);
                });

            modelBuilder.Entity("OpenModServer.Data.Releases.ModRelease", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Changelog")
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)")
                        .HasColumnName("changelog");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("now()");

                    b.Property<int>("CurrentStatus")
                        .HasColumnType("integer")
                        .HasColumnName("current_status");

                    b.Property<int>("DownloadCount")
                        .HasColumnType("integer")
                        .HasColumnName("download_count");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("file_name");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("file_path");

                    b.Property<decimal>("FileSizeKilobytes")
                        .HasColumnType("numeric(20,0)")
                        .HasColumnName("file_size_kilobytes");

                    b.Property<Guid>("ModListingId")
                        .HasColumnType("uuid")
                        .HasColumnName("listing_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<Guid?>("OmsUserId")
                        .HasColumnType("uuid")
                        .HasColumnName("oms_user_id");

                    b.Property<int>("ReleaseType")
                        .HasColumnType("integer")
                        .HasColumnName("release_type");

                    b.Property<string>("VT_AnalysisId")
                        .HasColumnType("text")
                        .HasColumnName("vt_analysis_id");

                    b.Property<DateTime?>("VT_LastUpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("vt_last_updated");

                    b.Property<int?>("VT_NumberOfFailedScans")
                        .HasColumnType("integer")
                        .HasColumnName("vt_num_fails");

                    b.Property<int?>("VT_NumberOfHarmlessScans")
                        .HasColumnType("integer")
                        .HasColumnName("vt_num_harmless");

                    b.Property<int?>("VT_NumberOfMaliciousScans")
                        .HasColumnType("integer")
                        .HasColumnName("vt_num_malicious");

                    b.Property<int?>("VT_NumberOfSuspiciousScans")
                        .HasColumnType("integer")
                        .HasColumnName("vt_num_sus");

                    b.Property<int?>("VT_ScanResult")
                        .HasColumnType("integer")
                        .HasColumnName("vt_scan_result");

                    b.Property<DateTime?>("VT_SubmittedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("vt_submitted_at");

                    b.HasKey("Id")
                        .HasName("pk_mod_releases");

                    b.HasIndex("ModListingId")
                        .HasDatabaseName("ix_mod_releases_listing_id");

                    b.HasIndex("OmsUserId")
                        .HasDatabaseName("ix_mod_releases_oms_user_id");

                    b.ToTable("mod_releases", (string)null);
                });

            modelBuilder.Entity("OpenModServer.Data.Identity.OmsRole", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>");

                    b.ToTable("roles", (string)null);

                    b.HasDiscriminator().HasValue("OmsRole");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_role_claims_roles_role_id");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("OpenModServer.Data.Identity.OmsUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_claims_users_user_id");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("OpenModServer.Data.Identity.OmsUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_logins_users_user_id");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_roles_roles_role_id");

                    b.HasOne("OpenModServer.Data.Identity.OmsUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_roles_users_user_id");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("OpenModServer.Data.Identity.OmsUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_tokens_users_user_id");
                });

            modelBuilder.Entity("OpenModServer.Data.Comments.ModComment", b =>
                {
                    b.HasOne("OpenModServer.Data.Identity.OmsUser", "Author")
                        .WithMany("Comments")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_comments_users_author_id");

                    b.HasOne("OpenModServer.Data.ModListing", "Listing")
                        .WithMany("Comments")
                        .HasForeignKey("ModListingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_comments_mod_listings_listing_id");

                    b.HasOne("OpenModServer.Data.Comments.ModComment", "ParentComment")
                        .WithMany()
                        .HasForeignKey("ParentCommentId")
                        .HasConstraintName("fk_comments_comments_parent_comment_id");

                    b.Navigation("Author");

                    b.Navigation("Listing");

                    b.Navigation("ParentComment");
                });

            modelBuilder.Entity("OpenModServer.Data.ModListing", b =>
                {
                    b.HasOne("OpenModServer.Data.Identity.OmsUser", "Creator")
                        .WithMany("Listings")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_mod_listings_users_creator_id");

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("OpenModServer.Data.Releases.Approvals.ModReleaseApprovalChange", b =>
                {
                    b.HasOne("OpenModServer.Data.Releases.ModRelease", "ModRelease")
                        .WithMany("ApprovalHistory")
                        .HasForeignKey("ModReleaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_mod_release_approvals_mod_releases_mod_release_id");

                    b.HasOne("OpenModServer.Data.Identity.OmsUser", "ModeratorResponsible")
                        .WithMany()
                        .HasForeignKey("ModeratorResponsibleId")
                        .HasConstraintName("fk_mod_release_approvals_users_moderator_responsible_id");

                    b.Navigation("ModRelease");

                    b.Navigation("ModeratorResponsible");
                });

            modelBuilder.Entity("OpenModServer.Data.Releases.ModRelease", b =>
                {
                    b.HasOne("OpenModServer.Data.ModListing", "ModListing")
                        .WithMany("Releases")
                        .HasForeignKey("ModListingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_mod_releases_mod_listings_listing_id");

                    b.HasOne("OpenModServer.Data.Identity.OmsUser", null)
                        .WithMany("Releases")
                        .HasForeignKey("OmsUserId")
                        .HasConstraintName("fk_mod_releases_users_oms_user_id");

                    b.Navigation("ModListing");
                });

            modelBuilder.Entity("OpenModServer.Data.Identity.OmsUser", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Listings");

                    b.Navigation("Releases");
                });

            modelBuilder.Entity("OpenModServer.Data.ModListing", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Releases");
                });

            modelBuilder.Entity("OpenModServer.Data.Releases.ModRelease", b =>
                {
                    b.Navigation("ApprovalHistory");
                });
#pragma warning restore 612, 618
        }
    }
}
