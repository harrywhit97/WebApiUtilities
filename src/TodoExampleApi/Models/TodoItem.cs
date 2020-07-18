using WebApiUtilities.Abstract;
using WebApiUtilities.Interfaces;

namespace TodoExampleApi.Models
{
    public class TodoItem : AuditableEntity<long>
    {
        public string Description { get; set; }
        public virtual TodoList List { get; set; }
    }
}
