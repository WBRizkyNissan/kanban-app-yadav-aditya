using KanbanAPI.Dtos;
using KanbanAPI.Entities;
using KanbanAPI.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KanbanAPI.Controllers;

[ApiController]
[Route("api/columns")]
public class ColumnsController : ControllerBase
{
    private readonly KanbanDbContext _db;
    private const string DefaultBoardId = "default-board";
    public ColumnsController(KanbanDbContext db) => _db = db;

    // POST /api/columns
    [HttpPost]
    public async Task<ActionResult<ColumnDto>> Create([FromBody] CreateColumnRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            return Problem(statusCode: 400, title: "Validation Error", detail: "Title is required");

        if (request.Title.Length > 200)
            return Problem(statusCode: 400, title: "Validation Error", detail: "Title should not exceed 200 characters");

        var board = await _db.Boards.FirstOrDefaultAsync(boardEntity => boardEntity.Id == DefaultBoardId);

        if (board is null)
            return Problem(statusCode: 500, title: "Missing default board", detail: "Default board not found");

        var columnId = string.IsNullOrWhiteSpace(request.Id) ? Guid.NewGuid().ToString("N") : request.Id.Trim();

        var column = new Column
        {
            Id = columnId,
            Title = request.Title.Trim(),
            BoardId = board.Id,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };

        _db.Columns.Add(column);
        await _db.SaveChangesAsync();

        // Return DTO shape expected by the UI
        return CreatedAtAction(nameof(GetOne), new { id = column.Id }, column.ToDto());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ColumnDto>> GetOne([FromRoute(Name = "id")] string columnId)
    {
        var column = await _db.Columns.Include(columnEntity => columnEntity.Tasks).FirstOrDefaultAsync(columnEntity => columnEntity.Id == columnId);
        
        if (column is null) return NotFound();
        return Ok(column.ToDto());
    }

    // PATCH /api/columns/{id}
    [HttpPatch("{id}")]
    public async Task<ActionResult<ColumnDto>> UpdateTitle([FromRoute(Name = "id")] string columnId, [FromBody] UpdateColumnRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            return Problem(statusCode: 400, title: "Validation error", detail: "Title is required.");

        if (request.Title.Length > 200)
            return Problem(statusCode: 400, title: "Validation error", detail: "Title should not exceed 200 characters");

        var column = await _db.Columns.FirstOrDefaultAsync(columnEntity => columnEntity.Id == columnId);
        
        if (column is null) return NotFound();

        column.Title = request.Title.Trim();
        await _db.SaveChangesAsync();

        // Re-fetch with tasks to keep return shape consistent
        var columnWithTasks = await _db.Columns.Include(columnEntity => columnEntity.Tasks).FirstAsync(columnEntity => columnEntity.Id == columnId);
        
        return Ok(columnWithTasks.ToDto());
    }

    // DELETE /api/columns/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute(Name = "id")] string columnId)
    {
        var column = await _db.Columns.FirstOrDefaultAsync(columnEntity => columnEntity.Id == columnId);
        if (column is null) return NotFound();

        _db.Columns.Remove(column); // cascade will delete tasks
        await _db.SaveChangesAsync();

        return NoContent();
    }

}