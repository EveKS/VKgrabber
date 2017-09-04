using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkGroupManager.JsonModel.VK;
using VkGroupManager.Models.VK;

namespace VkGroupManager.Models
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public DbSet<Contact> Contact { get; set; }

        public DbSet<VkUser> VkUserSet { get; set; }
        public DbSet<VkGroup> VkGroupSet { get; set; }
        public DbSet<VkClient> VkClientSet { get; set; }
        public DbSet<VkGroupFrom> VkGroupFromSet { get; set; }

        public DbSet<Item> ItemSet { get; set; }
        public DbSet<WallGet> WallGetSet { get; set; }
        public DbSet<Profile> ProfileSet { get; set; }
        public DbSet<GroupInfo> GroupInfoSet { get; set; }

        public DbSet<Video> VideoSet { get; set; }
        public DbSet<Photo> PhotoSet { get; set; }
        public DbSet<Doc> DocSet { get; set; }
        public DbSet<Size> SizeSet { get; set; }
        public DbSet<Preview> PreviewSet { get; set; }
        public DbSet<Attachment> AttachmentSet { get; set; }

        public DbSet<Likes> LikesSet { get; set; }
        public DbSet<Views> ViewsSet { get; set; }
        public DbSet<Reposts> RepostsSet { get; set; }

        public DbSet<Instagram> InstagramSet { get; set; }

        public DbSet<Filter> FilterSet { get; set; }

        public DbSet<Order> Orders { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }
    }
}
