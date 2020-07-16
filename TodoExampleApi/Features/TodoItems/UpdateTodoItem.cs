using TodoExampleApi.Models;
using WebApiUtilities.CrudRequests;

namespace TodoExampleApi.Features.TodoItems
{
    public class UpdateTodoItem : TodoItemDto, IUpdateCommand<TodoItem, long>
    {
    }
}
