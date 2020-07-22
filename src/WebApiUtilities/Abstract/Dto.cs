using WebApiUtilities.Interfaces;

namespace WebApiUtilities.Abstract
{
    public abstract class Dto<T, TId> : IDto<T, TId>
        where T : Entity<TId>
    {
        public TId Id { get; set; }
    }
}
