using ToDoList.Application.DTOs.Task;
using ToDoList.Application.Services.Interfaces;
using ToDoList.Application.Services.Mapping;
using ToDoList.Domain.Interfaces;

namespace ToDoList.Application.Services.Implementations;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly ITaskMapper _taskMapper;

    public TaskService(ITaskRepository taskRepository, ITaskMapper taskMapper)
    {
        _taskRepository = taskRepository;
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

    public async Task<TaskGetDto?> CreateTaskAsync(TaskCreateDto taskDto)
    {
        var task = _taskMapper.MapToEntity(taskDto);
        await _taskRepository.AddAsync(task);

        return _taskMapper.MapToGetDto(task);
    }

    public async Task<bool> UpdateTaskAsync(int id, TaskUpdateDto taskDto)
    {
        var existingTask = await _taskRepository.GetByIdAsync(id);
        if (existingTask == null)
            return false;

        existingTask = _taskMapper.MapToEntity(taskDto, existingTask);
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