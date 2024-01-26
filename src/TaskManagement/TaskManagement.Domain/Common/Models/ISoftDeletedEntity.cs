namespace TaskManagement.Domain.Common.Models;

public interface ISoftDeletedEntity : IEntity
{
    bool IsDeleted { get; set; }

    DateTime? DeletedTime { get; set; }
}
