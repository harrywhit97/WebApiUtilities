using TodoExampleApi.Models;
using WebApiUtilities.CrudRequests;

namespace TodoExampleApi.Features.TodoItems.Commands
{
    public class UpdateTodoItem : TodoItemDto, IUpdateCommand<TodoItem, long>
    {
    }
}
