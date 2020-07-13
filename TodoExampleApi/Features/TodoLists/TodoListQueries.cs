using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoExampleApi.Models;
using WebApiUtilities.CrudRequests;

namespace TodoExampleApi.Features.TodoLists
{
    public class TodoListQueries
    {
        public class GetTodoItemsHandler : GetEntitiesHandler<TodoList, long>
        {
            public GetTodoItemsHandler(TodoListContext context)
                :base(context)
            {
            }
        }

        public class GetTodoItemByIdHandler : GetEntityByIdHandler<TodoList, long>
        {
            public GetTodoItemByIdHandler(TodoListContext context)
                : base(context)
            {
            }
        }
    }
}
