using ToDoList.Application.DTOs.Tag;

namespace ToDoList.Application.Services.Interfaces;

public interface ITagService
{
    Task<TagGetDto?> GetTagByIdAsync(int id);
    Task<IEnumerable<TagGetDto>> GetAllTagsAsync();
    Task<TagGetDto?> CreateTagAsync(TagCreateDto tagDto);
    Task<bool> UpdateTagAsync(int id, TagUpdateDto tagDto);
    Task<bool> DeleteTagAsync(int id);

}