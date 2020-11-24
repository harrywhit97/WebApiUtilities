using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TodoExampleApi.Models;
using WebApiUtilities.Abstract;
using WebApiUtilities.Interfaces;

namespace TodoExampleApi.Features.TodoItems
{
    public class TodoItemController : RecordController<TodoItem, long, TodoItemDto>
    {
        private readonly ITodoService _todoService;

        public TodoItemController(ITodoService service, ILogger<TodoItemController> log, IMapper mapper)
            : base(service, log, mapper)
        {
            _todoService = service;
        }

        [HttpPost("complete/{id}")]
        public IActionResult Complete(long id)
        {
            _todoService.CompleteTask(id);
            return Ok();
        }
    }
}
