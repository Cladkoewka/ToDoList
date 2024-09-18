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

    public TaskService(ITaskRepository taskRepository, ITagRepository tagRepository, ITaskMapper taskMapper)
    {
        _taskRepository = taskRepository;
        _tagRepository = tagRepository;
        _taskMapper = taskMapper;
    }
    
    public async Task<TaskGetDto?> GetTaskByIdAsync(int id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        return task != null ? _taskMapper.MapToGetDto(task) : null;
    }

    public async Task<IEnumerable<TaskGetDto>> GetAllTasksAsynt()
    {
        var tasks = await _taskRepository.GetAllAsync();
        return tasks.Select(_taskMapper.MapToGetDto).ToList();
    }
    
    public async Task<IEnumerable<TaskGetDto>> GetTasksByTagsAsync(IEnumerable<int> tagIds)
    {
        var tasks = await _taskRepository.GetByTagsAsync(tagIds);
        return tasks.Select(_taskMapper.MapToGetDto).ToList();
    }

    public async Task<TaskGetDto?> CreateTaskAsync(TaskCreateDto taskDto)
    {
        var tags = await _tagRepository.GetTagsByIdsAsync(taskDto.TagIds);
        var task = _taskMapper.MapToEntity(taskDto, tags);
        await _taskRepository.AddAsync(task);

        return _taskMapper.MapToGetDto(task);
    }

    public async Task<bool> UpdateTaskAsync(int id, TaskUpdateDto taskDto)
    {
        var existingTask = await _taskRepository.GetByIdAsync(id);
        if (existingTask == null)
            return false;

        var tags = await _tagRepository.GetTagsByIdsAsync(taskDto.TagIds);

        existingTask = _taskMapper.MapToEntity(taskDto, existingTask, tags);
        await _taskRepository.UpdateAsync(existingTask);

        return true;
    }

    public async Task<bool> DeleteTaskAsync(int id)
    {
        var existingTask = await _taskRepository.GetByIdAsync(id);
        if (existingTask == null)
            return false;

        await _taskRepository.DeleteAsync(existingTask);

        return true;
    }
}