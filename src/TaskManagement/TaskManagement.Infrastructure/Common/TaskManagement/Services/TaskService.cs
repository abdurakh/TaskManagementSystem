using FluentValidation;
using System.Linq.Expressions;
using TaskManagement.Application.Common.TaskManagement.Services;
using TaskManagement.Domain.Common.Filtering.Models;
using TaskManagement.Domain.Entities;
using TaskManagement.Persistence.Repositories.Interfaces;

namespace TaskManagement.Infrastructure.Common.TaskManagement.Services;

public class TaskService(ITaskRepository taskRepository, IValidator<TaskModel> validator) : ITaskService
{
    public async ValueTask<TaskModel> CreateAsync(TaskModel taskModel, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        Validate(taskModel);
        return await taskRepository.CreateAsync(taskModel, saveChanges, cancellationToken);
    }

    public IQueryable<TaskModel> Get(Expression<Func<TaskModel, bool>>? predicate = null, FilterModel? filterModel = null, bool asNoTracking = false)
        => taskRepository.Get(predicate, filterModel, asNoTracking);

    public ValueTask<TaskModel?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default)
        => taskRepository.GetByIdAsync(id, asNoTracking, cancellationToken);

    public async ValueTask<TaskModel> UpdateAsync(TaskModel taskModel, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        Validate(taskModel);
        return await taskRepository.UpdateAsync(taskModel, saveChanges, cancellationToken);
    }

    public ValueTask<TaskModel> DeleteAsync(TaskModel taskModel, bool saveChanges = true, CancellationToken cancellationToken = default)
        => taskRepository.DeleteAsync(taskModel, saveChanges, cancellationToken);

    public ValueTask<TaskModel> DeleteByIdAsync(Guid id, bool saveChanges = true, CancellationToken cancellationToken = default)
        => taskRepository.DeleteByIdAsync(id, saveChanges, cancellationToken);

    private void Validate(TaskModel taskModel)
    {
        var validationResult = validator.Validate(taskModel);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
    }
}
