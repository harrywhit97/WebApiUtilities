using TodoExampleApi.Models;
using WebApiUtilities.CrudRequests;

namespace TodoExampleApi.Features.TodoLists
{
    public class DeleteTodoList : DeleteEntityHandler<TodoList, long>
    {
        public DeleteTodoList(TodoListContext context)
            :base(context)
        {
        }
    }
}
