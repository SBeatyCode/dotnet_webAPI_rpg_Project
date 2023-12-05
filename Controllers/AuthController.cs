using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using rpg_Class_Project.Data;
using rpg_Class_Project.Dtos.Character.User;
using rpg_Class_Project.Models;

namespace rpg_Class_Project.Controllers
{
    [ApiController]
    [Route("controller")]
    public class AuthController : ControllerBase
    {

        private readonly IAuthRepository _authRepository;
        
        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto request)
        {
            var response = await _authRepository.Register(
                new User{Username = request.Username},
                request.Password
            );

            if(response.Success)
                return Ok(response);
            else
                return BadRequest(response);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(UserRegisterDto request)
        {
            var response = await _authRepository.Login(request.Username, request.Password);

            if(response.Success)
                return Ok(response);
            else
                return BadRequest(response);
        }
    }
}