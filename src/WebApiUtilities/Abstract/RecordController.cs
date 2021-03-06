﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WebApiUtilities.Exceptions;
using WebApiUtilities.Interfaces;

namespace WebApiUtilities.Abstract
{
    public abstract class RecordController<T, TId, TDto> : ReadOnlyController<T, TId>
        where T : Entity<TId>
        where TDto : Dto<T, TId>
    {
        private readonly IRecordService<T, TId> _service;
        private readonly IMapper _mapper;

        public RecordController(IRecordService<T, TId> service, ILogger log, IMapper mapper)
            :base(service, log)
        {
            _service = service;
            _mapper = mapper;
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

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(TId id)
        {
            _log.LogDebug("Recieved Delete request");
            try
            {
                await _service.Delete(id);
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