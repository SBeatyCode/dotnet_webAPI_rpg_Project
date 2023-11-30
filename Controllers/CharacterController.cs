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
        }

        [HttpGet("GetAllCharacters")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterResponseDTO>>>> GetAllCharacters()
        {
            var response = await _characterService.GetAllCharacters();
            
            if(response.Data == null)
                return NotFound(response);
            else
                return Ok(response);
        }

        [HttpGet("GetCharacter/{id}")]
        public async Task<ActionResult<ServiceResponse<GetCharacterResponseDTO>>> GetCharacter(string id)
        {
            var response = await _characterService.GetCharacterById(id);

            if(response.Data == null)
                return NotFound(response);
            else
                return Ok(response);
        }

        [HttpPost("AddCharacter")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterResponseDTO>>>> AddCharacter(AddCharacterResponseDTO character)
        {
            return Ok(await _characterService.AddCharacter(character));
        }

        [HttpPut("UpdateCharacter")]
        public async Task<ActionResult<ServiceResponse<GetCharacterResponseDTO>>> UpdateCharacter(UpdateCharacterDTO updateCharacter)
        {
            var response = await _characterService.UpdateCharacter(updateCharacter);

            if(response.Data == null)
                return NotFound(response);
            else
                return Ok(response);
        }

        [HttpDelete("DeleteCharacter/{id}")]
        public async Task<ActionResult<ServiceResponse<GetCharacterResponseDTO>>> DeleteCharacter(string id)
        {
            var response = await _characterService.DeleteCharacter(id);

            if(response.Data == null)
                return NotFound(response);
            else
                return Ok(response);
        }
    }
}