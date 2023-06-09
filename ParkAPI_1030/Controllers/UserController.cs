using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkAPI_1030.Models;
using ParkAPI_1030.Repository.iRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkAPI_1030.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly iUserRepository _userRepository;
        public UserController(iUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpPost("register")]
        public IActionResult Register([FromBody]User user)
        {
            if(ModelState.IsValid)
            {
                var isUniqueUser = _userRepository.IsUniqueUser(user.UserName);
                if (!isUniqueUser)
                    return BadRequest("User in use!!!");
                var userInfo = _userRepository.Register(user.UserName, user.Password);
                if (userInfo == null) return BadRequest();
            }
            return Ok();
        }
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]UserVM userVM)
        {
            var user = _userRepository.Authenticate(userVM.UserName, userVM.Password);
            if (user == null) return BadRequest("wrong user/pwd");
            return Ok(user);
        }



  } 
}

