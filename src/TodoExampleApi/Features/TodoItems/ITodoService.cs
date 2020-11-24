using System.Threading.Tasks;
using TodoExampleApi.Models;
using WebApiUtilities.Interfaces;

namespace TodoExampleApi.Features.TodoItems
{
    public interface ITodoService : IRecordService<TodoItem, long>
    {
        Task CompleteTask(long id);
    }
}
