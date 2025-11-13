using System.Linq.Expressions;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Domain.Database;

public class DatabaseContext : IdentityDbContext<AppUser>
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }
    
    public DbSet<Car> Car { get; set; }
    public DbSet<Booking> Booking { get; set; }
    public DbSet<Review> Review { get; set; }
    public DbSet<SupportTicket> SupportTicket { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e is { Entity: BaseEntity, State: EntityState.Modified });

        foreach (var entityEntry in entries)
            ((BaseEntity) entityEntry.Entity).UpdatedAt = DateTime.UtcNow;

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<Car>()
            .HasOne(c => c.Owner)
            .WithMany(u => u.Cars)
            .HasForeignKey(c => c.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Entity<Booking>()
            .HasOne(b => b.Car)
            .WithMany(c => c.Bookings)
            .HasForeignKey(b => b.CarId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Entity<Booking>()
            .HasOne(b => b.Renter)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.RenterId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Entity<Review>()
            .HasOne(r => r.Booking)
            .WithOne(b => b.Review)
            .HasForeignKey<Review>(r => r.BookingId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<Review>()
            .HasOne(r => r.Author)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Entity<SupportTicket>()
            .HasOne(t => t.CreatedBy)
            .WithMany(u => u.SupportTicketsCreated)
            .HasForeignKey(t => t.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Entity<SupportTicket>()
            .HasOne(t => t.AssignedTo)
            .WithMany(u => u.SupportTicketsAssigned)
            .HasForeignKey(t => t.AssignedToId)
            .OnDelete(DeleteBehavior.SetNull);


        builder.Entity<Car>()
            .HasIndex(c => new { c.City, c.Brand, c.PricePerDay });

        builder.Entity<Booking>()
            .HasIndex(b => b.Status);

        builder.Entity<SupportTicket>()
            .HasIndex(t => t.Status);
        
        List<IdentityRole> roles = new List<IdentityRole>{
            new IdentityRole{Name = "Admin", NormalizedName = "ADMIN"},
            new IdentityRole{Name = "User", NormalizedName = "USER"},
            new IdentityRole{Name = "Owner", NormalizedName = "OWNER"},
            new IdentityRole { Name = "Support", NormalizedName = "SUPPORT" }
        };

        builder.Entity<IdentityRole>().HasData(roles);
        
        builder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
        
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            AddEntityQueryFilter(builder, entityType);
            
            foreach (var mutableForeignKey in entityType.GetForeignKeys())
                mutableForeignKey.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }

    private static void AddEntityQueryFilter(ModelBuilder builder, IReadOnlyTypeBase entityType)
    {
        var type = entityType.ClrType;
        if (type.IsSubclassOf(typeof(BaseEntity)))  //&& type != typeof(UserToken)
        {
            var parameter = Expression.Parameter(type);
            var propertyInfo = Expression.Property(parameter, "DeletedAt");
            var nullConstant = Expression.Constant(null, typeof(DateTime?));
            var equalExpression = Expression.Equal(propertyInfo, nullConstant);
            var filter = Expression.Lambda(equalExpression, parameter);
            builder.Entity(type).HasQueryFilter(filter).HasIndex("DeletedAt");
        }
    }
}