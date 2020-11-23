using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Options;
using IdentityServer4.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TodoExampleApi.Models;
using WebApiUtilities.Abstract;
using WebApiUtilities.Identity;
using WebApiUtilities.Interfaces;

namespace TodoExampleApi
{
    public class TodoListContext : AuditingDbContext
    {
        public DbSet<TodoItem> Todos { get; set; }
        //public DbSet<TodoList> TodoLists { get; set; }

        public TodoListContext(DbContextOptions options, IClock clock)
            : base(options, clock)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            modelbuilder.ApplyConfigurationsFromAssembly(typeof(TodoListContext).Assembly);

            //modelbuilder.Entity<TodoItem>()
            //    .HasOne(t => t.List)
            //    .WithMany(l => l.Todos);

            //modelbuilder.Entity<TodoList>()
            //    .HasMany(l => l.Todos)
            //    .WithOne(t => t.List)
            //    .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelbuilder);
        }
    }
}
