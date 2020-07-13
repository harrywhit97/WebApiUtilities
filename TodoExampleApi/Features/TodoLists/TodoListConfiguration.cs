using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoExampleApi.Models;

namespace TodoExampleApi.Features.TodoLists
{
    public class TodoListConfiguration : IEntityTypeConfiguration<TodoList>
    {
        public void Configure(EntityTypeBuilder<TodoList> builder)
        {
            builder.Property(x => x.ListName)
                .HasMaxLength(20);
        }
    }
}
