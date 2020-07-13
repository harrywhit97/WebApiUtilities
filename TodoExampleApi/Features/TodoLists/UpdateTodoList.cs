using AutoMapper;
using MediatR;
using TodoExampleApi.Models;
using WebApiUtilities.CrudRequests;
using WebApiUtilities.Interfaces;

namespace TodoExampleApi.Features.TodoLists
{
    public class UpdateTodoList : TodoListDto, IRequest<TodoList>, IHasId<long>, IMapFrom<TodoList>
    {
        public long Id { get; set; }
    }

    public class UpdateTodoListHandler : UpdateEntityFromRequestHandler<TodoList, long, UpdateTodoList>
    {
        public UpdateTodoListHandler(TodoListContext context, IMapper mapper)
            : base(context, mapper)
        {
        }
    }
}
