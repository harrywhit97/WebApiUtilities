using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebApiUtilities.Exceptions;
using WebApiUtilities.Interfaces;

namespace WebApiUtilities.Abstract
{
    public class BaseRecordService<TDbContext, T, TId> : IRecordService<T, TId>
        where T : Entity<TId>
        where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;

        public BaseRecordService(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> Create(T record)
        {
            _dbContext.Set<T>().Add(record);
            await _dbContext.SaveChangesAsync();
            return record;
        }

        public async Task<bool> Delete(TId id)
        {
            var entity = await _dbContext.Set<T>().FindAsync(id)
                            ?? throw new NotFoundException(typeof(T).Name, id);

            _dbContext.Set<T>().Remove(entity);
            _dbContext.SaveChanges();
            return true;
        }

        public async Task<T> Get(TId id)
        {
            return await _dbContext.Set<T>().FindAsync(id)
               ?? throw new NotFoundException(typeof(T).Name, id);
        }

        public async Task<IQueryable<T>> GetAll()
        {
            return await Task.FromResult(_dbContext.Set<T>().AsQueryable());
        }

        public async Task<T> Update(T record)
        {
            var existingEntity = await _dbContext.Set<T>().FindAsync(record.Id)
                            ?? throw new NotFoundException(typeof(T).Name, record.Id);

            _dbContext.Entry(existingEntity).State = EntityState.Detached;


            _dbContext.Entry(record).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return record;
        }
    }
}
