using TodoExampleApi.Models;
using WebApiUtilities.Abstract;

namespace TodoExampleApi.Features.TodoItems
{
    public class TodoRecord : ChangeableRecord<TodoItem, long, TodoItemDto, TodoListContext, CreateTodo, UpdateTodoItem, TodoItemValidator<CreateTodo>, TodoItemValidator<UpdateTodoItem>>
    {
    }
}
