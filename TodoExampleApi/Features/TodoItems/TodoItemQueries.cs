using TodoExampleApi.Models;
using WebApiUtilities.CrudRequests;

namespace TodoExampleApi.Features.TodoItems
{
    public class TodoListQueries
    {
        public class GetTodoItemsHandler : GetEntitiesHandler<TodoItem, long>
        {
            public GetTodoItemsHandler(TodoListContext context)
                :base(context)
            {
            }
        }

        public class GetTodoItemByIdHandler : GetEntityByIdHandler<TodoItem, long>
        {
            public GetTodoItemByIdHandler(TodoListContext context)
                : base(context)
            {
            }
        }
    }
}
