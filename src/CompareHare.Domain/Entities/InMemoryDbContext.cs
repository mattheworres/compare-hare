using CompareHare.Domain.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CompareHare.Domain.Entities
{
    public class InMemoryDbContext : IdentityDbContext<IdentityUser>
    {
        public InMemoryDbContext(DbContextOptions<InMemoryDbContext> options) : base(options) { }

        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken ct = default(CancellationToken))
        {
            AddTimestamps();
            return (await base.SaveChangesAsync(true, ct));
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