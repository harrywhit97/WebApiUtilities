using TodoExampleApi.Models;
using WebApiUtilities.Abstract;

namespace TodoExampleApi.Features.TodoItems
{
    public class TodoItemDto : Dto<TodoItem, long>
    {
        public string Description { get; set; }
        public virtual long ListId { get; set; }
        public virtual bool Done { get; set; }
    }
}
