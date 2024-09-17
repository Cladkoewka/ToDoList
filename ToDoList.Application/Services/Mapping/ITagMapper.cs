using ToDoList.Application.DTOs.Tag;
using ToDoList.Domain.Entities;

namespace ToDoList.Application.Services.Mapping;

public interface ITagMapper
{
    TagGetDto MapToGetDto(Tag tag);
    Tag MapToEntity(TagCreateDto tagDto);
    Tag MapToEntity(TagUpdateDto tagDto, Tag tag);
}