using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Common.Models;

namespace TaskManagement.Persistence.Interceptors;

public class UpdateAuditableInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
            UpdateAuditableEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateAuditableEntities(DbContext context)
    {
        var entityEntries = context.ChangeTracker.Entries<IAuditableEntity>().ToList();

        foreach (var entityEntry in entityEntries)
        {
            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Property(nameof(AuditableEntity.Id)).CurrentValue = Guid.NewGuid();
                entityEntry.Property(nameof(AuditableEntity.CreatedTime)).CurrentValue = DateTime.UtcNow;
            }

            if (entityEntry.State == EntityState.Deleted)
            {
                entityEntry.Property(nameof(AuditableEntity.IsDeleted)).CurrentValue = true;
                entityEntry.Property(nameof(AuditableEntity.DeletedTime)).CurrentValue = DateTime.UtcNow;
                entityEntry.State = EntityState.Modified;
            }

            if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property(nameof(AuditableEntity.ModifiedTime)).CurrentValue = DateTime.UtcNow;
            }
        }
    }
}

