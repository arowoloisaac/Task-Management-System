using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project_Manager.Model;

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

        public DbSet<OrganizationRole> OrganizationsRoles { get; set; }

        public DbSet<ProjectRole> ProjectRoles { get; set; }

        public DbSet<Comment> Comments { get; set; }
    }
}
