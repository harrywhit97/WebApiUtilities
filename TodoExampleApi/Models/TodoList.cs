using System.Collections.Generic;
using WebApiUtilities.Abstract;
using WebApiUtilities.Interfaces;

namespace TodoExampleApi.Models
{
    public class TodoList : AuditableEntity, IHasId<long>
    {
        public long Id { get; set; }
        public string ListName { get; set; }
        public IList<TodoItem> Todos { get; set; }

        public TodoList()
        {
            Todos = new List<TodoItem>();
        }
    }
}
