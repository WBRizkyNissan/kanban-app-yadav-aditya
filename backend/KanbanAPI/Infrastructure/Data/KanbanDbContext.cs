using Microsoft.EntityFrameworkCore;

namespace KanbanAPI.Infrastructure.Data
{
 public class KanbanDbContext : DbContext
 {
     public KanbanDbContext(DbContextOptions<KanbanDbContext> options) : base(options)
     {
     }
        // Db sets
 }   
}