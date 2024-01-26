namespace TaskManagement.Domain.Common.Models;

public abstract class Entity : IEntity
{
    public Guid Id { get; set; }
}
