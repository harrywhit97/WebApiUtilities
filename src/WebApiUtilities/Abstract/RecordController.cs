using AutoMapper;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApiUtilities.CrudRequests;
using WebApiUtilities.Exceptions;
using WebApiUtilities.Interfaces;

namespace WebApiUtilities.Abstract
{
    public abstract class RecordController<T, TId, TDto> : ApiController
        where T : Entity<TId>
        where TDto : Dto<T, TId>
    {
        private readonly IRecordService<T, TId> _service;
        private readonly ILogger _log;
        private readonly IMapper _mapper;


        public RecordController(IRecordService<T, TId> service, ILogger log, IMapper mapper)
        {
            _service = service;
            _log = log;
            _mapper = mapper;
        }

        [HttpGet]
        [EnableQuery]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public virtual IQueryable<T> Get()
        {
            _log.LogDebug("Recieved Get request");
            return _service.GetAll().Result;
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult<T>> GetById(TId Id)
        {
            _log.LogDebug("Recieved GetById request");

            try
            {
                return Ok(await _service.Get(Id));
            }
            catch (NotFoundException e)
            {
                _log.LogError(e, "There was an error processing a GetById request");
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                _log.LogError(e, "There was an error processing a GetById request");
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] TDto dto)
        {
            _log.LogDebug("Recieved Post request");
            try
            {
                var record = _mapper.Map<T>(dto);
                var entity = await _service.Create(record);
                return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
            }
            catch (Exception e)
            {
                _log.LogError(e, "There was an error processing a Post request");
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromBody] TDto dto)
        {
            _log.LogDebug("Recieved Put request");

            try
            {
                var record = _mapper.Map<T>(dto);
                return Ok(await _service.Update(record));
            }
            catch (NotFoundException e)
            {
                _log.LogError(e, "There was an error processing a Put request");
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                _log.LogError(e, "There was an error processing a Put request");
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(TId Id)
        {
            _log.LogDebug("Recieved Delete request");

            try
            {
                await Mediator.Send(new DeleteCommand<T, TId>(Id), new CancellationToken());
                return Ok();
            }
            catch (NotFoundException e)
            {
                _log.LogError(e, "There was an error processing a Delete request");
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                _log.LogError(e, "There was an error processing a Delete request");
                return BadRequest(e.Message);
            }
        }
    }
}