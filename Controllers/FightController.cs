using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using rpg_Class_Project.Dtos.Fight;
using rpg_Class_Project.Models;
using rpg_Class_Project.Services.FightService;

namespace rpg_Class_Project.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FightController : ControllerBase
    {
        private readonly IFightService _fightService;

        public FightController(IFightService fightService)
        {
            _fightService = fightService;
        }

        [HttpPost("Weapon")]
        public async Task<ActionResult<ServiceResponse<AttackResultDto>>> WeaponAttack(WeaponAttackDto weaponAttackDto)
        {
            var result = await _fightService.WeaponAttack(weaponAttackDto);

            if(result.Data == null)
                return NotFound(result);
            else
                return Ok(result);
        }

        [HttpPost("Skill")]
        public async Task<ActionResult<ServiceResponse<AttackResultDto>>> SkillAttack(SkillAttackDto skillAttackDto)
        {
            var result = await _fightService.SkillAttack(skillAttackDto);

            if(result.Data == null)
                return NotFound(result);
            else
                return Ok(result);
        }

        [HttpPost("Fight")]
        public async Task<ActionResult<ServiceResponse<FightResultDto>>> Fight(FightRequestDto fightRequestDto)
        {
            var result = await _fightService.Fight(fightRequestDto);

            if(result.Data == null)
                return NotFound(result);
            else
                return Ok(result);
        }
    }
}