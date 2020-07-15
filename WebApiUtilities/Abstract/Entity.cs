using WebApiUtilities.Interfaces;

namespace WebApiUtilities.Abstract
{
    public abstract class Entity<TId> : IHasId<TId>
    {
        public TId Id { get; set; }
    }
}
