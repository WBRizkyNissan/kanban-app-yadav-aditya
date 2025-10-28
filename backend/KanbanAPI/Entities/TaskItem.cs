namespace KanbanAPI.Entities;
public class TaskItem
{
    public string Id { get; set; } = default!;
    public string Title { get; set; } = default!;

    public string? Desc { get; set; }
    public DateOnly? DueDate { get; set; }

    public string ColumnId { get; set; } = default!;
    public Column Column { get; set; } = default!;

    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }

}