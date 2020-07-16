using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace WebApiUtilities.Abstract
{
    public abstract class AbstractRequestHandler<TRequest, TResponse, TDbContext> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TDbContext : DbContext
    {
        protected readonly TDbContext dbContext;

        public AbstractRequestHandler(TDbContext dbContext) => this.dbContext = dbContext;

        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }
}
