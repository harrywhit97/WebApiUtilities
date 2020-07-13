using Microsoft.Extensions.Logging;
using TodoExampleApi.Models;
using WebApiUtilities.Abstract;

namespace TodoExampleApi.Features.TodoItems
{
    public class TodoItemController : AbstractController<TodoItem, long, TodoItemDto>
    {
        public TodoItemController(TodoListContext context, ILogger<TodoItemController> logger)
            :base(context, logger)
        {
        }
    }
}
