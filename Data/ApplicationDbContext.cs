using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project_Manager.Model;
using System.Reflection.Emit;

namespace Project_Manager.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
     
        public override DbSet<User> Users { get; set; }

        public override DbSet<Role> Roles { get; set; }

        public DbSet<Wiki> Wikis { get; set; }

        public DbSet<Organization> Organizations { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<OrganizationUser> OrganizationUser { get; set; }

        public DbSet<GroupUser> GroupUsers { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Note> Notes { get; set; }

        public DbSet<Issue> Issues { get; set; }

        public DbSet<Avatar> Avatars { get; set; }

        public DbSet<Requests> Requests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }
    }
}
