using TodoExampleApi.Models;
using WebApiUtilities.Abstract;

namespace TodoExampleApi.Features.TodoLists
{
    public class TodoListRecord : ChangeableRecord<TodoList, long, TodoListDto, TodoListContext, CreateTodoList, UpdateTodoList, 
        TodoListValidator<CreateTodoList>, TodoListValidator<UpdateTodoList>>
    {
    }
}
