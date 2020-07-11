using WebApiUtilities.Abstract;
using WebApiUtilities.Interfaces;

namespace TodoExampleApi.Models
{
    public class TodoItem : AuditableEntity, IHasId<long>
    {
        public long Id { get; }
        public string Description { get; set; }
    }
}
