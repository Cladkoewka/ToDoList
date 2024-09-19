using Serilog;
using ToDoList.Application.DTOs.Task;
using ToDoList.Application.Services.Interfaces;
using ToDoList.Application.Services.Mapping;
using ToDoList.Domain.Interfaces;

namespace ToDoList.Application.Services.Implementations;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly ITagRepository _tagRepository;
    private readonly ITaskMapper _taskMapper;
    private readonly ILogger _logger;

    public TaskService(ITaskRepository taskRepository, ITagRepository tagRepository, ITaskMapper taskMapper)
    {
        _taskRepository = taskRepository;
        _tagRepository = tagRepository;
        _taskMapper = taskMapper;
        _logger = Log.ForContext<TaskService>();
    }
    
    public async Task<TaskGetDto?> GetTaskByIdAsync(int id)
    {
        _logger.Information("Fetching task with ID {TaskId}", id);
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null)
        {
            _logger.Warning("Task with ID {TaskId} not found", id);
            return null;
        }

        _logger.Information("Task with ID {TaskId} fetched successfully", id);
        return _taskMapper.MapToGetDto(task);
    }

    public async Task<IEnumerable<TaskGetDto>> GetAllTasksAsynt()
    {
        _logger.Information("Fetching all Tasks");
        var tasks = await _taskRepository.GetAllAsync();
        _logger.Information("Fetched {TaskCount} tasks", tasks.Count());
        return tasks.Select(_taskMapper.MapToGetDto).ToList();
    }
    
    public async Task<IEnumerable<TaskGetDto>> GetTasksByTagsAsync(IEnumerable<int> tagIds)
    {
        _logger.Information("Fetching tasks for tags: {TagIds}", string.Join(", ", tagIds));
        var tasks = await _taskRepository.GetByTagsAsync(tagIds);
        _logger.Information("Fetched {TaskCount} tasks for tags {TagIds}", tasks.Count(), string.Join(", ", tagIds));
        return tasks.Select(_taskMapper.MapToGetDto).ToList();
    }

    public async Task<TaskGetDto?> CreateTaskAsync(TaskCreateDto taskDto)
    {
        _logger.Information("Creating new task");
        var tags = await _tagRepository.GetTagsByIdsAsync(taskDto.TagIds);
        var task = _taskMapper.MapToEntity(taskDto, tags);
        await _taskRepository.AddAsync(task);
        
        _logger.Information("Task created successfully with ID {TaskId}", task.Id);
        return _taskMapper.MapToGetDto(task);
    }

    public async Task<bool> UpdateTaskAsync(int id, TaskUpdateDto taskDto)
    {
        _logger.Information("Updating task with ID {TaskId}", id);
        var existingTask = await _taskRepository.GetByIdAsync(id);
        if (existingTask == null)
        {
            _logger.Warning("Task with ID {TaskId} not found for update", id);
            return false;
        }

        var tags = await _tagRepository.GetTagsByIdsAsync(taskDto.TagIds);
        existingTask = _taskMapper.MapToEntity(taskDto, existingTask, tags);
        await _taskRepository.UpdateAsync(existingTask);
        _logger.Information("Task with ID {TaskId} updated successfully", id);
        return true;
    }

    public async Task<bool> DeleteTaskAsync(int id)
    {
        _logger.Information("Deleting task with ID {TaskId}", id);
        var existingTask = await _taskRepository.GetByIdAsync(id);
        if (existingTask == null)
        {
            _logger.Warning("Task with ID {TaskId} not found for deletion", id);
            return false;
        }

        await _taskRepository.DeleteAsync(existingTask);
        _logger.Information("Task with ID {TaskId} deleted successfully", id);
        return true;
    }
}