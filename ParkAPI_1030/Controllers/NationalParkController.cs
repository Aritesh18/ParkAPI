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
    [Route("api/NationalPark")]                  // [Route("api/[controller]")]  agar hum ese controller call kare to bd ma controller ka name change kr diya to user us url se isko access nahi kr payega so use controller name here
    [ApiController]
    [Authorize]
    public class NationalParkController : Controller //also use here  ControllerBase
    {
        private readonly iNationalParkRepository _nationalParkRepository;
        private readonly IMapper _mapper;
        public NationalParkController(iNationalParkRepository nationalParkRepository, IMapper mapper)
        {
            _nationalParkRepository = nationalParkRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetNationalParks()
        {
            var nationalParkListDto = _nationalParkRepository.GetNationalParks().ToList().Select(_mapper.Map<NationalPark, NationalParkDto>);
            return Ok(nationalParkListDto);    //200
        }
        [HttpGet("{nationalParkId:int}", Name = "GetNationalPark")]
        public IActionResult GetNationalPark(int nationalParkId)
        {
            var nationalPark = _nationalParkRepository.GetNationalPark(nationalParkId);
            if (nationalPark == null) return NotFound();
            var nationalParkDto = _mapper.Map<NationalParkDto>(nationalPark);
            return Ok(nationalParkDto);
        }
        [HttpPost]
        public IActionResult CreateNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null) return BadRequest(ModelState);
            if (_nationalParkRepository.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError("", "National Park Already in DB");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var natioanlPark = _mapper.Map<NationalParkDto, NationalPark>(nationalParkDto);
            if (!_nationalParkRepository.CreateNationalPark(natioanlPark))
            {
                ModelState.AddModelError("", $"Something went wrong while save data:{natioanlPark.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            //return Ok();
            return CreatedAtRoute("GetNationalPark", new { nationalParkId = natioanlPark.Id }, natioanlPark);
        }
        [HttpPut]
        public IActionResult UpdateNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null) return BadRequest(ModelState); //400
            if (!ModelState.IsValid) return BadRequest(ModelState);
            //Now we have to convert Model into Dto(NationalPark to NationalParkDto)
            var nationalPark = _mapper.Map<NationalPark>(nationalParkDto);
            if (!_nationalParkRepository.UpdateNationalPark(nationalPark))
            {
                ModelState.AddModelError("", $"Something went wrong while update data:{nationalPark.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return NoContent(); //204
        }
        [HttpDelete("{nationalParkId:int}")]
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            if (!_nationalParkRepository.NationalParkExists(nationalParkId)) return NotFound();
            var nationalPark = _nationalParkRepository.GetNationalPark(nationalParkId);
            if (nationalPark == null) return NotFound();
            if (!_nationalParkRepository.DeleteNationalPark(nationalPark))
            {
                ModelState.AddModelError("", $"Something went wrong while delete data:{nationalPark.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }
    }
}