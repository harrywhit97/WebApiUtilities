using TodoExampleApi.Models;
using WebApiUtilities.CrudRequests;

namespace TodoExampleApi.Features.TodoLists.Commands
{
    public class UpdateTodoList : TodoListDto, IUpdateCommand<TodoList, long>
    {
    }
}
