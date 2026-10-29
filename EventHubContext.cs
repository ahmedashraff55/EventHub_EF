using EventHub.Configurations;
using EventHub.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHub;

public class EventHubContext : DbContext
{
    public DbSet<Organizer> Organizers => Set<Organizer>();
    public DbSet<OrganizerProfile> OrganizerProfiles => Set<OrganizerProfile>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<Attendee> Attendees => Set<Attendee>();
    public DbSet<Badge> Badges => Set<Badge>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
                @"Server=(localdb)\MSSQLLocalDB;Database=EventHubDb;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // OrganizerProfile: Fluent API configured directly here.
        modelBuilder.Entity<OrganizerProfile>(entity =>
        {
            entity.HasKey(p => p.OrganizerProfileId);
            entity.Property(p => p.Biography).IsRequired().HasMaxLength(1000);
            entity.Property(p => p.WebsiteUrl).HasMaxLength(300);
            entity.Property(p => p.LogoUrl).HasMaxLength(300);

            entity.HasOne(p => p.Organizer)
                .WithOne(o => o.Profile)
                .HasForeignKey<OrganizerProfile>(p => p.OrganizerId)
                .IsRequired();

            entity.HasIndex(p => p.OrganizerId).IsUnique();
        });

        // Badge: Fluent API configured directly here.
        modelBuilder.Entity<Badge>(entity =>
        {
            entity.HasKey(b => b.BadgeId);
            entity.Property(b => b.BadgeNumber).IsRequired().HasMaxLength(50);
            entity.Property(b => b.IssuedDate).IsRequired();
            entity.Property(b => b.Tier).IsRequired();

            entity.HasOne(b => b.Attendee)
                .WithOne(a => a.Badge)
                .HasForeignKey<Badge>(b => b.AttendeeId)
                .IsRequired();

            entity.HasIndex(b => b.AttendeeId).IsUnique();
            entity.HasIndex(b => b.BadgeNumber).IsUnique();
        });

        // Event: Fluent API configuration is in a separate class.
        modelBuilder.ApplyConfiguration(new EventConfiguration());
    }
}
