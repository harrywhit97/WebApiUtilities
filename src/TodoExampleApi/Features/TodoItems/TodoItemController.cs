using AutoMapper;
using Microsoft.Extensions.Logging;
using TodoExampleApi.Models;
using WebApiUtilities.Abstract;
using WebApiUtilities.Interfaces;

namespace TodoExampleApi.Features.TodoItems
{
    public class TodoItemController : RecordController<TodoItem, long, TodoItemDto>
    {
        public TodoItemController(IRecordService<TodoItem, long> service, ILogger<TodoItemController> log, IMapper mapper)
            : base(service, log, mapper)
        {
        }
    }
}
