namespace KanbanAPI.Dtos;

public record ColumnDto
(
    string Id,
    string Name, 
    List <TaskDto> Columns
);
