using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkAPI_1030.Models;
using ParkAPI_1030.Models.DTOs;
using ParkAPI_1030.Repository.iRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkAPI_1030.Controllers
{
    [Route("api/Trail")]
    [ApiController]
    [Authorize]

    public class TrailController : ControllerBase
    {
        private readonly iTrailRepository _trailRepository;
        private readonly IMapper _mapper;
        public TrailController(iTrailRepository trailRepository,IMapper mapper)
        {
            _trailRepository = trailRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetTrails()
        {
            return Ok(_trailRepository.GetTrails().ToList().Select(_mapper.Map<Trail, TrailDto>));
        }

        [HttpGet("{trailId:int}", Name = "GetTrail")]
        public IActionResult GetTrail(int trailId)
        {
            var trail = _trailRepository.GetTrail(trailId);
            if (trail == null) return NotFound();
            var trailDto = _mapper.Map<Trail>(trail);
            return Ok(trailDto);
        }
        [HttpPost]
        public IActionResult CreateTrail([FromBody]TrailDto trailDto)
        {
            if (trailDto == null) return BadRequest(ModelState);
            if (!ModelState.IsValid) return BadRequest();
            if(_trailRepository.TrailExists(trailDto.Name))
            {
                ModelState.AddModelError("", $"Trail already in DB ");  
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            var trail = _mapper.Map<TrailDto,Trail>(trailDto);
            if(!_trailRepository.CreateTrail(trail))
            {
                ModelState.AddModelError("", $"Trail already in DB :{trailDto.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            // return Ok();
            return CreatedAtRoute("GetTrail", new { trailId = trail.Id }, trail);
        }
        //for update
        [HttpPut]
        public IActionResult UpdateTrail([FromBody]TrailDto trailDto)
        {
            if (trailDto == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest();
            var trail = _mapper.Map<Trail>(trailDto);
            if(! _trailRepository.UpdateTrail(trail))
            {
                ModelState.AddModelError("", $"something went wrong while update trail :{trail.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return NoContent();
        }
        [HttpDelete("{trailId:int}")]
        public IActionResult DeleteTrail(int trailId)
        {
            if (!_trailRepository.TrailExists(trailId)) return NotFound();
            var trail = _trailRepository.GetTrail(trailId);
            if (trail == null) return NotFound();
            if (!_trailRepository.DeleteTrail(trail))
            {
                ModelState.AddModelError("", $"Something went wrong while delete data:{trail.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();

        }
    }
}
