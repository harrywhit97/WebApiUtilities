using WebApiUtilities.Abstract;
using WebApiUtilities.Interfaces;

namespace TodoExampleApi.Models
{
    public class TodoItem : AuditableEntity, IHasId<long>
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public TodoList List { get; set; }
    }
}
