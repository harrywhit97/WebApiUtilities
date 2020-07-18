using System.Collections.Generic;
using WebApiUtilities.Abstract;
using WebApiUtilities.Interfaces;

namespace TodoExampleApi.Models
{
    public class TodoList : AuditableEntity<long>
    {
        public string ListName { get; set; }
        public virtual IList<TodoItem> Todos { get; set; }

        public TodoList()
        {
            Todos = new List<TodoItem>();
        }
    }
}
