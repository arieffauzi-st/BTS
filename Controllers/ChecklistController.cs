using System;
using Microsoft.AspNetCore.Mvc;
using BTS.Models;
using BTS.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace BTS.Controllers;

[Authorize]
[ApiController]
[Route("api/checklist")]
public class ChecklistController : ControllerBase
{
    private readonly ApiClient _apiClient;

    public ChecklistController(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [HttpPost]
    public async Task<IActionResult> CreateChecklist([FromBody] CreateChecklistModel model)
    {
        try
        {
            var checklist = await _apiClient.CreateChecklist(model.Name);
            return CreatedAtAction(nameof(GetChecklist), new { id = checklist.Id }, checklist);
        }
        catch (HttpRequestException)
        {
            return BadRequest(new { Message = "Failed to create checklist" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteChecklist(int id)
    {
        try
        {
            var success = await _apiClient.DeleteChecklist(id);
            if (success)
            {
                return NoContent();
            }
            return NotFound();
        }
        catch (HttpRequestException)
        {
            return BadRequest(new { Message = "Failed to delete checklist" });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllChecklists()
    {
        try
        {
            var checklists = await _apiClient.GetAllChecklists();
            return Ok(checklists);
        }
        catch (HttpRequestException)
        {
            return BadRequest(new { Message = "Failed to get checklists" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetChecklist(int id)
    {
        try
        {
            var checklist = await _apiClient.GetChecklistDetail(id);
            return Ok(checklist);
        }
        catch (HttpRequestException)
        {
            return NotFound();
        }
    }

    [HttpPost("{checklistId}/item")]
    public async Task<IActionResult> AddChecklistItem(int checklistId, [FromBody] CreateChecklistItemModel model)
    {
        try
        {
            var item = await _apiClient.AddChecklistItem(checklistId, model.Name);
            return CreatedAtAction(nameof(GetChecklistItem), new { checklistId, itemId = item.Id }, item);
        }
        catch (HttpRequestException)
        {
            return BadRequest(new { Message = "Failed to add checklist item" });
        }
    }

    [HttpGet("{checklistId}/item/{itemId}")]
    public async Task<IActionResult> GetChecklistItem(int checklistId, int itemId)
    {
        try
        {
            var item = await _apiClient.GetChecklistItemDetail(checklistId, itemId);
            return Ok(item);
        }
        catch (HttpRequestException)
        {
            return NotFound();
        }
    }

    [HttpPut("{checklistId}/item/{itemId}")]
    public async Task<IActionResult> UpdateChecklistItem(int checklistId, int itemId, [FromBody] UpdateChecklistItemModel model)
    {
        try
        {
            var item = await _apiClient.UpdateChecklistItem(checklistId, itemId, model.Name);
            return Ok(item);
        }
        catch (HttpRequestException)
        {
            return BadRequest(new { Message = "Failed to update checklist item" });
        }
    }

    [HttpPut("{checklistId}/item/{itemId}/status")]
    public async Task<IActionResult> UpdateItemStatus(int checklistId, int itemId)
    {
        try
        {
            var item = await _apiClient.UpdateItemStatus(checklistId, itemId);
            return Ok(item);
        }
        catch (HttpRequestException)
        {
            return BadRequest(new { Message = "Failed to update item status" });
        }
    }

    [HttpDelete("{checklistId}/item/{itemId}")]
    public async Task<IActionResult> DeleteChecklistItem(int checklistId, int itemId)
    {
        try
        {
            var success = await _apiClient.DeleteChecklistItem(checklistId, itemId);
            if (success)
            {
                return NoContent();
            }
            return NotFound();
        }
        catch (HttpRequestException)
        {
            return BadRequest(new { Message = "Failed to delete checklist item" });
        }
    }

    [AllowAnonymous]
    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok("API is working");
    }
}
