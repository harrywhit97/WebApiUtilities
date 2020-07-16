﻿using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApiUtilities.CrudRequests;
using WebApiUtilities.Exceptions;

namespace WebApiUtilities.Abstract
{
    public abstract class CrudController<T, TId, TCreateCommand, TUpdateCommand> : ApiController
        where T : Entity<TId>
        where TCreateCommand : ICreateCommand<T, TId>
        where TUpdateCommand : IUpdateCommand<T, TId>
    {
        //public CrudController(DbContext context, ILogger logger)
        //    :base(context, logger)
        //{
        //}

        protected readonly ILogger Logger;

        public CrudController(DbContext context, ILogger logger)
        {
            Logger = logger;
            context.Database.EnsureCreated(); //move this
        }

        [HttpGet]
        [EnableQuery]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public virtual IQueryable<T> Get()
        {
            Logger.LogDebug("Recieved Get request");
            return Mediator.Send(new GetEntities<T, TId>()).Result;
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<IActionResult> Get(TId Id)
        {
            Logger.LogDebug("Recieved GetById request");

            try
            {
                return Ok(await Mediator.Send(new GetEntityById<T, TId>(Id)));
            }
            catch (NotFoundException e)
            {
                Logger.LogError(e, "There was an error processing a GetById request");
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "There was an error processing a GetById request");
                return BadRequest(e.Message);
            }
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
        public async Task<IActionResult> Delete(TId Id)
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