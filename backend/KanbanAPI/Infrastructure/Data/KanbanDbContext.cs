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

            foreach (var e in ChangeTracker.Entries()
                .Where(e => e.State is EntityState.Added or EntityState.Modified))
            {
                if (e.Metadata.FindProperty("UpdatedAtUtc") is not null)
                {
                    e.CurrentValues["UpdatedAtUtc"] = now;
                }
                if (e.State == EntityState.Added && e.Metadata.FindProperty("CreatedAtUtc") is not null)
                {
                    e.CurrentValues["CreatedAtUtc"] = now;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder b)
        {
            // Board
            b.Entity<Board>(eb =>
            {
                eb.HasKey(x => x.Id);
                eb.Property(x => x.Id).HasMaxLength(64);
                eb.Property(x => x.Name).IsRequired().HasMaxLength(200);
                eb.Property(x => x.CreatedAtUtc);
                eb.Property(x => x.UpdatedAtUtc);
            });

            // Column
            b.Entity<Column>(eb =>
            {
                eb.HasKey(x => x.Id);
                eb.Property(x => x.Id).HasMaxLength(64);
                eb.Property(x => x.Title).IsRequired().HasMaxLength(200);

                eb.HasOne(x => x.Board)
                  .WithMany(bd => bd.Columns)
                  .HasForeignKey(x => x.BoardId)
                  .OnDelete(DeleteBehavior.Cascade);

                eb.Property(x => x.CreatedAtUtc);
                eb.Property(x => x.UpdatedAtUtc);
            });
            
            // TaslkItem
            b.Entity<TaskItem>(eb =>
            {
                eb.HasKey(x => x.Id);
                eb.Property(x => x.Id).HasMaxLength(64);
                eb.Property(x => x.Title).IsRequired().HasMaxLength(200);
                eb.Property(x => x.Desc).IsRequired().HasMaxLength(1000);

                eb.HasOne(x => x.Column)
                  .WithMany(c => c.Tasks)
                  .HasForeignKey(x => x.ColumnId)
                  .OnDelete(DeleteBehavior.Cascade);

                eb.Property(x => x.CreatedAtUtc);
                eb.Property(x => x.UpdatedAtUtc);

                eb.HasIndex(x => new { x.ColumnId });
            });
        }
    }