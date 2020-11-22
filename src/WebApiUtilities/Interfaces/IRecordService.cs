using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiUtilities.Interfaces
{
    public interface IRecordService<T, TId>
        where T : IEntity<TId>
    {
        Task<IQueryable<T>> GetAll();
        Task<T> Get(TId id);
        Task<T> Create(T record);
        Task<T> Update(T record);
        Task<bool> Delete(TId id);
    }
}
