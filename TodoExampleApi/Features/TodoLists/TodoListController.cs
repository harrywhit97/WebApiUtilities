using Microsoft.Extensions.Logging;
using TodoExampleApi.Models;
using WebApiUtilities.Abstract;

namespace TodoExampleApi.Features.TodoLists
{
    public class TodoListController : AbstractController<TodoList, long, TodoListDto>
    {
        public TodoListController(TodoListContext context, ILogger<TodoListController> logger)
            :base(context, logger)
        {
        }
    }
}
