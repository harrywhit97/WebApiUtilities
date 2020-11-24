using System.Threading.Tasks;
using TodoExampleApi.Models;
using WebApiUtilities.Abstract;

namespace TodoExampleApi.Features.TodoItems
{
    public class TodoItemService : BaseRecordService<TodoListContext, TodoItem, long>, ITodoService
    {
        public TodoItemService(TodoListContext context)
            : base(context)
        {
        }

        public async Task CompleteTask(long id)
        {
            var item = await Get(id);
            item.IsDone = true;
            await Update(item);
        }
    }
}
