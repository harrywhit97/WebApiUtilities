using TodoExampleApi.Features.TodoLists.Commands;
using TodoExampleApi.Models;
using WebApiUtilities.Abstract;

namespace TodoExampleApi.Features.TodoLists
{
    public class TodoListRecord : ChangeableRecord<TodoList, long, TodoListContext, CreateTodoList, UpdateTodoList, 
        TodoListValidator<CreateTodoList>, TodoListValidator<UpdateTodoList>>
    {
    }
}
