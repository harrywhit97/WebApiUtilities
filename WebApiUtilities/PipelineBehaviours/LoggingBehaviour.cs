using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace WebApiUtilities.PipelineBehaviours
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        readonly ILogger Logger;

        public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
        {
            Logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            Logger.LogDebug($"Recieved request of type {typeof(TRequest).Name}");

            var response = await next();

            Logger.LogDebug($"Exceuted request of type {typeof(TRequest).Name}");

            return response;
        }
    }
}
