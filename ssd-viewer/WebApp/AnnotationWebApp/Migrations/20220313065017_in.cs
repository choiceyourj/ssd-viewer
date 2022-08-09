using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AnnotationWebApp.Migrations
{
    public partial class @in : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SsdRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    RoleDescription = table.Column<string>(type: "text", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SsdRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SsdUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserDisplayName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SsdUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SsdRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SsdRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SsdRoleClaims_SsdRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "SsdRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EndoscopeVideos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    VideoFileLocation = table.Column<string>(type: "text", nullable: false),
                    TotalNumberOfFrame = table.Column<int>(type: "integer", nullable: false),
                    IsAllImageTreated = table.Column<bool>(type: "boolean", nullable: false),
                    UploadTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UserId = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EndoscopeVideos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EndoscopeVideos_SsdUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "SsdUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SsdUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SsdUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SsdUserClaims_SsdUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "SsdUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SsdUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SsdUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_SsdUserLogins_SsdUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "SsdUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SsdUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SsdUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_SsdUserRoles_SsdRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "SsdRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SsdUserRoles_SsdUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "SsdUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SsdUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SsdUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_SsdUserTokens_SsdUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "SsdUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StillCutImages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    ImageFileLocation = table.Column<string>(type: "text", nullable: false),
                    LastUpdateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ImageCreatedTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsCropComplete = table.Column<bool>(type: "boolean", nullable: false),
                    VideoId = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    UserId = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StillCutImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StillCutImages_EndoscopeVideos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "EndoscopeVideos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StillCutImages_SsdUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "SsdUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TumorPositions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    ImageId = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    StartX = table.Column<int>(type: "integer", nullable: false),
                    Width = table.Column<int>(type: "integer", nullable: false),
                    Height = table.Column<int>(type: "integer", nullable: false),
                    StartY = table.Column<int>(type: "integer", nullable: false),
                    EndX = table.Column<int>(type: "integer", nullable: false),
                    EndY = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TumorPositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TumorPositions_StillCutImages_ImageId",
                        column: x => x.ImageId,
                        principalTable: "StillCutImages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EndoscopeVideos_UserId",
                table: "EndoscopeVideos",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SsdRoleClaims_RoleId",
                table: "SsdRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "SsdRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SsdUserClaims_UserId",
                table: "SsdUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SsdUserLogins_UserId",
                table: "SsdUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SsdUserRoles_RoleId",
                table: "SsdUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "SsdUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "SsdUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StillCutImages_UserId",
                table: "StillCutImages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StillCutImages_VideoId",
                table: "StillCutImages",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_TumorPositions_ImageId",
                table: "TumorPositions",
                column: "ImageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SsdRoleClaims");

            migrationBuilder.DropTable(
                name: "SsdUserClaims");

            migrationBuilder.DropTable(
                name: "SsdUserLogins");

            migrationBuilder.DropTable(
                name: "SsdUserRoles");

            migrationBuilder.DropTable(
                name: "SsdUserTokens");

            migrationBuilder.DropTable(
                name: "TumorPositions");

            migrationBuilder.DropTable(
                name: "SsdRoles");

            migrationBuilder.DropTable(
                name: "StillCutImages");

            migrationBuilder.DropTable(
                name: "EndoscopeVideos");

            migrationBuilder.DropTable(
                name: "SsdUsers");
        }
    }
}
