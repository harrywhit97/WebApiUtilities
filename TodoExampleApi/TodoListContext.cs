using Microsoft.EntityFrameworkCore;
using TodoExampleApi.Models;
using WebApiUtilities.Abstract;
using WebApiUtilities.Interfaces;

namespace TodoExampleApi
{
    public class TodoListContext : AuditingDbContext
    {
        public DbSet<TodoItem> Todos { get; set; }
        public DbSet<TodoList> TodoLists { get; set; }
        public IClock Clock { get; set; }

        public TodoListContext(DbContextOptions<TodoListContext> options, IClock clock)
            :base(options)
        {
            Clock = clock;
        }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            modelbuilder.ApplyConfigurationsFromAssembly(typeof(TodoListContext).Assembly);

            modelbuilder.Entity<TodoItem>()
                .HasOne(t => t.List)
                .WithMany(l => l.Todos);
            
            modelbuilder.Entity<TodoList>()
                .HasMany(l => l.Todos)
                .WithOne(t => t.List)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelbuilder);
        }
    }
}
