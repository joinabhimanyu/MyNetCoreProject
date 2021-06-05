using Microsoft.EntityFrameworkCore;

namespace MyProject.Models
{
    public class EntityContext : DbContext
    {
        public EntityContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Entity> Entities { get; set; }
    }
}