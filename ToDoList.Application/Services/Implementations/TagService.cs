using Serilog;
using ToDoList.Application.DTOs.Tag;
using ToDoList.Application.Services.Interfaces;
using ToDoList.Application.Services.Mapping;
using ToDoList.Domain.Interfaces;

namespace ToDoList.Application.Services.Implementations;

public class TagService : ITagService
{
    private readonly ITagRepository _tagRepository;
    private readonly ITagMapper _tagMapper;
    private readonly ILogger _logger;

    public TagService(ITagRepository tagRepository, ITagMapper tagMapper)
    {
        _tagRepository = tagRepository;
        _tagMapper = tagMapper;
        _logger = Log.ForContext<TagService>();
    }
    
    public async Task<TagGetDto?> GetTagByIdAsync(int id)
    {
        _logger.Information("Fetching tag with ID {TagId}", id);
        var tag = await _tagRepository.GetByIdAsync(id);
        if (tag == null)
        {
            _logger.Warning("Tag with ID {TagId} not found", id);
            return null;
        }

        _logger.Information("Tag with ID {TagId} fetched successfully", id);
        return _tagMapper.MapToGetDto(tag);
    }

    public async Task<IEnumerable<TagGetDto>> GetAllTagsAsync()
    {
        _logger.Information("Fetching all tags");
        var tags = await _tagRepository.GetAllAsync();
        _logger.Information("Fetched {TagCount} tags", tags.Count());
        return tags.Select(_tagMapper.MapToGetDto).ToList();
    }

    public async Task<TagGetDto?> CreateTagAsync(TagCreateDto tagDto)
    {
        _logger.Information("Creating new tag with name: {TagName}", tagDto.Name);
        var existingTag = await _tagRepository.GetByNameAsync(tagDto.Name);
        if (existingTag != null)
        {
            _logger.Information("Tag with name {TagName} already exists", tagDto.Name);
            return _tagMapper.MapToGetDto(existingTag);
        }
        
        var tag = _tagMapper.MapToEntity(tagDto);
        await _tagRepository.AddAsync(tag);
        _logger.Information("Tag with ID {TagId} created successfully", tag.Id);

        return _tagMapper.MapToGetDto(tag);
    }

    public async Task<bool> UpdateTagAsync(int id, TagUpdateDto tagDto)
    {
        _logger.Information("Updating tag with ID {TagId}", id);
        var existingTag = await _tagRepository.GetByIdAsync(id);
        if (existingTag == null)
        {
            _logger.Warning("Tag with ID {TagId} not found for update", id);
            return false;
        }

        existingTag = _tagMapper.MapToEntity(tagDto, existingTag);
        await _tagRepository.UpdateAsync(existingTag);
        _logger.Information("Tag with ID {TagId} updated successfully", id);
        return true;
    }

    public async Task<bool> DeleteTagAsync(int id)
    {
        _logger.Information("Deleting tag with ID {TagId}", id);
        var existingTag = await _tagRepository.GetByIdAsync(id);
        if (existingTag == null)
        {
            _logger.Warning("Tag with ID {TagId} not found for deletion", id);
            return false;
        }

        await _tagRepository.DeleteAsync(existingTag);
        _logger.Information("Tag with ID {TagId} deleted successfully", id);
        return true;
    }
}
