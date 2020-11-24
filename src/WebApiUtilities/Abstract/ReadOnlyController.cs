using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApiUtilities.Exceptions;
using WebApiUtilities.Interfaces;

namespace WebApiUtilities.Abstract
{
    public abstract class ReadOnlyController<T, TId> : ApiController
        where T : Entity<TId>
    {
        private readonly IRecordService<T, TId> _service;
        protected readonly ILogger _log;

        public ReadOnlyController(IRecordService<T, TId> service, ILogger log)
        {
            _service = service;
            _log = log;
        }

        [HttpGet]
        [EnableQuery]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public virtual IQueryable<T> Get()
        {
            _log.LogDebug("Recieved Get request");
            return _service.GetAll().Result;
        }

        [HttpGet("{id}")]
        [EnableQuery]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult<T>> GetById(TId id)
        {
            _log.LogDebug("Recieved GetById request");

            try
            {
                return Ok(await _service.Get(id));
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
    }
}