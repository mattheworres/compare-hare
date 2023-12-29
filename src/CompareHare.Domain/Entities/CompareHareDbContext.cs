using CompareHare.Domain.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CompareHare.Domain.Entities
{
    public class CompareHareDbContext : IdentityDbContext<User, Role, int>
    {
        public CompareHareDbContext(DbContextOptions<CompareHareDbContext> options) : base(options) { }

        public DbSet<AlertMatch> AlertMatches { get; set; }
        public DbSet<Alert> Alerts { get; set; }
        public DbSet<PendingAlertNotification> PendingAlertNotifications { get; set; }
        public DbSet<ProductRetailerPrice> ProductRetailerPrices { get; set; }
        public DbSet<ProductRetailerPriceHistory> ProductRetailerPriceHistories { get; set; }
        public DbSet<ProductPriceScrapingException> ProductPriceScrapingExceptions { get; set; }
        public DbSet<StateUtilityIndex> StateUtilityIndices { get; set; }
        public DbSet<TrackedProduct> TrackedProducts { get; set; }
        public DbSet<TrackedProductRetailer> TrackedProductRetailers { get; set; }
        public DbSet<UtilityPrice> UtilityPrices { get; set; }
        public DbSet<UtilityPriceHistory> UtilityPriceHistories { get; set; }

        // public DbSet<AlertMatchUtilityPriceHistory> AlertMatchUtilityPriceHistories { get; set; }

        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken ct = default(CancellationToken))
        {
            AddTimestamps();
            return await base.SaveChangesAsync(true, ct);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            AddTimestamps();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        private void AddTimestamps()
        {
            var dateTrackingEntities = ChangeTracker.Entries().Where(x =>
                (x.Entity is ICreatedDateTimeTracker || x.Entity is IModifiedDateTimeTracker)
                && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in dateTrackingEntities)
            {
                if (entity.State == EntityState.Added && entity.Entity is ICreatedDateTimeTracker)
                    ((ICreatedDateTimeTracker)entity.Entity).CreatedDate = DateTime.UtcNow;

                if (entity.State == EntityState.Modified && entity.Entity is IModifiedDateTimeTracker)
                    ((IModifiedDateTimeTracker)entity.Entity).ModifiedDate = DateTime.UtcNow;
            }
        }
    }
}
