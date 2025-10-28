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
            b.Entity<Board>(entity_builder =>
            {
                entity_builder.HasKey(x => x.Id);
                entity_builder.Property(x => x.Id).HasMaxLength(64);
                entity_builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
                entity_builder.Property(x => x.CreatedAtUtc);
                entity_builder.Property(x => x.UpdatedAtUtc);
            });

            // Column
            b.Entity<Column>(entity_builder =>
            {
                entity_builder.HasKey(x => x.Id);
                entity_builder.Property(x => x.Id).HasMaxLength(64);
                entity_builder.Property(x => x.Title).IsRequired().HasMaxLength(200);

                entity_builder.HasOne(x => x.Board)
                  .WithMany(bd => bd.Columns)
                  .HasForeignKey(x => x.BoardId)
                  .OnDelete(DeleteBehavior.Cascade);

                entity_builder.Property(x => x.CreatedAtUtc);
                entity_builder.Property(x => x.UpdatedAtUtc);
            });
            
            // TaslkItem
            b.Entity<TaskItem>(entity_builder =>
            {
                entity_builder.HasKey(x => x.Id);
                entity_builder.Property(x => x.Id).HasMaxLength(64);
                entity_builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
                entity_builder.Property(x => x.Desc).IsRequired().HasMaxLength(1000);

                entity_builder.HasOne(x => x.Column)
                  .WithMany(c => c.Tasks)
                  .HasForeignKey(x => x.ColumnId)
                  .OnDelete(DeleteBehavior.Cascade);

                entity_builder.Property(x => x.CreatedAtUtc);
                entity_builder.Property(x => x.UpdatedAtUtc);

                entity_builder.HasIndex(x => new { x.ColumnId });
            });
        }
    }