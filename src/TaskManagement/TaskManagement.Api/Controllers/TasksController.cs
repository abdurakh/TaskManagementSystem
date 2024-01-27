using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Api.Models.Dtos;
using TaskManagement.Application.Common.TaskManagement.Services;
using TaskManagement.Domain.Common.Filtering.Models;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController(ITaskService taskService, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Get a list of tasks
    /// </summary>
    /// <param name="filterModel">filter model containing pagination options for the tasks list</param>
    [HttpGet]
    public async ValueTask<IActionResult> Get([FromQuery] FilterModel filterModel)
    {
        var result = taskService.Get(filterModel: filterModel, asNoTracking: true);

        return result.Any() ? Ok(mapper.Map<List<TaskModelDto>>(result)) : NoContent();
    }

    /// <summary>
    /// Get details of a specific task by Id
    /// </summary>
    /// <param name="id">The Id of the task</param>
    [HttpGet("{id:guid}")]
    public async ValueTask<IActionResult> GetById([FromRoute] Guid id)
    {
        var result = await taskService.GetByIdAsync(id, true, HttpContext.RequestAborted);
        return result is not null ? Ok(mapper.Map<TaskModelDto>(result)) : NotFound();
    }

    /// <summary>
    /// Create a new task
    /// </summary>
    /// <param name="taskModelDto">the details of the new task to be created</param>
    [HttpPost]
    public async ValueTask<IActionResult> Create([FromBody] TaskModelDto taskModelDto)
    {
        var result = await taskService.CreateAsync(mapper.Map<TaskModel>(taskModelDto), cancellationToken: HttpContext.RequestAborted);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, mapper.Map<TaskModelDto>(result));
    }

    /// <summary>
    /// Update an existing task
    /// </summary>
    /// <param name="taskModelDto">The updated details of the task</param>
    [HttpPut]
    public async ValueTask<IActionResult> Update(TaskModelDto taskModelDto)
    {
        var result = await taskService.UpdateAsync(mapper.Map<TaskModel>(taskModelDto), cancellationToken: HttpContext.RequestAborted);
        return result is not null ? Ok(mapper.Map<TaskModelDto>(result)) : BadRequest();
    }

    /// <summary>
    /// Delete an existing  task
    /// </summary>
    /// <param name="taskModelDto">Details of the task to delete</param>
    [HttpDelete]
    public async ValueTask<IActionResult> Delete([FromBody] TaskModelDto taskModelDto)
    {
        var result = await taskService.DeleteAsync(mapper.Map<TaskModel>(taskModelDto), cancellationToken: HttpContext.RequestAborted);
        return result is not null ? Ok(mapper.Map<TaskModelDto>(result)) : BadRequest();
    }

    /// <summary>
    /// Delete an existing task
    /// </summary>
    /// <param name="id">The id of task to be delete</param>
    [HttpDelete("{id:guid}")]
    public async ValueTask<IActionResult> DeleteById([FromRoute] Guid id)
    {
        var result = await taskService.DeleteByIdAsync(id, cancellationToken: HttpContext.RequestAborted);
        return result is not null ? Ok(mapper.Map<TaskModelDto>(result)) : BadRequest();
    }
}
