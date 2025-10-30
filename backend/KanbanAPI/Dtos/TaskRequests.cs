namespace KanbanAPI.Dtos;

public class CreateTaskRequest
{
    public string? Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Desc { get; set;}
    public string? DueDate { get; set;}
}

public class UpdateTaskRequest
{
    public string? Title { get; set; }
    public string? Desc { get; set; } 
    public string? DueDate { get; set; } 
}