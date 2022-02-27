using Kanban.Data.Mappings;
using Kanban.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kanban.Data.Contexts
{
    public class KanbanContext : DbContext
    {
        public KanbanContext(DbContextOptions<KanbanContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CardMap());
        }

        public DbSet<Card> Card { get; set; }
    }
}
