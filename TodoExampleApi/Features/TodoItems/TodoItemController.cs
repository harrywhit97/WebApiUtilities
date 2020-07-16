using Microsoft.Extensions.Logging;
using TodoExampleApi.Features.TodoItems.Commands;
using TodoExampleApi.Models;
using WebApiUtilities.Abstract;

namespace TodoExampleApi.Features.TodoItems
{
    public class TodoItemController : CrudController<TodoItem, long, CreateTodo, UpdateTodoItem>
    {
        public TodoItemController(TodoListContext context, ILogger<TodoItemController> logger)
            :base(context, logger)
        {
        }
    }
}
