using KanbanAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace KanbanAPI.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task MigrateAndSeedAsync(this IServiceProvider services, IWebHostEnvironment env)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<KanbanDbContext>();

        // Applying pending migrations in Dev Env
        await db.Database.MigrateAsync();

        // seed default board if none exists
        if (!await db.Boards.AnyAsync())
        {
            db.Boards.Add(new Board
            {
                Id = "default-board",
                Name = "Default",
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow
            });
            await db.SaveChangesAsync();
        }
    }
}