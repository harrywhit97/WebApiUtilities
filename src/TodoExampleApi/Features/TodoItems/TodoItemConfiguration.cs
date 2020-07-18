using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoExampleApi.Models;

namespace TodoExampleApi.Features.TodoItems
{
    public class TodoListConfiguration : IEntityTypeConfiguration<TodoItem>
    {
        public void Configure(EntityTypeBuilder<TodoItem> builder)
        {
            builder.Property(x => x.Description)
                .HasMaxLength(20);
        }
    }
}
