using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoExampleApi.Models;
using WebApiUtilities.CrudRequests;
using WebApiUtilities.Interfaces;

namespace TodoExampleApi.Features.TodoItems
{
    public class UpdateTodoItem : TodoItemDto, IRequest<TodoItem>, IHasId<long>, IMapFrom<TodoItem>
    {
        public long Id { get; set; }
    }

    public class UpdateTodoItemHandler : UpdateEntityFromRequestHandler<TodoItem, long, UpdateTodoItem>
    {
        public UpdateTodoItemHandler(TodoListContext context, IMapper mapper)
            : base(context, mapper)
        {
        }
    }
}
