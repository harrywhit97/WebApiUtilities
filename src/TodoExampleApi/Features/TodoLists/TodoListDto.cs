using TodoExampleApi.Models;
using WebApiUtilities.Abstract;

namespace TodoExampleApi.Features.TodoLists
{
    public class TodoListDto : Dto<TodoList, long>
    {
        public string ListName { get; set; }
    }
}
