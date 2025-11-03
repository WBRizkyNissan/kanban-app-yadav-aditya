namespace KanbanAPI.Dtos;

public record ColumnDto
(
    string Id,
    string Title, 
    List <TaskDto> Tasks
);
