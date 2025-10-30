using System.Globalization;
using KanbanAPI.Dtos;
using KanbanAPI.Entities;
using KanbanAPI.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KanbanAPI.Controllers;

[ApiController]
public class TasksController : ControllerBase
{
    private readonly KanbanDbContext _db;
    public TasksController(KanbanDbContext db) => _db = db;

    // POST /api/columns/{columnId}/tasks
    [HttpPost("api/columns/{columnId}/tasks")]
    public async Task<ActionResult<TaskDto>> Create(string columnId, [FromBody] CreateTaskPayload payload)
    {
        // validate title 
        if (string.IsNullOrWhiteSpace(payload.Title))
            return Problem(statusCode: 400, title: "Validation error", detail: "Title is required.");
        if (payload.Title.Length > 100)
            return Problem(statusCode: 400, title: "Validation error", detail: "Title should not exceed 100 characters");
        if (payload.Desc is not null && payload.Desc.Length > 1000) 
            return Problem(statusCode: 400, title: "Validation error", detail: "Description should not exceed 1000 characters");

        var column = await _db.Columns.FirstOrDefaultAsync(columnEntity => columnEntity.Id == columnId);
        if (column is null) return NotFound(new { message = "Column not found" });

        var taskId = string.IsNullOrWhiteSpace(payload.Id) ? Guid.NewGuid().ToString("N") : payload.Id.Trim();

        DateOnly? parsedDueDate = null;
        if (!string.IsNullOrWhiteSpace(payload.DueDate))
        {
            if (!DateOnly.TryParseExact(payload.DueDate, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                                        DateTimeStyles.None, out var dueDateValue))
                return Problem(statusCode: 400, title: "Validation error", detail: "Due Date must be yyyy-MM-dd");
            parsedDueDate = dueDateValue;
        }

        var newTask = new TaskItem
        {
            Id = taskId,
            Title = payload.Title.Trim(),
            Desc  = payload.Desc?.Trim(),
            DueDate = parsedDueDate,
            ColumnId = column.Id,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };

        _db.Tasks.Add(newTask);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetOne), new { id = newTask.Id }, newTask.ToDto());
    }

    // GET /api/tasks/{id} 
    [HttpGet("api/tasks/{id}")]
    public async Task<ActionResult<TaskDto>> GetOne(string id)
    {
        var taskItem = await _db.Tasks.FirstOrDefaultAsync(task => task.Id == id);
        if (taskItem is null) return NotFound();
        return Ok(taskItem.ToDto());
    }

    // PATCH /api/tasks/{id}
    [HttpPatch("api/tasks/{id}")]
    public async Task<ActionResult<TaskDto>> Update(string id, [FromBody] UpdateTaskPayload payload)
    {
        var existingTask = await _db.Tasks.FirstOrDefaultAsync(task => task.Id == id);
        if (existingTask is null) return NotFound();

        if (payload.Title is not null)
        {
            if (string.IsNullOrWhiteSpace(payload.Title))
                return Problem(statusCode: 400, title: "Validation error", detail: "Title cannot be empty.");
            if (payload.Title.Length > 100) 
                return Problem(statusCode: 400, title: "Validation error", detail: "Title should not exceed 100 characters");
            existingTask.Title = payload.Title.Trim();
        }

        if (payload.Desc is not null)
        {
            if (payload.Desc.Length > 1000)
                return Problem(statusCode: 400, title: "Validation error", detail: "Description should not exceed 1000 characters");
            existingTask.Desc = payload.Desc.Trim();
        }

        if (payload.DueDate is not null)
        {
            if (payload.DueDate == string.Empty)
            {
                existingTask.DueDate = null; 
            }
            else
            {
                if (!DateOnly.TryParseExact(payload.DueDate, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out var parsedDueDate))
                    return Problem(statusCode: 400, title: "Validation error", detail: "Due Date must be yyyy-MM-dd.");
                existingTask.DueDate = parsedDueDate;
            }
        }

        await _db.SaveChangesAsync();
        return Ok(existingTask.ToDto());
    }

    // DELETE /api/tasks/{id}
    [HttpDelete("api/tasks/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var taskToDelete = await _db.Tasks.FirstOrDefaultAsync(task => task.Id == id);
        if (taskToDelete is null) return NotFound();

        _db.Tasks.Remove(taskToDelete);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}