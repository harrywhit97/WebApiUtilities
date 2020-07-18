using Microsoft.Extensions.Logging;
using TodoExampleApi.Features.TodoLists.Commands;
using TodoExampleApi.Models;
using WebApiUtilities.Abstract;

namespace TodoExampleApi.Features.TodoLists
{
    public class TodoListController : CrudController<TodoList, long, CreateTodoList, UpdateTodoList>
    {
        public TodoListController(TodoListContext context, ILogger<TodoListController> logger)
            :base(context, logger)
        {
        }
    }
}
