using ToDoList.Application.DTOs.Tag;
using ToDoList.Application.Services.Interfaces;
using ToDoList.Application.Services.Mapping;
using ToDoList.Domain.Interfaces;

namespace ToDoList.Application.Services.Implementations;

public class TagService : ITagService
{
    private readonly ITagRepository _tagRepository;
    private readonly ITagMapper _tagMapper;

    public TagService(ITagRepository tagRepository, ITagMapper tagMapper)
    {
        _tagRepository = tagRepository;
        _tagMapper = tagMapper;
    }
    
    public async Task<TagGetDto?> GetTagByIdAsync(int id)
    {
        var tag = await _tagRepository.GetByIdAsync(id);
        return tag != null ? _tagMapper.MapToGetDto(tag) : null;
    }

    public async Task<IEnumerable<TagGetDto>> GetAllTagsAsync()
    {
        var tags = await _tagRepository.GetAllAsync();
        return tags.Select(_tagMapper.MapToGetDto).ToList();

    }

    public async Task<TagGetDto?> CreateTagAsync(TagCreateDto tagDto)
    {
        var existingTag = await _tagRepository.GetByNameAsync(tagDto.Name);
        if (existingTag != null)
            return null;
        
        var tag = _tagMapper.MapToEntity(tagDto);
        await _tagRepository.AddAsync(tag);

        return _tagMapper.MapToGetDto(tag);
    }

    public async Task<bool> UpdateTagAsync(int id, TagUpdateDto tagDto)
    {
        var existingTag = await _tagRepository.GetByIdAsync(id);
        if (existingTag == null)
            return false;

        existingTag = _tagMapper.MapToEntity(tagDto, existingTag);
        await _tagRepository.UpdateAsync(existingTag);

        return true;
    }

    public async Task<bool> DeleteTagAsync(int id)
    {
        var existingTag = await _tagRepository.GetByIdAsync(id);
        if (existingTag == null)
            return false;

        await _tagRepository.DeleteAsync(existingTag);
        return true;
    }
}