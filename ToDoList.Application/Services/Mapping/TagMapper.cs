using ToDoList.Application.DTOs.Tag;
using ToDoList.Domain.Entities;

namespace ToDoList.Application.Services.Mapping;

public class TagMapper : ITagMapper
{
    public TagGetDto MapToGetDto(Tag tag)
    {
        return new TagGetDto
        {
            Id = tag.Id,
            Name = tag.Name
        };
    }

    public Tag MapToEntity(TagCreateDto tagDto)
    {
        return new Tag
        {
            Name = tagDto.Name,
        };
    }

    public Tag MapToEntity(TagUpdateDto tagDto, Tag tag)
    {
        tag.Name = tagDto.Name;

        return tag;
    }
}