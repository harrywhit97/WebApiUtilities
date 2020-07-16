using TodoExampleApi.Models;
using WebApiUtilities.CrudRequests;

namespace TodoExampleApi.Features.TodoLists
{
    public class CreateTodoList : TodoListDto, ICreateCommand<TodoList, long>
    {
    }
}
