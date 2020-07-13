using AutoMapper;
using MediatR;
using TodoExampleApi.Models;
using WebApiUtilities.CrudRequests;
using WebApiUtilities.Interfaces;

namespace TodoExampleApi.Features.TodoItems
{
    public class CreateTodoItem : TodoItemDto, IRequest<TodoItem>, IMapFrom<TodoItem>
    {
    }

    public class CreateTodoItemHandler : CreateEntityFromRequestHandler<TodoItem, long, CreateTodoItem>
    {
        public CreateTodoItemHandler(TodoListContext context, IMapper mapper)
            :base(context, mapper)
        {
        }
    }
}
