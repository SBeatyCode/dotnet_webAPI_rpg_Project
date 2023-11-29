using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using rpg_Class_Project.Dtos.Character;
using rpg_Class_Project.Models;
using rpg_Class_Project.Services.CharacterService;

namespace rpg_Class_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _characterService;

        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
            //
        }

        [HttpGet("GetAllCharacters")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterResponseDTO>>>> GetAllCharacters()
        {
            return Ok(await _characterService.GetAllCharacters());
        }

        [HttpGet("GetCharacter/{id}")]
        public async Task<ActionResult<ServiceResponse<GetCharacterResponseDTO>>> GetCharacter(string id)
        {
            return Ok(await _characterService.GetCharacterById(id));
        }

        [HttpPost("AddCharacter")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterResponseDTO>>>> AddCharacter(AddCharacterResponseDTO character)
        {
            return Ok(await _characterService.AddCharacter(character));
        }
    }
}