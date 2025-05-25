using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Worknet.Core.Entities;

namespace Worknet.DAL;
public class WorknetDbContext : IdentityDbContext<User>
{
    public WorknetDbContext(DbContextOptions options) : base(options){}

    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<GoogleDriveFile> Files { get; set; }
    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries<DbItem>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }

            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
            }
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Profile>()
        .HasMany(p => p.Skills)
        .WithOne(s => s.Profile)
        .HasForeignKey(s => s.ProfileId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Profile>()
        .HasMany(p => p.Files)
        .WithOne(s => s.Profile)
        .HasForeignKey(s => s.ProfileId)
        .OnDelete(DeleteBehavior.SetNull);
    }
}