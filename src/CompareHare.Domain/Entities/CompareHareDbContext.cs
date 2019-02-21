#region usings

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using CompareHare.Domain.Services;
using System;

#endregion

namespace CompareHare.Domain.Entities
{
    public class CompareHareDbContext : IdentityDbContext<User, Role, int>
    {
        public CompareHareDbContext(
            DbContextOptions<CompareHareDbContext> dbContextOptions) : base(dbContextOptions) {}

        public DbSet<Alert> Alerts { get; set; }
        public DbSet<AlertCriteria> AlertCriteria { get; set; }
        public DbSet<PendingAlertNotification> PendingAlertNotifications { get; set; }
        public DbSet<StateUtilityIndex> StateUtilityIndices { get; set; }
        public DbSet<UtilityPrice> UtilityPrices { get; set; }
        public DbSet<UtilityPriceHistory> UtilityPriceHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var userBuilder = modelBuilder.Entity<User>();
            userBuilder.Property(u => u.FullAccessGrantedDate).HasColumnType("date");
            userBuilder.HasIndex(u => u.Email).IsUnique();

            modelBuilder.Entity<AlertUtilityPriceHistory>()
              .HasKey(x => new {x.AlertId, x.UtilityPriceHistoryId});

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            AddTimestamps();
            return (await base.SaveChangesAsync(true, cancellationToken));
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

                if(entity.State == EntityState.Modified && entity.Entity is IModifiedDateTimeTracker)
                    ((IModifiedDateTimeTracker)entity.Entity).ModifiedDate = DateTime.UtcNow;
            }
        }
    }
}
