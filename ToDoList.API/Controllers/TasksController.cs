using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using ToDoList.Application.DTOs.Task;
using ToDoList.Application.Services.Interfaces;
using ILogger = Serilog.ILogger;

namespace ToDoList.API.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;
    private readonly ILogger _logger;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
        _logger = Log.ForContext<TasksController>();
    }

    private int GetCurrentUserId()
    {
        if (!Request.Headers.TryGetValue("Token", out var authToken))
        {
            throw new InvalidOperationException("Auth token is not present in the headers.");
        }

        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(authToken);
        var userIdClaim = token.Claims.FirstOrDefault(c => c.Type == "nameid");

        if (userIdClaim == null)
        {
            throw new InvalidOperationException("User ID claim is not present in the token.");
        }

        return int.Parse(userIdClaim.Value);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskGetDto>>> GetAllTasks()
    {
        var tasks = await _taskService.GetAllTasksAsync(GetCurrentUserId());
        return Ok(tasks);
    }
    
    [HttpGet("paginated")]
    public async Task<ActionResult<PaginatedResult<TaskGetDto>>> GetPaginatedTasks([FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        if (pageNumber <= 0 || pageSize <= 0)
        {
            return BadRequest("Page number and page size must be greater than 0.");
        }

        var paginatedTasks = await _taskService.GetPaginatedTasksAsync(pageNumber,
            pageSize, GetCurrentUserId());
        return Ok(paginatedTasks);
    }
    
    [HttpGet("filtered")]
    public async Task<ActionResult<PaginatedResult<TaskGetDto>>> GetPaginatedTasks(
        [FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] bool showCompleted)
    {
        if (pageNumber <= 0 || pageSize <= 0)
        {
            return BadRequest("Page number and page size must be greater than 0.");
        }

        var paginatedTasks = await _taskService.GetFilteredTasksAsync(pageNumber,
            pageSize, showCompleted, GetCurrentUserId());
        return Ok(paginatedTasks);
    }
    
    [HttpGet("by-tags")]
    public async Task<ActionResult<IEnumerable<TaskGetDto>>> GetTasksByTags([FromQuery] IEnumerable<int> tagIds)
    {
        var tasks = await _taskService.GetTasksByTagsAsync(tagIds);
        return Ok(tasks);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskGetDto>> GetTaskById(int id)
    {
        var task = await _taskService.GetTaskByIdAsync(id, GetCurrentUserId());
        if (task == null)
            return NotFound($"Task with ID {id} not found");

        return Ok(task);
    }

    [HttpPost]
    public async Task<ActionResult<TaskGetDto>> AddTask([FromBody] TaskCreateDto taskDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.Warning("Invalid model state for task creation.");
            return BadRequest(ModelState);
        }

        var createdTask = await _taskService.CreateTaskAsync(taskDto, GetCurrentUserId());

        if (createdTask == null)
            return BadRequest("Task could not be created.");

        return CreatedAtAction(nameof(GetTaskById), new { id = createdTask.Id }, createdTask);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateTask(int id, [FromBody] TaskUpdateDto taskDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.Warning("Invalid model state for task update.");
            return BadRequest(ModelState);
        }

        var success = await _taskService.UpdateTaskAsync(id, taskDto, GetCurrentUserId());
        if (!success)
            return NotFound($"Task with ID {id} not found.");

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteTask(int id)
    {
        var success = await _taskService.DeleteTaskAsync(id, GetCurrentUserId());
        if (!success)
            return NotFound($"Task with ID {id} not found.");

        return NoContent();
    }
}