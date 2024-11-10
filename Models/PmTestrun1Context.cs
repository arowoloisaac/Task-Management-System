using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Project_Manager.Models;

public partial class PmTestrun1Context : DbContext
{
    public PmTestrun1Context()
    {
    }

    public PmTestrun1Context(DbContextOptions<PmTestrun1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<GroupUser> GroupUsers { get; set; }

    public virtual DbSet<Issue> Issues { get; set; }

    public virtual DbSet<Note> Notes { get; set; }

    public virtual DbSet<Organization> Organizations { get; set; }

    public virtual DbSet<OrganizationUser> OrganizationUsers { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Wiki> Wikis { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=pm-testrun-1;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.GroupId, "IX_AspNetUsers_GroupId");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasOne(d => d.Group).WithMany(p => p.AspNetUsers).HasForeignKey(d => d.GroupId);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasIndex(e => e.IssueId, "IX_Comments_IssueId");

            entity.HasIndex(e => e.UserId, "IX_Comments_UserId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Issue).WithMany(p => p.Comments).HasForeignKey(d => d.IssueId);

            entity.HasOne(d => d.User).WithMany(p => p.Comments).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasIndex(e => e.OrganizationId, "IX_Groups_OrganizationId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Organization).WithMany(p => p.Groups).HasForeignKey(d => d.OrganizationId);
        });

        modelBuilder.Entity<GroupUser>(entity =>
        {
            entity.HasIndex(e => e.GroupId, "IX_GroupUsers_GroupId");

            entity.HasIndex(e => e.UserId, "IX_GroupUsers_UserId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Group).WithMany(p => p.GroupUsers).HasForeignKey(d => d.GroupId);

            entity.HasOne(d => d.User).WithMany(p => p.GroupUsers).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Issue>(entity =>
        {
            entity.HasIndex(e => e.ParentIssueId, "IX_Issues_ParentIssueId");

            entity.HasIndex(e => e.ProjectId, "IX_Issues_ProjectId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.ParentIssue).WithMany(p => p.InverseParentIssue).HasForeignKey(d => d.ParentIssueId);

            entity.HasOne(d => d.Project).WithMany(p => p.Issues).HasForeignKey(d => d.ProjectId);
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasIndex(e => e.IssueId, "IX_Notes_IssueId");

            entity.HasIndex(e => e.UsersId, "IX_Notes_UsersId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Issue).WithMany(p => p.Notes).HasForeignKey(d => d.IssueId);

            entity.HasOne(d => d.Users).WithMany(p => p.Notes).HasForeignKey(d => d.UsersId);
        });

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<OrganizationUser>(entity =>
        {
            entity.ToTable("OrganizationUser");

            entity.HasIndex(e => e.OrganizationId, "IX_OrganizationUser_OrganizationId");

            entity.HasIndex(e => e.RoleId, "IX_OrganizationUser_RoleId");

            entity.HasIndex(e => e.UserId, "IX_OrganizationUser_UserId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Organization).WithMany(p => p.OrganizationUsers).HasForeignKey(d => d.OrganizationId);

            entity.HasOne(d => d.Role).WithMany(p => p.OrganizationUsers).HasForeignKey(d => d.RoleId);

            entity.HasOne(d => d.User).WithMany(p => p.OrganizationUsers).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasIndex(e => e.CreatorId, "IX_Projects_CreatorId");

            entity.HasIndex(e => e.GroupId, "IX_Projects_GroupId");

            entity.HasIndex(e => e.OrganizationId, "IX_Projects_OrganizationId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Creator).WithMany(p => p.Projects).HasForeignKey(d => d.CreatorId);

            entity.HasOne(d => d.Group).WithMany(p => p.Projects).HasForeignKey(d => d.GroupId);

            entity.HasOne(d => d.Organization).WithMany(p => p.Projects).HasForeignKey(d => d.OrganizationId);
        });

        modelBuilder.Entity<Wiki>(entity =>
        {
            entity.HasIndex(e => e.ProjectId, "IX_Wikis_ProjectId");

            entity.HasIndex(e => e.UserId, "IX_Wikis_UserId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Project).WithMany(p => p.Wikis).HasForeignKey(d => d.ProjectId);

            entity.HasOne(d => d.User).WithMany(p => p.Wikis).HasForeignKey(d => d.UserId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
