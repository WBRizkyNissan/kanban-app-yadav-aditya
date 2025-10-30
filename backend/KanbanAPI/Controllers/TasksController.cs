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
    public async Task<ActionResult<TaskDto>> Create(string columnId, [FromBody] CreateTaskRequest request)
    {
        // validate title 
        if (string.IsNullOrWhiteSpace(request.Title))
            return Problem(statusCode: 400, title: "Validation error", detail: "Title is required.");
        if (request.Title.Length > 100)
            return Problem(statusCode: 400, title: "Validation error", detail: "Title should not exceed 100 characters");
        if (request.Desc is not null && request.Desc.Length > 1000) 
            return Problem(statusCode: 400, title: "Validation error", detail: "Description should not exceed 1000 characters");

        var column = await _db.Columns.FirstOrDefaultAsync(columnEntity => columnEntity.Id == columnId);
        if (column is null) return NotFound(new { message = "Column not found" });

        var taskId = string.IsNullOrWhiteSpace(request.Id) ? Guid.NewGuid().ToString("N") : request.Id.Trim();

        DateOnly? parsedDueDate = null;
        if (!string.IsNullOrWhiteSpace(request.DueDate))
        {
            if (!DateOnly.TryParseExact(request.DueDate, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                                        DateTimeStyles.None, out var dueDateValue))
                return Problem(statusCode: 400, title: "Validation error", detail: "Due Date must be yyyy-MM-dd");
            parsedDueDate = dueDateValue;
        }

        var newTask = new TaskItem
        {
            Id = taskId,
            Title = request.Title.Trim(),
            Desc  = request.Desc?.Trim(),
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
    public async Task<ActionResult<TaskDto>> Update(string id, [FromBody] UpdateTaskRequest request)
    {
        var existingTask = await _db.Tasks.FirstOrDefaultAsync(task => task.Id == id);
        if (existingTask is null) return NotFound();

        if (request.Title is not null)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
                return Problem(statusCode: 400, title: "Validation error", detail: "Title cannot be empty.");
            if (request.Title.Length > 100) 
                return Problem(statusCode: 400, title: "Validation error", detail: "Title should not exceed 100 characters");
            existingTask.Title = request.Title.Trim();
        }

        if (request.Desc is not null)
        {
            if (request.Desc.Length > 1000)
                return Problem(statusCode: 400, title: "Validation error", detail: "Description should not exceed 1000 characters");
            existingTask.Desc = request.Desc.Trim();
        }

        if (request.DueDate is not null)
        {
            if (request.DueDate == string.Empty)
            {
                existingTask.DueDate = null; 
            }
            else
            {
                if (!DateOnly.TryParseExact(request.DueDate, "yyyy-MM-dd", CultureInfo.InvariantCulture,
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