using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManagement.Domain.Common.Exceptions;
using TaskManagement.Domain.Common.Models;
using TaskManagement.Persistence.Caching.Brokers;

public class EntityRepositoryBase<TEntity, TContext>(
    TContext dbContext,
    ICacheBroker cacheBroker)
    where TEntity : class, IEntity where TContext : DbContext
{
    protected TContext DbContext => dbContext;

    protected IQueryable<TEntity> Get(
        Expression<Func<TEntity, bool>>? predicate = default,
        bool asNoTracking = false)
    {
        var initialQuery = DbContext.Set<TEntity>().AsQueryable();

        if (predicate is not null)
            initialQuery = initialQuery.Where(predicate);

        if (asNoTracking)
            initialQuery = initialQuery.AsNoTracking();

        return initialQuery;
    }

    protected ValueTask<TEntity?> GetByIdAsync(
        Guid id,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default)
    {
        return cacheBroker.GetOrSetAsync<TEntity>(
            id.ToString(),
            async () =>
            {
                var initialQuery = DbContext.Set<TEntity>().AsQueryable();

                if (asNoTracking)
                    initialQuery = initialQuery.AsNoTracking();

                var foundEntity = await initialQuery.FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken)
                    ?? throw new EntityNotFoundException(typeof(TEntity));

                return foundEntity;
            }
        );
    }

    protected async ValueTask<TEntity> CreateAsync(
        TEntity entity,
        bool saveChanges = true,
        CancellationToken cancellationToken = default)
    {
        await DbContext.Set<TEntity>().AddAsync(entity, cancellationToken);

        await cacheBroker.SetAsync(entity.Id.ToString(), entity);

        if (saveChanges) await DbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }

    protected async ValueTask<TEntity> UpdateAsync(
        TEntity entity,
        bool saveChanges = true,
        CancellationToken cancellationToken = default
    )
    {
        DbContext.Set<TEntity>().Update(entity);

        await cacheBroker.SetAsync(entity.Id.ToString(), entity, cancellationToken);

        if (saveChanges) await DbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }

    protected async ValueTask<TEntity> DeleteAsync(
        TEntity entity,
        bool saveChanges = true,
        CancellationToken cancellationToken = default
    )
    {
        var foundEntity = await DbContext.Set<TEntity>().FirstOrDefaultAsync(dbEntity => dbEntity.Id == entity.Id, cancellationToken: cancellationToken)
            ?? throw new EntityNotFoundException(typeof(TEntity));

        DbContext.Set<TEntity>().Remove(foundEntity);

        await cacheBroker.DeleteAsync(foundEntity.Id.ToString(), cancellationToken);

        if (saveChanges) await DbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }

    protected async ValueTask<TEntity> DeleteByIdAsync(
        Guid id,
        bool saveChanges = true,
        CancellationToken cancellationToken = default
    )
    {
        var foundEntity = DbContext.Set<TEntity>().FirstOrDefault(entity => entity.Id == id)
            ?? throw new EntityNotFoundException(typeof(TEntity));

        DbContext.Set<TEntity>().Remove(foundEntity);

        await cacheBroker.DeleteAsync(id.ToString(), cancellationToken);

        if (saveChanges) await DbContext.SaveChangesAsync(cancellationToken);

        return foundEntity;
    }
}
