using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApiUtilities.CrudRequests;
using WebApiUtilities.Exceptions;

namespace WebApiUtilities.Abstract
{
    public abstract class ReadOnlyController<T, TId> : ApiController
        where T : Entity<TId>
    {
        protected readonly ILogger Logger;

        public ReadOnlyController(DbContext context, ILogger logger)
        {
            Logger = logger;
            context.Database.EnsureCreated(); //move this
        }

        [HttpGet]
        [EnableQuery]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IQueryable<T> Get()
        {
            Logger.LogDebug("Recieved Get request");
            return Mediator.Send(new GetEntities<T, TId>()).Result;
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById(TId Id)
        {
            Logger.LogDebug("Recieved GetById request");

            try
            {
                return Ok(await Mediator.Send(new GetEntityById<T, TId>(Id)));
            }
            catch (NotFoundException e)
            {
                Logger.LogError(e, "There was an error processing a Get request");
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "There was an error processing a Get request");
                return BadRequest(e.Message);
            }
        }
    }
}
