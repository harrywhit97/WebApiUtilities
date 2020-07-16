using WebApiUtilities.Interfaces;

namespace WebApiUtilities.Abstract
{
    public abstract class Dto<T, TId> : Entity<TId>, IDto<T, TId>
        where T : Entity<TId>
    {
    }
}
