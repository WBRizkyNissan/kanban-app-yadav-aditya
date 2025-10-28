namespace KanbanAPI.Dtos;

public record TaskDto
(
    string Id,
    string Title,
    string? Desc,
    string? DueDate
);
