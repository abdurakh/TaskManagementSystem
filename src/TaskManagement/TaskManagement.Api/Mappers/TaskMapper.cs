using AutoMapper;
using TaskManagement.Api.Models.Dtos;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Api.Mappers;

public class TaskMapper : Profile
{
    public TaskMapper()
    {
        CreateMap<TaskModel, TaskModelDto>().ReverseMap();
    }
}
