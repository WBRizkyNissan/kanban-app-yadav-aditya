using KanbanAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace KanbanAPI.Infrastructure.Data;
    public class KanbanDbContext : DbContext
    {
        public KanbanDbContext(DbContextOptions<KanbanDbContext> options) : base(options) { }

        // Db sets
        public DbSet<Board> Boards => Set<Board>();
        public DbSet<Column> Columns => Set<Column>();
        public DbSet<TaskItem> Tasks => Set<TaskItem>();

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries()
            .Where(entry => entry.State is EntityState.Added or EntityState.Modified))
        {
            if (entry.Metadata.FindProperty("UpdatedAtUtc") is not null)
            {
                entry.CurrentValues["UpdatedAtUtc"] = now;
            }
            if (entry.State == EntityState.Added && entry.Metadata.FindProperty("CreatedAtUtc") is not null)
            {
                entry.CurrentValues["CreatedAtUtc"] = now;
            }
        }
            return base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder b)
        {
            // Board
            b.Entity<Board>(entityBuilder =>
            {
                entityBuilder.HasKey(x => x.Id);
                entityBuilder.Property(x => x.Id).HasMaxLength(64);
                entityBuilder.Property(x => x.Name).IsRequired().HasMaxLength(200);
                entityBuilder.Property(x => x.CreatedAtUtc);
                entityBuilder.Property(x => x.UpdatedAtUtc);
            });

            // Column
            b.Entity<Column>(entityBuilder =>
            {
                entityBuilder.HasKey(x => x.Id);
                entityBuilder.Property(x => x.Id).HasMaxLength(64);
                entityBuilder.Property(x => x.Title).IsRequired().HasMaxLength(200);

                entityBuilder.HasOne(x => x.Board)
                  .WithMany(bd => bd.Columns)
                  .HasForeignKey(x => x.BoardId)
                  .OnDelete(DeleteBehavior.Cascade);

                entityBuilder.Property(x => x.CreatedAtUtc);
                entityBuilder.Property(x => x.UpdatedAtUtc);
            });
            
            // TaslkItem
            b.Entity<TaskItem>(entityBuilder =>
            {
                entityBuilder.HasKey(x => x.Id);
                entityBuilder.Property(x => x.Id).HasMaxLength(64);
                entityBuilder.Property(x => x.Title).IsRequired().HasMaxLength(200);
                entityBuilder.Property(x => x.Desc).IsRequired().HasMaxLength(1000);

                entityBuilder.HasOne(x => x.Column)
                  .WithMany(c => c.Tasks)
                  .HasForeignKey(x => x.ColumnId)
                  .OnDelete(DeleteBehavior.Cascade);

                entityBuilder.Property(x => x.CreatedAtUtc);
                entityBuilder.Property(x => x.UpdatedAtUtc);

                entityBuilder.HasIndex(x => new { x.ColumnId });
            });
        }
    }