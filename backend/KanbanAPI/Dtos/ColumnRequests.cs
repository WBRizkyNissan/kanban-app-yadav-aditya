namespace KanbanAPI.Dtos;

public class CreateColumnRequest
{
    public string? Id { get; set; }
    public string Title { get; set; } = default!;
}

public class UpdateColumnRequest
{
    public string Title { get; set; } = default!;
}