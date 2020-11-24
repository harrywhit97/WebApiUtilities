using WebApiUtilities.Abstract;

namespace TodoExampleApi.Models
{
    public class TodoItem : AuditableEntity<long>
    {
        public string Description { get; set; }
        public virtual long ListId { get; set; }
        public virtual bool IsDone { get; set; }
    }
}
