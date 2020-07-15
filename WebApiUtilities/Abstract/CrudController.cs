using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebApiUtilities.CrudRequests;
using WebApiUtilities.Exceptions;

namespace WebApiUtilities.Abstract
{
    public abstract class CrudController<T, TId, TDto, TCreateCommand, TUpdateCommand> : ReadOnlyController<T, TId>
        where T : Entity<TId>
        where TDto : class
        where TCreateCommand : ICreateCommand<T, TId>
        where TUpdateCommand : IUpdateCommand<T, TId>
    {
        public CrudController(DbContext context, ILogger logger)
            :base(context, logger)
        {
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] TCreateCommand dto)
        {
            Logger.LogDebug("Recieved Post request");
            try
            {
                var entity = await Mediator.Send(dto);
                return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "There was an error processing a Post request");
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromBody] TUpdateCommand dto)
        {
            Logger.LogDebug("Recieved Put request");

            try
            {
                return Ok(await Mediator.Send(dto));
            }
            catch (NotFoundException e)
            {
                Logger.LogError(e, "There was an error processing a Put request");
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "There was an error processing a Put request");
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<IActionResult> Delete(TId Id)
        {
            Logger.LogDebug("Recieved Delete request");

            try
            {
                await Mediator.Send(new DeleteEntity<T, TId>(Id), new CancellationToken());
                return Ok();
            }
            catch (NotFoundException e)
            {
                Logger.LogError(e, "There was an error processing a Delete request");
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "There was an error processing a Delete request");
                return BadRequest(e.Message);
            }
        }
    }
}