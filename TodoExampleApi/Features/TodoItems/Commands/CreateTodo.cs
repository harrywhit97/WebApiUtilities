using TodoExampleApi.Models;
using WebApiUtilities.CrudRequests;

namespace TodoExampleApi.Features.TodoItems.Commands
{
    public class CreateTodo : TodoItemDto, ICreateCommand<TodoItem, long>
    {
    }
}
