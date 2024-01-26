namespace TaskManagement.Domain.Common.Models;

public interface IAuditableEntity : IEntity
{
    DateTime CreatedTime { get; set; }

    DateTime? ModifiedTime { get; set; }
}
