using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace VkGroupManager.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    CityId = table.Column<string>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.CityId);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentsId = table.Column<string>(nullable: false),
                    CanPost = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentsId);
                });

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    CountryId = table.Column<string>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "LikesSet",
                columns: table => new
                {
                    LikesId = table.Column<string>(nullable: false),
                    CanLike = table.Column<int>(nullable: false),
                    CanPublish = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: true),
                    UserLikes = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikesSet", x => x.LikesId);
                });

            migrationBuilder.CreateTable(
                name: "PhotoSet",
                columns: table => new
                {
                    PhotoId = table.Column<string>(nullable: false),
                    AccessKey = table.Column<string>(nullable: true),
                    AlbumId = table.Column<int>(nullable: false),
                    Date = table.Column<int>(nullable: false),
                    Height = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    OwnerId = table.Column<int>(nullable: false),
                    Photo1280 = table.Column<string>(nullable: true),
                    Photo130 = table.Column<string>(nullable: true),
                    Photo2560 = table.Column<string>(nullable: true),
                    Photo604 = table.Column<string>(nullable: true),
                    Photo75 = table.Column<string>(nullable: true),
                    Photo807 = table.Column<string>(nullable: true),
                    PostId = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    Width = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotoSet", x => x.PhotoId);
                });

            migrationBuilder.CreateTable(
                name: "PostSource",
                columns: table => new
                {
                    PostSourceId = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostSource", x => x.PostSourceId);
                });

            migrationBuilder.CreateTable(
                name: "RepostsSet",
                columns: table => new
                {
                    RepostsId = table.Column<string>(nullable: false),
                    Count = table.Column<int>(nullable: true),
                    UserReposted = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepostsSet", x => x.RepostsId);
                });

            migrationBuilder.CreateTable(
                name: "VideoSet",
                columns: table => new
                {
                    VideoId = table.Column<string>(nullable: false),
                    AccessKey = table.Column<string>(nullable: true),
                    CanAdd = table.Column<int>(nullable: false),
                    Comments = table.Column<int>(nullable: false),
                    Date = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Duration = table.Column<int>(nullable: false),
                    FileSize = table.Column<int>(nullable: false),
                    Height = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    OwnerId = table.Column<int>(nullable: false),
                    Photo130 = table.Column<string>(nullable: true),
                    Photo320 = table.Column<string>(nullable: true),
                    Photo640 = table.Column<string>(nullable: true),
                    Platform = table.Column<string>(nullable: true),
                    Src = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Views = table.Column<int>(nullable: false),
                    Width = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoSet", x => x.VideoId);
                });

            migrationBuilder.CreateTable(
                name: "ViewsSet",
                columns: table => new
                {
                    ViewsId = table.Column<string>(nullable: false),
                    Count = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViewsSet", x => x.ViewsId);
                });

            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    ContactId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: true),
                    OwnerID = table.Column<string>(nullable: true),
                    RegesterDate = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.ContactId);
                });

            migrationBuilder.CreateTable(
                name: "FilterSet",
                columns: table => new
                {
                    FilterId = table.Column<string>(nullable: false),
                    CopyWithAuthor = table.Column<bool>(nullable: true),
                    GetOnlyGroupPost = table.Column<bool>(nullable: true),
                    GetWithLink = table.Column<bool>(nullable: true),
                    GetWithPicture = table.Column<bool>(nullable: true),
                    GetWithVkLink = table.Column<bool>(nullable: true),
                    GetWithWikiPage = table.Column<bool>(nullable: true),
                    RemoveAuthor = table.Column<bool>(nullable: true),
                    RemoveTag = table.Column<bool>(nullable: true),
                    RemoveText = table.Column<bool>(nullable: true),
                    RepalaceFrom1 = table.Column<string>(nullable: true),
                    RepalaceFrom2 = table.Column<string>(nullable: true),
                    RepalaceTo1 = table.Column<string>(nullable: true),
                    RepalaceTo2 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilterSet", x => x.FilterId);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<string>(nullable: false),
                    Amount = table.Column<decimal>(nullable: true),
                    Date = table.Column<DateTime>(nullable: true),
                    DateEnd = table.Column<DateTime>(nullable: true),
                    IsOrdered = table.Column<bool>(nullable: false),
                    Operation_Id = table.Column<string>(nullable: true),
                    Payed = table.Column<decimal>(nullable: true),
                    Sender = table.Column<string>(nullable: true),
                    Sum = table.Column<decimal>(nullable: false),
                    UserId = table.Column<int>(nullable: true),
                    WithdrawAmount = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                });

            migrationBuilder.CreateTable(
                name: "VkClientSet",
                columns: table => new
                {
                    VkClientId = table.Column<string>(nullable: false),
                    ClientId = table.Column<string>(maxLength: 450, nullable: false),
                    ClientSecret = table.Column<string>(maxLength: 450, nullable: false),
                    Display = table.Column<string>(maxLength: 50, nullable: false),
                    InstagramQueryId = table.Column<string>(nullable: true),
                    RedirectUri = table.Column<string>(maxLength: 450, nullable: false),
                    ResponseType = table.Column<string>(maxLength: 50, nullable: false),
                    Revoke = table.Column<string>(maxLength: 450, nullable: true),
                    Scope = table.Column<string>(maxLength: 450, nullable: false),
                    ServerUrl = table.Column<string>(maxLength: 450, nullable: true),
                    State = table.Column<string>(maxLength: 450, nullable: true),
                    Version = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VkClientSet", x => x.VkClientId);
                });

            migrationBuilder.CreateTable(
                name: "VkUserSet",
                columns: table => new
                {
                    VkUserId = table.Column<string>(nullable: false),
                    AccessToken = table.Column<string>(nullable: true),
                    ExpiresIn = table.Column<string>(nullable: true),
                    UserVkId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VkUserSet", x => x.VkUserId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SizeSet",
                columns: table => new
                {
                    SizeId = table.Column<string>(nullable: false),
                    Height = table.Column<int>(nullable: false),
                    PhotoId = table.Column<string>(nullable: true),
                    Src = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Width = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SizeSet", x => x.SizeId);
                    table.ForeignKey(
                        name: "FK_SizeSet_PhotoSet_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "PhotoSet",
                        principalColumn: "PhotoId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PreviewSet",
                columns: table => new
                {
                    PreviewId = table.Column<string>(nullable: false),
                    PhotoId = table.Column<string>(nullable: true),
                    VideoId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreviewSet", x => x.PreviewId);
                    table.ForeignKey(
                        name: "FK_PreviewSet_PhotoSet_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "PhotoSet",
                        principalColumn: "PhotoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PreviewSet_VideoSet_VideoId",
                        column: x => x.VideoId,
                        principalTable: "VideoSet",
                        principalColumn: "VideoId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    InstagramOwnerId = table.Column<string>(nullable: true),
                    InstagramToken = table.Column<string>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    OrderId = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    RegesterDate = table.Column<DateTime>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    VkUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_VkUserSet_VkUserId",
                        column: x => x.VkUserId,
                        principalTable: "VkUserSet",
                        principalColumn: "VkUserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DocSet",
                columns: table => new
                {
                    DocId = table.Column<string>(nullable: false),
                    AccessKey = table.Column<string>(nullable: true),
                    Date = table.Column<int>(nullable: false),
                    Ext = table.Column<string>(nullable: true),
                    Id = table.Column<int>(nullable: false),
                    OwnerId = table.Column<int>(nullable: false),
                    PreviewId = table.Column<string>(nullable: true),
                    Size = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocSet", x => x.DocId);
                    table.ForeignKey(
                        name: "FK_DocSet_PreviewSet_PreviewId",
                        column: x => x.PreviewId,
                        principalTable: "PreviewSet",
                        principalColumn: "PreviewId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
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
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProfileSet",
                columns: table => new
                {
                    ProfileId = table.Column<string>(nullable: false),
                    Bdate = table.Column<string>(nullable: true),
                    CityId = table.Column<string>(nullable: true),
                    CountryId = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    HasPhoto = table.Column<int>(nullable: false),
                    HomeTown = table.Column<string>(nullable: true),
                    Id = table.Column<int>(nullable: false),
                    LastName = table.Column<string>(nullable: true),
                    Photo100 = table.Column<string>(nullable: true),
                    Photo200 = table.Column<string>(nullable: true),
                    Photo200Orig = table.Column<string>(nullable: true),
                    Photo50 = table.Column<string>(nullable: true),
                    PhotoId = table.Column<string>(nullable: true),
                    PhotoMax = table.Column<string>(nullable: true),
                    Sex = table.Column<int>(nullable: false),
                    Verified = table.Column<int>(nullable: false),
                    WallGetId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileSet", x => x.ProfileId);
                    table.ForeignKey(
                        name: "FK_ProfileSet_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "CityId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfileSet_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItemSet",
                columns: table => new
                {
                    ItemId = table.Column<string>(nullable: false),
                    AddedTime = table.Column<long>(nullable: true),
                    BackUpText = table.Column<string>(nullable: true),
                    CanPin = table.Column<int>(nullable: false),
                    CommentsId = table.Column<string>(nullable: true),
                    Date = table.Column<long>(nullable: false),
                    FromId = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    LikesId = table.Column<string>(nullable: true),
                    MarkedAsAds = table.Column<int>(nullable: false),
                    OwnerId = table.Column<int>(nullable: false),
                    PostSourceId = table.Column<string>(nullable: true),
                    PostType = table.Column<string>(nullable: true),
                    PublishDate = table.Column<long>(nullable: false),
                    RepostsId = table.Column<string>(nullable: true),
                    Statuse = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    TimeKey = table.Column<long>(nullable: false),
                    ViewsId = table.Column<string>(nullable: true),
                    WallGetId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemSet", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_ItemSet_Comments_CommentsId",
                        column: x => x.CommentsId,
                        principalTable: "Comments",
                        principalColumn: "CommentsId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemSet_LikesSet_LikesId",
                        column: x => x.LikesId,
                        principalTable: "LikesSet",
                        principalColumn: "LikesId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemSet_PostSource_PostSourceId",
                        column: x => x.PostSourceId,
                        principalTable: "PostSource",
                        principalColumn: "PostSourceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemSet_RepostsSet_RepostsId",
                        column: x => x.RepostsId,
                        principalTable: "RepostsSet",
                        principalColumn: "RepostsId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemSet_ViewsSet_ViewsId",
                        column: x => x.ViewsId,
                        principalTable: "ViewsSet",
                        principalColumn: "ViewsId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AttachmentSet",
                columns: table => new
                {
                    AttachmentId = table.Column<string>(nullable: false),
                    DocId = table.Column<string>(nullable: true),
                    ItemId = table.Column<string>(nullable: true),
                    PhotoId = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    VideoId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentSet", x => x.AttachmentId);
                    table.ForeignKey(
                        name: "FK_AttachmentSet_DocSet_DocId",
                        column: x => x.DocId,
                        principalTable: "DocSet",
                        principalColumn: "DocId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AttachmentSet_ItemSet_ItemId",
                        column: x => x.ItemId,
                        principalTable: "ItemSet",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AttachmentSet_PhotoSet_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "PhotoSet",
                        principalColumn: "PhotoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AttachmentSet_VideoSet_VideoId",
                        column: x => x.VideoId,
                        principalTable: "VideoSet",
                        principalColumn: "VideoId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VkGroupSet",
                columns: table => new
                {
                    VkGroupId = table.Column<string>(nullable: false),
                    AccessToken = table.Column<string>(nullable: true),
                    Atribute = table.Column<string>(nullable: true),
                    Domain = table.Column<string>(nullable: true),
                    ExpiresIn = table.Column<string>(nullable: true),
                    FilterId = table.Column<string>(nullable: true),
                    GroupId = table.Column<string>(nullable: true),
                    GroupInfoId = table.Column<string>(nullable: true),
                    MaxFrom = table.Column<int>(nullable: true),
                    MaxLoad = table.Column<int>(nullable: true),
                    TimeFrom = table.Column<int>(nullable: true),
                    TimeTo = table.Column<int>(nullable: true),
                    VkUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VkGroupSet", x => x.VkGroupId);
                    table.ForeignKey(
                        name: "FK_VkGroupSet_FilterSet_FilterId",
                        column: x => x.FilterId,
                        principalTable: "FilterSet",
                        principalColumn: "FilterId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VkGroupSet_VkUserSet_VkUserId",
                        column: x => x.VkUserId,
                        principalTable: "VkUserSet",
                        principalColumn: "VkUserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InstagramSet",
                columns: table => new
                {
                    InstagramId = table.Column<string>(nullable: false),
                    BackUpText = table.Column<string>(nullable: true),
                    Coment = table.Column<int>(nullable: true),
                    Date = table.Column<long>(nullable: true),
                    Likes = table.Column<int>(nullable: true),
                    PublishDate = table.Column<long>(nullable: false),
                    Statuse = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    TimeKey = table.Column<long>(nullable: false),
                    Url = table.Column<string>(nullable: true),
                    VkGroupId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstagramSet", x => x.InstagramId);
                    table.ForeignKey(
                        name: "FK_InstagramSet_VkGroupSet_VkGroupId",
                        column: x => x.VkGroupId,
                        principalTable: "VkGroupSet",
                        principalColumn: "VkGroupId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VkGroupFromSet",
                columns: table => new
                {
                    VkGroupFromId = table.Column<string>(nullable: false),
                    Atribute = table.Column<string>(nullable: true),
                    Domain = table.Column<string>(nullable: true),
                    FilterId = table.Column<string>(nullable: true),
                    GroupId = table.Column<string>(nullable: true),
                    GroupInfoId = table.Column<string>(nullable: true),
                    VkGroupId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VkGroupFromSet", x => x.VkGroupFromId);
                    table.ForeignKey(
                        name: "FK_VkGroupFromSet_FilterSet_FilterId",
                        column: x => x.FilterId,
                        principalTable: "FilterSet",
                        principalColumn: "FilterId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VkGroupFromSet_VkGroupSet_VkGroupId",
                        column: x => x.VkGroupId,
                        principalTable: "VkGroupSet",
                        principalColumn: "VkGroupId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WallGetSet",
                columns: table => new
                {
                    WallGetId = table.Column<string>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    VkGroupFromId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WallGetSet", x => x.WallGetId);
                    table.ForeignKey(
                        name: "FK_WallGetSet_VkGroupFromSet_VkGroupFromId",
                        column: x => x.VkGroupFromId,
                        principalTable: "VkGroupFromSet",
                        principalColumn: "VkGroupFromId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GroupInfoSet",
                columns: table => new
                {
                    GroupInfoId = table.Column<string>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    IsAdmin = table.Column<int>(nullable: false),
                    IsClosed = table.Column<int>(nullable: false),
                    IsMember = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Photo100 = table.Column<string>(nullable: true),
                    Photo200 = table.Column<string>(nullable: true),
                    Photo50 = table.Column<string>(nullable: true),
                    ScreenName = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    WallGetId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupInfoSet", x => x.GroupInfoId);
                    table.ForeignKey(
                        name: "FK_GroupInfoSet_WallGetSet_WallGetId",
                        column: x => x.WallGetId,
                        principalTable: "WallGetSet",
                        principalColumn: "WallGetId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentSet_DocId",
                table: "AttachmentSet",
                column: "DocId");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentSet_ItemId",
                table: "AttachmentSet",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentSet_PhotoId",
                table: "AttachmentSet",
                column: "PhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentSet_VideoId",
                table: "AttachmentSet",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_DocSet_PreviewId",
                table: "DocSet",
                column: "PreviewId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupInfoSet_WallGetId",
                table: "GroupInfoSet",
                column: "WallGetId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSet_CommentsId",
                table: "ItemSet",
                column: "CommentsId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSet_LikesId",
                table: "ItemSet",
                column: "LikesId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSet_PostSourceId",
                table: "ItemSet",
                column: "PostSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSet_RepostsId",
                table: "ItemSet",
                column: "RepostsId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSet_ViewsId",
                table: "ItemSet",
                column: "ViewsId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSet_WallGetId",
                table: "ItemSet",
                column: "WallGetId");

            migrationBuilder.CreateIndex(
                name: "IX_PreviewSet_PhotoId",
                table: "PreviewSet",
                column: "PhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_PreviewSet_VideoId",
                table: "PreviewSet",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileSet_CityId",
                table: "ProfileSet",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileSet_CountryId",
                table: "ProfileSet",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileSet_WallGetId",
                table: "ProfileSet",
                column: "WallGetId");

            migrationBuilder.CreateIndex(
                name: "IX_SizeSet_PhotoId",
                table: "SizeSet",
                column: "PhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_WallGetSet_VkGroupFromId",
                table: "WallGetSet",
                column: "VkGroupFromId");

            migrationBuilder.CreateIndex(
                name: "IX_InstagramSet_VkGroupId",
                table: "InstagramSet",
                column: "VkGroupId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_OrderId",
                table: "AspNetUsers",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_VkUserId",
                table: "AspNetUsers",
                column: "VkUserId");

            migrationBuilder.CreateIndex(
                name: "IX_VkGroupSet_FilterId",
                table: "VkGroupSet",
                column: "FilterId");

            migrationBuilder.CreateIndex(
                name: "IX_VkGroupSet_GroupInfoId",
                table: "VkGroupSet",
                column: "GroupInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_VkGroupSet_VkUserId",
                table: "VkGroupSet",
                column: "VkUserId");

            migrationBuilder.CreateIndex(
                name: "IX_VkGroupFromSet_FilterId",
                table: "VkGroupFromSet",
                column: "FilterId");

            migrationBuilder.CreateIndex(
                name: "IX_VkGroupFromSet_GroupInfoId",
                table: "VkGroupFromSet",
                column: "GroupInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_VkGroupFromSet_VkGroupId",
                table: "VkGroupFromSet",
                column: "VkGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileSet_WallGetSet_WallGetId",
                table: "ProfileSet",
                column: "WallGetId",
                principalTable: "WallGetSet",
                principalColumn: "WallGetId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemSet_WallGetSet_WallGetId",
                table: "ItemSet",
                column: "WallGetId",
                principalTable: "WallGetSet",
                principalColumn: "WallGetId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VkGroupSet_GroupInfoSet_GroupInfoId",
                table: "VkGroupSet",
                column: "GroupInfoId",
                principalTable: "GroupInfoSet",
                principalColumn: "GroupInfoId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VkGroupFromSet_GroupInfoSet_GroupInfoId",
                table: "VkGroupFromSet",
                column: "GroupInfoId",
                principalTable: "GroupInfoSet",
                principalColumn: "GroupInfoId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupInfoSet_WallGetSet_WallGetId",
                table: "GroupInfoSet");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AttachmentSet");

            migrationBuilder.DropTable(
                name: "ProfileSet");

            migrationBuilder.DropTable(
                name: "SizeSet");

            migrationBuilder.DropTable(
                name: "Contact");

            migrationBuilder.DropTable(
                name: "InstagramSet");

            migrationBuilder.DropTable(
                name: "VkClientSet");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "DocSet");

            migrationBuilder.DropTable(
                name: "ItemSet");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "PreviewSet");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "LikesSet");

            migrationBuilder.DropTable(
                name: "PostSource");

            migrationBuilder.DropTable(
                name: "RepostsSet");

            migrationBuilder.DropTable(
                name: "ViewsSet");

            migrationBuilder.DropTable(
                name: "PhotoSet");

            migrationBuilder.DropTable(
                name: "VideoSet");

            migrationBuilder.DropTable(
                name: "WallGetSet");

            migrationBuilder.DropTable(
                name: "VkGroupFromSet");

            migrationBuilder.DropTable(
                name: "VkGroupSet");

            migrationBuilder.DropTable(
                name: "FilterSet");

            migrationBuilder.DropTable(
                name: "GroupInfoSet");

            migrationBuilder.DropTable(
                name: "VkUserSet");
        }
    }
}
