using TodoExampleApi.Models;
using WebApiUtilities.CrudRequests;

namespace TodoExampleApi.Features.TodoItems
{
    public class CreateTodo : TodoItemDto, ICreateCommand<TodoItem, long>
    {
    }
}
