namespace KanbanAPI.Entities;

public class Column
{
    public string Id { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string BoardId { get; set; } = default!;
    public Board Board { get; set; } = default!;

    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }

    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}