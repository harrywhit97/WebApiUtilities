using WebApiUtilities.Interfaces;

namespace WebApiUtilities.Abstract
{
    public abstract class Entity<TId> : IEntity<TId>
    {
        public TId Id { get; set; }
    }
}
