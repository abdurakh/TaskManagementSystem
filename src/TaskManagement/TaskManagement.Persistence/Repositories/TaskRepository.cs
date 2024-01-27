using System.Linq.Expressions;
using TaskManagement.Domain.Common.Exceptions;
using TaskManagement.Domain.Common.Filtering.Models;
using TaskManagement.Domain.Entities;
using TaskManagement.Persistence.Caching.Brokers;
using TaskManagement.Persistence.DbContexts;
using TaskManagement.Persistence.Repositories.Interfaces;

namespace TaskManagement.Persistence.Repositories;

public class TaskRepository(AppDbContext dbContext, ICacheBroker cacheBroker)
    : EntityRepositoryBase<TaskModel, AppDbContext>(dbContext, cacheBroker), ITaskRepository
{
    public new ValueTask<TaskModel> CreateAsync(TaskModel taskModel, bool saveChanges = true, CancellationToken cancellationToken = default)
        => base.CreateAsync(taskModel, saveChanges, cancellationToken);

    public new IQueryable<TaskModel> Get(Expression<Func<TaskModel, bool>>? predicate = null, FilterModel? filterModel = default, bool asNoTracking = false)
        => base.Get(predicate, filterModel, asNoTracking);

    public new ValueTask<TaskModel?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default)
        => base.GetByIdAsync(id, asNoTracking, cancellationToken);

    public new ValueTask<TaskModel> UpdateAsync(TaskModel taskModel, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        var foundTask = dbContext.Tasks.FirstOrDefault(dbTask => dbTask.Id == taskModel.Id)
            ?? throw new EntityNotFoundException(typeof(TaskModel));

        foundTask.Title = taskModel.Title;
        foundTask.Description = taskModel.Description;
        foundTask.DueDate = taskModel.DueDate;
        foundTask.IsCompleted = taskModel.IsCompleted;
        foundTask.Priority = taskModel.Priority;

        return base.UpdateAsync(foundTask, saveChanges, cancellationToken);
    }

    public new ValueTask<TaskModel> DeleteAsync(TaskModel taskModel, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        var foundTask = dbContext.Tasks.FirstOrDefault(task => task.Id == taskModel.Id)
            ?? throw new EntityNotFoundException(typeof(TaskModel));

        return base.DeleteAsync(foundTask, saveChanges, cancellationToken);
    }

    public new ValueTask<TaskModel> DeleteByIdAsync(Guid id, bool saveChanges = true, CancellationToken cancellationToken = default)
        => base.DeleteByIdAsync(id, saveChanges, cancellationToken);
}
