using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiUtilities.Abstract;

namespace TodoExampleApi.Models
{
    public class TodoList : AuditableEntity
    {
        public string ListName { get; set; }
        public IList<TodoItem> Todos { get; set; }
    }
}
