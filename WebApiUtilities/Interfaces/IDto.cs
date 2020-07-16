using WebApiUtilities.Abstract;

namespace WebApiUtilities.Interfaces
{
    public interface IDto<T, TId> : IHasId<TId>, IMapFrom<T>
        where T : Entity<TId>
    {
    }
}
