using TodoExampleApi.Models;
using WebApiUtilities.CrudRequests;

namespace TodoExampleApi.Features.TodoLists
{
    public class UpdateTodoList : TodoListDto, IUpdateCommand<TodoList, long>
    {
    }
}
