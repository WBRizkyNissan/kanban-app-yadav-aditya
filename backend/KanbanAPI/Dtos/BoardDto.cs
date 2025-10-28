namespace KanbanAPI.Dtos;

public record BoardDto
(
    string Id,
    string Name, 
    List <ColumnDto> Columns
);
