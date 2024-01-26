using TaskManagement.Domain.Common.Enums;
using TaskManagement.Domain.Common.Models;

namespace TaskManagement.Domain.Entities;

public class TaskModel : AuditableEntity
{
    public string Title { get; set; } = default!;

    public string? Description { get; set; }

    public DateTime DueDate { get; set; }

    public bool IsCompleted { get; set; }

    public PriorityLevel Priority { get; set; }
}
