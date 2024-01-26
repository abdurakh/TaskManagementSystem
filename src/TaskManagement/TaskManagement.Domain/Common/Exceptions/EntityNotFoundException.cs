namespace TaskManagement.Domain.Common.Exceptions;

public class EntityNotFoundException(Type type) : Exception($"Entity of type - {type}, not found!")
{
}
