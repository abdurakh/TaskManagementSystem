using FluentValidation;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Common.Validators;

public class TaskValidator : AbstractValidator<TaskModel>
{
    public TaskValidator()
    {
        RuleFor(task => task.Title)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(256);

        RuleFor(task => task.Description)
            .MaximumLength(1024);

        RuleFor(task => task.DueDate)
            .Custom((dueDate, context) =>
            {
                if(dueDate < DateTime.Now)
                    context.AddFailure(nameof(dueDate), "the due date cannot be less than the current date");
            });
    }
}
