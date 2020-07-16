using TodoExampleApi.Models;
using WebApiUtilities.CrudRequests;

namespace TodoExampleApi.Features.TodoLists.Commands
{
    public class CreateTodoList : TodoListDto, ICreateCommand<TodoList, long>
    {
    }
}
