using TodoExampleApi.Models;
using WebApiUtilities.CrudRequests;

namespace TodoExampleApi.Features.TodoItems
{
    public class DeleteTodoList : DeleteEntityHandler<TodoItem, long>
    {
        public DeleteTodoList(TodoListContext context)
            :base(context)
        {
        }
    }
}
