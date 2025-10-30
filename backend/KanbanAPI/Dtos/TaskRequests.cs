namespace KanbanAPI.Dtos;

public class CreateTaskPayload
{
    public string? Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Desc { get; set;}
    public string? DueDate { get; set;}
}

public class UpdateTaskPayload
{
    public string? Title { get; set; }
    public string? Desc { get; set; } 
    public string? DueDate { get; set; } 
}