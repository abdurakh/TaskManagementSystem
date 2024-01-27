using System.Linq.Expressions;
using TaskManagement.Domain.Common.Filtering.Models;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Persistence.Repositories.Interfaces;

public interface ITaskRepository
{
    ValueTask<TaskModel> CreateAsync(TaskModel taskModel, bool saveChanges = true, CancellationToken cancellationToken = default);

    IQueryable<TaskModel> Get(Expression<Func<TaskModel, bool>>? predicate = default,FilterModel? filterModel = default, bool asNoTracking = false);

    ValueTask<TaskModel?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default);

    ValueTask<TaskModel> UpdateAsync(TaskModel taskModel, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<TaskModel> DeleteAsync(TaskModel taskModel, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<TaskModel> DeleteByIdAsync(Guid id, bool saveChanges = true, CancellationToken cancellationToken = default);
}