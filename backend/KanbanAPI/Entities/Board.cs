namespace KanbanAPI.Entities;
    public class Board
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;

        public DateTime CreatedAtUtc { get; set; }
        public DateTime UpdatedAtUtc { get; set; }

        public ICollection<Column> Columns { get; set; } = new List<Column>();
    }
