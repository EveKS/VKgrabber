using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using VkGroupManager.Models;

namespace VkGroupManager.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("VkGroupManager.JsonModel.VK.Attachment", b =>
                {
                    b.Property<string>("AttachmentId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DocId");

                    b.Property<string>("ItemId");

                    b.Property<string>("PhotoId");

                    b.Property<string>("Type");

                    b.Property<string>("VideoId");

                    b.HasKey("AttachmentId");

                    b.HasIndex("DocId");

                    b.HasIndex("ItemId");

                    b.HasIndex("PhotoId");

                    b.HasIndex("VideoId");

                    b.ToTable("AttachmentSet");
                });

            modelBuilder.Entity("VkGroupManager.JsonModel.VK.City", b =>
                {
                    b.Property<string>("CityId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Id");

                    b.Property<string>("Title");

                    b.HasKey("CityId");

                    b.ToTable("City");
                });

            modelBuilder.Entity("VkGroupManager.JsonModel.VK.Comments", b =>
                {
                    b.Property<string>("CommentsId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CanPost");

                    b.Property<int>("Count");

                    b.HasKey("CommentsId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("VkGroupManager.JsonModel.VK.Country", b =>
                {
                    b.Property<string>("CountryId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Id");

                    b.Property<string>("Title");

                    b.HasKey("CountryId");

                    b.ToTable("Country");
                });

            modelBuilder.Entity("VkGroupManager.JsonModel.VK.Doc", b =>
                {
                    b.Property<string>("DocId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccessKey");

                    b.Property<int>("Date");

                    b.Property<string>("Ext");

                    b.Property<int>("Id");

                    b.Property<int>("OwnerId");

                    b.Property<string>("PreviewId");

                    b.Property<int>("Size");

                    b.Property<string>("Title");

                    b.Property<int>("Type");

                    b.Property<string>("Url");

                    b.HasKey("DocId");

                    b.HasIndex("PreviewId");

                    b.ToTable("DocSet");
                });

            modelBuilder.Entity("VkGroupManager.JsonModel.VK.GroupInfo", b =>
                {
                    b.Property<string>("GroupInfoId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Id");

                    b.Property<int>("IsAdmin");

                    b.Property<int>("IsClosed");

                    b.Property<int>("IsMember");

                    b.Property<string>("Name");

                    b.Property<string>("Photo100");

                    b.Property<string>("Photo200");

                    b.Property<string>("Photo50");

                    b.Property<string>("ScreenName");

                    b.Property<string>("Type");

                    b.Property<string>("WallGetId");

                    b.HasKey("GroupInfoId");

                    b.HasIndex("WallGetId");

                    b.ToTable("GroupInfoSet");
                });

            modelBuilder.Entity("VkGroupManager.JsonModel.VK.Item", b =>
                {
                    b.Property<string>("ItemId")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("AddedTime");

                    b.Property<string>("BackUpText");

                    b.Property<int>("CanPin");

                    b.Property<string>("CommentsId");

                    b.Property<long>("Date");

                    b.Property<int>("FromId");

                    b.Property<int>("Id");

                    b.Property<string>("LikesId");

                    b.Property<int>("MarkedAsAds");

                    b.Property<int>("OwnerId");

                    b.Property<string>("PostSourceId");

                    b.Property<string>("PostType");

                    b.Property<long>("PublishDate");

                    b.Property<string>("RepostsId");

                    b.Property<string>("Statuse");

                    b.Property<string>("Text");

                    b.Property<long>("TimeKey");

                    b.Property<string>("ViewsId");

                    b.Property<string>("WallGetId");

                    b.HasKey("ItemId");

                    b.HasIndex("CommentsId");

                    b.HasIndex("LikesId");

                    b.HasIndex("PostSourceId");

                    b.HasIndex("RepostsId");

                    b.HasIndex("ViewsId");

                    b.HasIndex("WallGetId");

                    b.ToTable("ItemSet");
                });

            modelBuilder.Entity("VkGroupManager.JsonModel.VK.Likes", b =>
                {
                    b.Property<string>("LikesId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CanLike");

                    b.Property<int>("CanPublish");

                    b.Property<int?>("Count");

                    b.Property<int>("UserLikes");

                    b.HasKey("LikesId");

                    b.ToTable("LikesSet");
                });

            modelBuilder.Entity("VkGroupManager.JsonModel.VK.Photo", b =>
                {
                    b.Property<string>("PhotoId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccessKey");

                    b.Property<int>("AlbumId");

                    b.Property<int>("Date");

                    b.Property<int>("Height");

                    b.Property<int>("Id");

                    b.Property<int>("OwnerId");

                    b.Property<string>("Photo1280");

                    b.Property<string>("Photo130");

                    b.Property<string>("Photo2560");

                    b.Property<string>("Photo604");

                    b.Property<string>("Photo75");

                    b.Property<string>("Photo807");

                    b.Property<int>("PostId");

                    b.Property<string>("Text");

                    b.Property<int>("UserId");

                    b.Property<int>("Width");

                    b.HasKey("PhotoId");

                    b.ToTable("PhotoSet");
                });

            modelBuilder.Entity("VkGroupManager.JsonModel.VK.PostSource", b =>
                {
                    b.Property<string>("PostSourceId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Type");

                    b.HasKey("PostSourceId");

                    b.ToTable("PostSource");
                });

            modelBuilder.Entity("VkGroupManager.JsonModel.VK.Preview", b =>
                {
                    b.Property<string>("PreviewId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("PhotoId");

                    b.Property<string>("VideoId");

                    b.HasKey("PreviewId");

                    b.HasIndex("PhotoId");

                    b.HasIndex("VideoId");

                    b.ToTable("PreviewSet");
                });

            modelBuilder.Entity("VkGroupManager.JsonModel.VK.Profile", b =>
                {
                    b.Property<string>("ProfileId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Bdate");

                    b.Property<string>("CityId");

                    b.Property<string>("CountryId");

                    b.Property<string>("FirstName");

                    b.Property<int>("HasPhoto");

                    b.Property<string>("HomeTown");

                    b.Property<int>("Id");

                    b.Property<string>("LastName");

                    b.Property<string>("Photo100");

                    b.Property<string>("Photo200");

                    b.Property<string>("Photo200Orig");

                    b.Property<string>("Photo50");

                    b.Property<string>("PhotoId");

                    b.Property<string>("PhotoMax");

                    b.Property<int>("Sex");

                    b.Property<int>("Verified");

                    b.Property<string>("WallGetId");

                    b.HasKey("ProfileId");

                    b.HasIndex("CityId");

                    b.HasIndex("CountryId");

                    b.HasIndex("WallGetId");

                    b.ToTable("ProfileSet");
                });

            modelBuilder.Entity("VkGroupManager.JsonModel.VK.Reposts", b =>
                {
                    b.Property<string>("RepostsId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("Count");

                    b.Property<int>("UserReposted");

                    b.HasKey("RepostsId");

                    b.ToTable("RepostsSet");
                });

            modelBuilder.Entity("VkGroupManager.JsonModel.VK.Size", b =>
                {
                    b.Property<string>("SizeId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Height");

                    b.Property<string>("PhotoId");

                    b.Property<string>("Src");

                    b.Property<string>("Type");

                    b.Property<int>("Width");

                    b.HasKey("SizeId");

                    b.HasIndex("PhotoId");

                    b.ToTable("SizeSet");
                });

            modelBuilder.Entity("VkGroupManager.JsonModel.VK.Video", b =>
                {
                    b.Property<string>("VideoId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccessKey");

                    b.Property<int>("CanAdd");

                    b.Property<int>("Comments");

                    b.Property<int>("Date");

                    b.Property<string>("Description");

                    b.Property<int>("Duration");

                    b.Property<int>("FileSize");

                    b.Property<int>("Height");

                    b.Property<int>("Id");

                    b.Property<int>("OwnerId");

                    b.Property<string>("Photo130");

                    b.Property<string>("Photo320");

                    b.Property<string>("Photo640");

                    b.Property<string>("Platform");

                    b.Property<string>("Src");

                    b.Property<string>("Title");

                    b.Property<int>("Views");

                    b.Property<int>("Width");

                    b.HasKey("VideoId");

                    b.ToTable("VideoSet");
                });

            modelBuilder.Entity("VkGroupManager.JsonModel.VK.Views", b =>
                {
                    b.Property<string>("ViewsId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("Count");

                    b.HasKey("ViewsId");

                    b.ToTable("ViewsSet");
                });

            modelBuilder.Entity("VkGroupManager.JsonModel.VK.WallGet", b =>
                {
                    b.Property<string>("WallGetId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Count");

                    b.Property<string>("VkGroupFromId");

                    b.HasKey("WallGetId");

                    b.HasIndex("VkGroupFromId");

                    b.ToTable("WallGetSet");
                });

            modelBuilder.Entity("VkGroupManager.Models.Contact", b =>
                {
                    b.Property<int>("ContactId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<bool?>("EmailConfirmed");

                    b.Property<string>("OwnerID");

                    b.Property<DateTime?>("RegesterDate");

                    b.Property<int>("Status");

                    b.HasKey("ContactId");

                    b.ToTable("Contact");
                });

            modelBuilder.Entity("VkGroupManager.Models.Filter", b =>
                {
                    b.Property<string>("FilterId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool?>("CopyWithAuthor");

                    b.Property<bool?>("GetOnlyGroupPost");

                    b.Property<bool?>("GetWithLink");

                    b.Property<bool?>("GetWithPicture");

                    b.Property<bool?>("GetWithVkLink");

                    b.Property<bool?>("GetWithWikiPage");

                    b.Property<bool?>("RemoveAuthor");

                    b.Property<bool?>("RemoveSmile");

                    b.Property<bool?>("RemoveTag");

                    b.Property<bool?>("RemoveText");

                    b.Property<string>("RepalaceFrom1");

                    b.Property<string>("RepalaceFrom2");

                    b.Property<string>("RepalaceTo1");

                    b.Property<string>("RepalaceTo2");

                    b.HasKey("FilterId");

                    b.ToTable("FilterSet");
                });

            modelBuilder.Entity("VkGroupManager.Models.Instagram", b =>
                {
                    b.Property<string>("InstagramId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BackUpText");

                    b.Property<int?>("Coment");

                    b.Property<long?>("Date");

                    b.Property<int?>("Likes");

                    b.Property<long>("PublishDate");

                    b.Property<string>("Statuse");

                    b.Property<string>("Text");

                    b.Property<long>("TimeKey");

                    b.Property<string>("Url");

                    b.Property<string>("VkGroupId");

                    b.HasKey("InstagramId");

                    b.HasIndex("VkGroupId");

                    b.ToTable("InstagramSet");
                });

            modelBuilder.Entity("VkGroupManager.Models.Order", b =>
                {
                    b.Property<string>("OrderId")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal?>("Amount");

                    b.Property<DateTime?>("Date");

                    b.Property<DateTime?>("DateEnd");

                    b.Property<bool>("IsOrdered");

                    b.Property<string>("Operation_Id");

                    b.Property<decimal?>("Payed");

                    b.Property<string>("Sender");

                    b.Property<decimal>("Sum");

                    b.Property<int?>("UserId");

                    b.Property<decimal?>("WithdrawAmount");

                    b.HasKey("OrderId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("VkGroupManager.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("InstagramOwnerId");

                    b.Property<string>("InstagramToken");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("OrderId");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<DateTime?>("RegesterDate");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.Property<string>("VkUserId");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.HasIndex("OrderId");

                    b.HasIndex("VkUserId");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("VkGroupManager.Models.VK.VkClient", b =>
                {
                    b.Property<string>("VkClientId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasMaxLength(450);

                    b.Property<string>("ClientSecret")
                        .IsRequired()
                        .HasMaxLength(450);

                    b.Property<string>("Display")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("InstagramQueryId");

                    b.Property<string>("RedirectUri")
                        .IsRequired()
                        .HasMaxLength(450);

                    b.Property<string>("ResponseType")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Revoke")
                        .HasMaxLength(450);

                    b.Property<string>("Scope")
                        .IsRequired()
                        .HasMaxLength(450);

                    b.Property<string>("ServerUrl")
                        .HasMaxLength(450);

                    b.Property<string>("State")
                        .HasMaxLength(450);

                    b.Property<string>("Version")
                        .HasMaxLength(50);

                    b.HasKey("VkClientId");

                    b.ToTable("VkClientSet");
                });

            modelBuilder.Entity("VkGroupManager.Models.VK.VkGroup", b =>
                {
                    b.Property<string>("VkGroupId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccessToken");

                    b.Property<string>("Atribute");

                    b.Property<string>("Domain");

                    b.Property<string>("ExpiresIn");

                    b.Property<string>("FilterId");

                    b.Property<string>("GroupId");

                    b.Property<string>("GroupInfoId");

                    b.Property<int?>("MaxFrom");

                    b.Property<int?>("MaxLoad");

                    b.Property<int?>("TimeFrom");

                    b.Property<int?>("TimeTo");

                    b.Property<string>("VkUserId");

                    b.HasKey("VkGroupId");

                    b.HasIndex("FilterId");

                    b.HasIndex("GroupInfoId");

                    b.HasIndex("VkUserId");

                    b.ToTable("VkGroupSet");
                });

            modelBuilder.Entity("VkGroupManager.Models.VK.VkGroupFrom", b =>
                {
                    b.Property<string>("VkGroupFromId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Atribute");

                    b.Property<string>("Domain");

                    b.Property<string>("FilterId");

                    b.Property<string>("GroupId");

                    b.Property<string>("GroupInfoId");

                    b.Property<string>("VkGroupId");

                    b.HasKey("VkGroupFromId");

                    b.HasIndex("FilterId");

                    b.HasIndex("GroupInfoId");

                    b.HasIndex("VkGroupId");

                    b.ToTable("VkGroupFromSet");
                });

            modelBuilder.Entity("VkGroupManager.Models.VK.VkUser", b =>
                {
                    b.Property<string>("VkUserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccessToken");

                    b.Property<string>("ExpiresIn");

                    b.Property<string>("UserVkId");

                    b.HasKey("VkUserId");

                    b.ToTable("VkUserSet");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("VkGroupManager.Models.User")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("VkGroupManager.Models.User")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("VkGroupManager.Models.User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VkGroupManager.JsonModel.VK.Attachment", b =>
                {
                    b.HasOne("VkGroupManager.JsonModel.VK.Doc", "Doc")
                        .WithMany()
                        .HasForeignKey("DocId");

                    b.HasOne("VkGroupManager.JsonModel.VK.Item", "Item")
                        .WithMany("Attachments")
                        .HasForeignKey("ItemId");

                    b.HasOne("VkGroupManager.JsonModel.VK.Photo", "Photo")
                        .WithMany()
                        .HasForeignKey("PhotoId");

                    b.HasOne("VkGroupManager.JsonModel.VK.Video", "Video")
                        .WithMany()
                        .HasForeignKey("VideoId");
                });

            modelBuilder.Entity("VkGroupManager.JsonModel.VK.Doc", b =>
                {
                    b.HasOne("VkGroupManager.JsonModel.VK.Preview", "Preview")
                        .WithMany()
                        .HasForeignKey("PreviewId");
                });

            modelBuilder.Entity("VkGroupManager.JsonModel.VK.GroupInfo", b =>
                {
                    b.HasOne("VkGroupManager.JsonModel.VK.WallGet", "WallGet")
                        .WithMany("GroupInfos")
                        .HasForeignKey("WallGetId");
                });

            modelBuilder.Entity("VkGroupManager.JsonModel.VK.Item", b =>
                {
                    b.HasOne("VkGroupManager.JsonModel.VK.Comments", "Comments")
                        .WithMany()
                        .HasForeignKey("CommentsId");

                    b.HasOne("VkGroupManager.JsonModel.VK.Likes", "Likes")
                        .WithMany()
                        .HasForeignKey("LikesId");

                    b.HasOne("VkGroupManager.JsonModel.VK.PostSource", "PostSource")
                        .WithMany()
                        .HasForeignKey("PostSourceId");

                    b.HasOne("VkGroupManager.JsonModel.VK.Reposts", "Reposts")
                        .WithMany()
                        .HasForeignKey("RepostsId");

                    b.HasOne("VkGroupManager.JsonModel.VK.Views", "Views")
                        .WithMany()
                        .HasForeignKey("ViewsId");

                    b.HasOne("VkGroupManager.JsonModel.VK.WallGet", "WallGet")
                        .WithMany("Items")
                        .HasForeignKey("WallGetId");
                });

            modelBuilder.Entity("VkGroupManager.JsonModel.VK.Preview", b =>
                {
                    b.HasOne("VkGroupManager.JsonModel.VK.Photo", "Photo")
                        .WithMany()
                        .HasForeignKey("PhotoId");

                    b.HasOne("VkGroupManager.JsonModel.VK.Video", "Video")
                        .WithMany()
                        .HasForeignKey("VideoId");
                });

            modelBuilder.Entity("VkGroupManager.JsonModel.VK.Profile", b =>
                {
                    b.HasOne("VkGroupManager.JsonModel.VK.City", "City")
                        .WithMany()
                        .HasForeignKey("CityId");

                    b.HasOne("VkGroupManager.JsonModel.VK.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId");

                    b.HasOne("VkGroupManager.JsonModel.VK.WallGet", "WallGet")
                        .WithMany("Profiles")
                        .HasForeignKey("WallGetId");
                });

            modelBuilder.Entity("VkGroupManager.JsonModel.VK.Size", b =>
                {
                    b.HasOne("VkGroupManager.JsonModel.VK.Photo")
                        .WithMany("Sizes")
                        .HasForeignKey("PhotoId");
                });

            modelBuilder.Entity("VkGroupManager.JsonModel.VK.WallGet", b =>
                {
                    b.HasOne("VkGroupManager.Models.VK.VkGroupFrom", "VkGroupFrom")
                        .WithMany("WallGets")
                        .HasForeignKey("VkGroupFromId");
                });

            modelBuilder.Entity("VkGroupManager.Models.Instagram", b =>
                {
                    b.HasOne("VkGroupManager.Models.VK.VkGroup", "VkGroup")
                        .WithMany("Instagrams")
                        .HasForeignKey("VkGroupId");
                });

            modelBuilder.Entity("VkGroupManager.Models.User", b =>
                {
                    b.HasOne("VkGroupManager.Models.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId");

                    b.HasOne("VkGroupManager.Models.VK.VkUser", "VkUser")
                        .WithMany()
                        .HasForeignKey("VkUserId");
                });

            modelBuilder.Entity("VkGroupManager.Models.VK.VkGroup", b =>
                {
                    b.HasOne("VkGroupManager.Models.Filter", "Filter")
                        .WithMany()
                        .HasForeignKey("FilterId");

                    b.HasOne("VkGroupManager.JsonModel.VK.GroupInfo", "GroupInfo")
                        .WithMany()
                        .HasForeignKey("GroupInfoId");

                    b.HasOne("VkGroupManager.Models.VK.VkUser", "VkUser")
                        .WithMany("VkGroups")
                        .HasForeignKey("VkUserId");
                });

            modelBuilder.Entity("VkGroupManager.Models.VK.VkGroupFrom", b =>
                {
                    b.HasOne("VkGroupManager.Models.Filter", "Filter")
                        .WithMany()
                        .HasForeignKey("FilterId");

                    b.HasOne("VkGroupManager.JsonModel.VK.GroupInfo", "GroupInfo")
                        .WithMany()
                        .HasForeignKey("GroupInfoId");

                    b.HasOne("VkGroupManager.Models.VK.VkGroup", "VkGroup")
                        .WithMany("VkGroupsFrom")
                        .HasForeignKey("VkGroupId");
                });
        }
    }
}
