using AutoMapper;
using MediatR;
using TodoExampleApi.Models;
using WebApiUtilities.CrudRequests;
using WebApiUtilities.Interfaces;

namespace TodoExampleApi.Features.TodoLists
{
    public class CreateTodoList : TodoListDto, IRequest<TodoList>, IMapFrom<TodoList>
    {
    }

    public class CreateTodoItemHandler : CreateEntityFromRequestHandler<TodoList, long, CreateTodoList>
    {
        public CreateTodoItemHandler(TodoListContext context, IMapper mapper)
            :base(context, mapper)
        {
        }
    }
}
