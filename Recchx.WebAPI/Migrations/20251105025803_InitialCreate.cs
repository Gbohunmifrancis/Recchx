using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recchx.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "Campaigns",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    TemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ScheduledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SentCount = table.Column<int>(type: "integer", nullable: false),
                    OpenCount = table.Column<int>(type: "integer", nullable: false),
                    ClickCount = table.Column<int>(type: "integer", nullable: false),
                    ReplyCount = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaigns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Domain = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Industry = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Size = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Embedding = table.Column<float[]>(type: "real[]", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailTemplates",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Subject = table.Column<string>(type: "text", nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MailboxConnections",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Provider = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AccessToken = table.Column<string>(type: "text", nullable: false),
                    RefreshToken = table.Column<string>(type: "text", nullable: false),
                    Scope = table.Column<string>(type: "text", nullable: true),
                    ConnectedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailboxConnections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserCompanyMatches",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Score = table.Column<double>(type: "double precision", nullable: false),
                    GeneratedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCompanyMatches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthProviderId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    PasswordResetToken = table.Column<string>(type: "text", nullable: true),
                    PasswordResetTokenExpiry = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CampaignContacts",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CampaignId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContactId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SentAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OpenedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ClickedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RepliedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PersonalizedSubject = table.Column<string>(type: "text", nullable: true),
                    PersonalizedBody = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampaignContacts_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalSchema: "public",
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Verified = table.Column<bool>(type: "boolean", nullable: false),
                    LastVerifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contacts_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "public",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Summary = table.Column<string>(type: "text", nullable: true),
                    Skills = table.Column<List<string>>(type: "text[]", nullable: false),
                    Preferences = table.Column<string>(type: "jsonb", nullable: true),
                    Embedding = table.Column<float[]>(type: "real[]", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CampaignContacts_CampaignId_ContactId",
                schema: "public",
                table: "CampaignContacts",
                columns: new[] { "CampaignId", "ContactId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_UserId",
                schema: "public",
                table: "Campaigns",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_Domain",
                schema: "public",
                table: "Companies",
                column: "Domain",
                unique: true,
                filter: "\"Domain\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_CompanyId",
                schema: "public",
                table: "Contacts",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_Email",
                schema: "public",
                table: "Contacts",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplates_UserId",
                schema: "public",
                table: "EmailTemplates",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MailboxConnections_UserId",
                schema: "public",
                table: "MailboxConnections",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCompanyMatches_UserId",
                schema: "public",
                table: "UserCompanyMatches",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCompanyMatches_UserId_CompanyId",
                schema: "public",
                table: "UserCompanyMatches",
                columns: new[] { "UserId", "CompanyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_UserId",
                schema: "public",
                table: "UserProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_AuthProviderId",
                schema: "public",
                table: "Users",
                column: "AuthProviderId",
                unique: true,
                filter: "\"AuthProviderId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                schema: "public",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CampaignContacts",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Contacts",
                schema: "public");

            migrationBuilder.DropTable(
                name: "EmailTemplates",
                schema: "public");

            migrationBuilder.DropTable(
                name: "MailboxConnections",
                schema: "public");

            migrationBuilder.DropTable(
                name: "UserCompanyMatches",
                schema: "public");

            migrationBuilder.DropTable(
                name: "UserProfiles",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Campaigns",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Companies",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "public");
        }
    }
}
