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
    
    public async Task<TaskGetDto?> GetTaskByIdAsync(int id, int userId)
    {
        _logger.Information("Fetching task with ID {TaskId}", id);
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null || task.UserId != userId)
        {
            _logger.Warning("Task with ID {TaskId} not found or does not belong to user {UserId}", id, userId);
            return null;
        }

        _logger.Information("Task with ID {TaskId} fetched successfully for user {UserId}", id, userId);
        return _taskMapper.MapToGetDto(task);
    }

    public async Task<IEnumerable<TaskGetDto>> GetAllTasksAsync(int userId)
    {
        _logger.Information("Fetching all tasks for user {UserId}", userId);
        var tasks = await _taskRepository.GetAllByUserIdAsync(userId);
        _logger.Information("Fetched {TaskCount} tasks for user {UserId}", tasks.Count(), userId);

        return tasks.Select(_taskMapper.MapToGetDto).ToList();
    }
    
    public async Task<PaginatedResult<TaskGetDto>> GetPaginatedTasksAsync(int pageNumber, int pageSize,
        int userId)
    {
        _logger.Information("Fetching paginated tasks for user {UserId}", userId);
        var allTasksCount = await _taskRepository.GetTaskCount(userId);
        var tasks = await _taskRepository.GetPaginatedTasksAsync(pageNumber, pageSize, userId);
        _logger.Information("Fetched {TaskCount} tasks for user {UserId}", tasks.Count(), userId);
        return new PaginatedResult<TaskGetDto>()
        {
            Tasks = tasks.Select(_taskMapper.MapToGetDto).ToList(),
            TotalCount = allTasksCount
        };
    }
    
    public async Task<PaginatedResult<TaskGetDto>> GetFilteredTasksAsync(int pageNumber, int pageSize,
        bool showCompleted, int userId)
    {
        _logger.Information("Fetching filtered Tasks");
        if (showCompleted)
        {
            var allTasksCount = await _taskRepository.GetTaskCount(userId);
            var tasks = await _taskRepository.GetPaginatedTasksAsync(pageNumber, pageSize, userId);
            _logger.Information("Fetched {TaskCount} filtered tasks for user {UserId}", tasks.Count(), userId);
            return new PaginatedResult<TaskGetDto>()
            {
                Tasks = tasks.Select(_taskMapper.MapToGetDto).ToList(),
                TotalCount = allTasksCount
            };
        }
        else
        {
            var notCompletedTasksCount = await _taskRepository.GetTaskCount(false, userId);
            var tasks = await _taskRepository.GetPaginatedTasksAsync(pageNumber, pageSize, false, userId);
            return new PaginatedResult<TaskGetDto>()
            {
                Tasks = tasks.Select(_taskMapper.MapToGetDto).ToList(),
                TotalCount = notCompletedTasksCount
            };
        }
    }
    
    // redudant
    public async Task<IEnumerable<TaskGetDto>> GetTasksByTagsAsync(IEnumerable<int> tagIds)
    {
        _logger.Information("Fetching tasks for tags: {TagIds}", string.Join(", ", tagIds));
        var tasks = await _taskRepository.GetByTagsAsync(tagIds);
        _logger.Information("Fetched {TaskCount} tasks for tags {TagIds}", tasks.Count(), string.Join(", ", tagIds));
        return tasks.Select(_taskMapper.MapToGetDto).ToList();
    }

    public async Task<TaskGetDto?> CreateTaskAsync(TaskCreateDto taskDto, int userId)
    {
        _logger.Information("Creating new task");
        var tags = await _tagRepository.GetTagsByIdsAsync(taskDto.TagIds);
        var task = _taskMapper.MapToEntity(taskDto, tags);
        task.UserId = userId;
        await _taskRepository.AddAsync(task);
        
        _logger.Information("Task created successfully with ID {TaskId}", task.Id);
        return _taskMapper.MapToGetDto(task);
    }

    public async Task<bool> UpdateTaskAsync(int id, TaskUpdateDto taskDto, int userId)
    {
        _logger.Information("Updating task with ID {TaskId}", id);
        var existingTask = await _taskRepository.GetByIdAsync(id);
        if (existingTask == null || existingTask.UserId != userId)
        {
            _logger.Warning("Task with ID {TaskId} not found or does not belong to user {UserId}", id, userId);
            return false;
        }

        var tags = await _tagRepository.GetTagsByIdsAsync(taskDto.TagIds);
        existingTask = _taskMapper.MapToEntity(taskDto, existingTask, tags);
        await _taskRepository.UpdateAsync(existingTask);
        _logger.Information("Task with ID {TaskId} updated successfully", id);
        return true;
    }

    public async Task<bool> DeleteTaskAsync(int id, int userId)
    {
        _logger.Information("Deleting task with ID {TaskId}", id);
        var existingTask = await _taskRepository.GetByIdAsync(id);
        if (existingTask == null || existingTask.UserId != userId)
        {
            _logger.Warning("Task with ID {TaskId} not found or does not belong to user {UserId}", id, userId);
            return false;
        }

        await _taskRepository.DeleteAsync(existingTask);
        _logger.Information("Task with ID {TaskId} deleted successfully", id);
        return true;
    }
}