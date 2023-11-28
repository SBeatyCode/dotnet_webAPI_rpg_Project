using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
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
        public ActionResult<List<Character>> Get()
        {
            return Ok(_characterService.GetAllCharacters());
        }

        [HttpGet("GetCharacter{id}")]
        public ActionResult<Character> GetCharacter(string id)
        {
            return Ok(_characterService.GetCharacterById(id));
        }

        [HttpPost("AddCharacter")]
        public ActionResult<List<Character>> AddCharacter(Character character)
        {
            return Ok(_characterService.AddCCharacter(character));
        }
    }
}