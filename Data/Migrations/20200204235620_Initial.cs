using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace territory.mobi.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        { 
           migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.EnsureSchema(
                name: "app");

            migrationBuilder.CreateTable(
                name: "Cong",
                schema: "app",
                columns: table => new
                {
                    CongID = table.Column<Guid>(nullable: false),
                    CongName = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    ServID = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    UpdateDatetime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cong", x => x.CongID);
                });

            migrationBuilder.CreateTable(
                name: "dncpword",
                schema: "app",
                columns: table => new
                {
                    pwdID = table.Column<Guid>(nullable: false),
                    congID = table.Column<Guid>(nullable: false),
                    passwordHash = table.Column<string>(unicode: false, nullable: false),
                    notinuse = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dncpword", x => x.pwdID);
                });

            migrationBuilder.CreateTable(
                name: "MapAssignment",
                schema: "app",
                columns: table => new
                {
                    assignId = table.Column<Guid>(nullable: false),
                    mapId = table.Column<Guid>(nullable: false),
                    userId = table.Column<string>(maxLength: 450, nullable: true),
                    nonUserName = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    dateAssigned = table.Column<DateTime>(type: "datetime", nullable: false),
                    dateReturned = table.Column<DateTime>(type: "datetime", nullable: true),
                    updatedatetime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mapAssignment", x => x.assignId);
                });

            migrationBuilder.CreateTable(
                name: "MapFeature",
                schema: "app",
                columns: table => new
                {
                    MapFeatureId = table.Column<Guid>(nullable: false),
                    MapId = table.Column<Guid>(nullable: false),
                    Type = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Position = table.Column<string>(unicode: false, nullable: false),
                    Color = table.Column<string>(unicode: false, maxLength: 10, nullable: true),
                    Opacity = table.Column<decimal>(nullable: true),
                    zIndex = table.Column<int>(nullable: false),
                    zoom = table.Column<int>(nullable: false),
                    Title = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    updatedatetime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapFeature", x => x.MapFeatureId);
                });

            migrationBuilder.CreateTable(
                name: "pageHelp",
                schema: "app",
                columns: table => new
                {
                    pageId = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    sectionID = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    htmlHelp = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pageHelp", x => new { x.pageId, x.sectionID });
                });

            migrationBuilder.CreateTable(
                name: "Section",
                schema: "app",
                columns: table => new
                {
                    SectionId = table.Column<Guid>(nullable: false),
                    CongID = table.Column<Guid>(nullable: false),
                    SectionTitle = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    SortOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Section", x => x.SectionId);
                });

            migrationBuilder.CreateTable(
                name: "Setting",
                schema: "app",
                columns: table => new
                {
                    SettingId = table.Column<Guid>(nullable: false),
                    SettingType = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    SettingValue = table.Column<string>(unicode: false, nullable: false),
                    CongId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setting", x => x.SettingId);
                });

            migrationBuilder.CreateTable(
                name: "Token",
                schema: "app",
                columns: table => new
                {
                    tokenId = table.Column<Guid>(nullable: false),
                    UserEmail = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    UserCong = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Token", x => x.tokenId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Map",
                schema: "app",
                columns: table => new
                {
                    MapID = table.Column<Guid>(nullable: false),
                    CongID = table.Column<Guid>(nullable: false),
                    MapKey = table.Column<string>(unicode: false, maxLength: 5, nullable: false),
                    MapDesc = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    MapArea = table.Column<string>(unicode: false, maxLength: 30, nullable: false),
                    MapPolygon = table.Column<string>(unicode: false, nullable: true),
                    MapType = table.Column<string>(unicode: false, maxLength: 10, nullable: true),
                    imgID = table.Column<Guid>(nullable: true),
                    GoogleRef = table.Column<string>(unicode: false, nullable: true),
                    Notes = table.Column<string>(unicode: false, nullable: true),
                    Parking = table.Column<string>(unicode: false, nullable: true),
                    SectionID = table.Column<Guid>(nullable: true),
                    SortOrder = table.Column<int>(nullable: false),
                    Display = table.Column<bool>(nullable: false),
                    UpdateDatetime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Map", x => x.MapID);
                    table.ForeignKey(
                        name: "FK_Map_Section_SectionID",
                        column: x => x.SectionID,
                        principalSchema: "app",
                        principalTable: "Section",
                        principalColumn: "SectionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "dbo",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                schema: "dbo",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                schema: "dbo",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "dbo",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                schema: "dbo",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoNotCall",
                schema: "app",
                columns: table => new
                {
                    dncID = table.Column<Guid>(nullable: false),
                    mapID = table.Column<Guid>(nullable: false),
                    dateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    dateValidated = table.Column<DateTime>(type: "datetime", nullable: false),
                    streetNo = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    streetName = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    AptNo = table.Column<string>(unicode: false, maxLength: 10, nullable: true),
                    Suburb = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    Note = table.Column<string>(unicode: false, nullable: true),
                    display = table.Column<bool>(nullable: false),
                    geocode = table.Column<string>(unicode: false, nullable: true),
                    updateDatetime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_doNotCall", x => x.dncID);
                    table.ForeignKey(
                        name: "FK_DoNotCall_Map",
                        column: x => x.mapID,
                        principalSchema: "app",
                        principalTable: "Map",
                        principalColumn: "MapID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "images",
                schema: "app",
                columns: table => new
                {
                    imgID = table.Column<Guid>(nullable: false),
                    imgText = table.Column<string>(unicode: false, maxLength: 250, nullable: false),
                    imgPath = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    imgImage = table.Column<byte[]>(type: "image", nullable: true),
                    mapID = table.Column<Guid>(nullable: true),
                    updatedatetime = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_images", x => x.imgID);
                    table.ForeignKey(
                        name: "FK_images_Map",
                        column: x => x.mapID,
                        principalSchema: "app",
                        principalTable: "Map",
                        principalColumn: "MapID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DoNotCall_mapID",
                schema: "app",
                table: "DoNotCall",
                column: "mapID");

            migrationBuilder.CreateIndex(
                name: "IX_images_mapID",
                schema: "app",
                table: "images",
                column: "mapID");

            migrationBuilder.CreateIndex(
                name: "IX_Map_SectionID",
                schema: "app",
                table: "Map",
                column: "SectionID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                schema: "dbo",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "dbo",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                schema: "dbo",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                schema: "dbo",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                schema: "dbo",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "dbo",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "dbo",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cong",
                schema: "app");

            migrationBuilder.DropTable(
                name: "dncpword",
                schema: "app");

            migrationBuilder.DropTable(
                name: "DoNotCall",
                schema: "app");

            migrationBuilder.DropTable(
                name: "images",
                schema: "app");

            migrationBuilder.DropTable(
                name: "MapAssignment",
                schema: "app");

            migrationBuilder.DropTable(
                name: "MapFeature",
                schema: "app");

            migrationBuilder.DropTable(
                name: "pageHelp",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Setting",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Token",
                schema: "app");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Map",
                schema: "app");

            migrationBuilder.DropTable(
                name: "AspNetRoles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetUsers",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Section",
                schema: "app");
        }
    }
}
