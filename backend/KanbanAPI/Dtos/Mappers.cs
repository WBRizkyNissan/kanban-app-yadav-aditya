using KanbanAPI.Entities;

namespace KanbanAPI.Dtos;

public static class Mappers
{
    public static BoardDto ToDto(this Board board) => new BoardDto(
        board.Id,
        board.Name,
        board.Columns
            .OrderBy(column => column.CreatedAtUtc)
            .Select(column => column.ToDto())
            .ToList()
    );
    public static ColumnDto ToDto(this Column column) => new ColumnDto(
            column.Id,
            column.Title,
            (column.Tasks ?? Array.Empty<TaskItem>())
                .OrderByDescending(task => task.CreatedAtUtc)
                .Select(task => task.ToDto())
                .ToList()
    );
    public static TaskDto ToDto(this TaskItem task) => new TaskDto(
            task.Id,
            task.Title,
            task.Desc,
            task.DueDate?.ToString("yyyy-MM-dd")
    );
}