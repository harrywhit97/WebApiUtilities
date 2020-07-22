using WebApiUtilities.Abstract;

namespace WebApiUtilities.Interfaces
{
    public interface IDto { }

    public interface IDto<T, TId> : IDto, IMapFrom<T>, IHasId<TId>
        where T : Entity<TId>
    {
    }
}
