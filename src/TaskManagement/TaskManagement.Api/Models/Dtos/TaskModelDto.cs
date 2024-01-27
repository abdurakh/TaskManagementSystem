using AutoMapper;
using TaskManagement.Domain.Common.Enums;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Api.Models.Dtos;

public class TaskModelDto
{
    public string Title { get; set; } = default!;

    public string? Description { get; set; }

    public DateTime DueDate { get; set; }

    public bool IsCompleted { get; set; }

    public PriorityLevel Priority { get; set; }
}
