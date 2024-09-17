using Microsoft.AspNetCore.Mvc;
using ToDoList.Application.DTOs.Tag;
using ToDoList.Application.Services.Interfaces;

namespace ToDoList.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagsController : ControllerBase
{
    private readonly ITagService _tagService;

    public TagsController(ITagService tagService)
    {
        _tagService = tagService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TagGetDto>>> GetAllTags()
    {
        var tags = await _tagService.GetAllTagsAsync();
        return Ok(tags);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TagGetDto>> GetTagById(int id)
    {
        var tag = await _tagService.GetTagByIdAsync(id);
        if (tag == null)
            return NotFound($"Tag with ID {id} not found");

        return Ok(tag);
    }

    [HttpPost]
    public async Task<ActionResult<TagGetDto>> AddTag([FromBody] TagCreateDto tagDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdTag = await _tagService.CreateTagAsync(tagDto);

        if (createdTag == null)
            return BadRequest("Tag could not be created.");

        return CreatedAtAction(nameof(GetTagById), new { id = createdTag.Id }, createdTag);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateTag(int id, [FromBody] TagUpdateDto tagDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var success = await _tagService.UpdateTagAsync(id, tagDto);
        if (!success)
            return NotFound($"Tag with ID {id} not found.");

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteTag(int id)
    {
        var success = await _tagService.DeleteTagAsync(id);
        if (!success)
            return NotFound($"Tag with ID {id} not found.");

        return NoContent();
    }
}