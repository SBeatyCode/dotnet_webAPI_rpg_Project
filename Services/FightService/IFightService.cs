using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rpg_Class_Project.Dtos.Fight;
using rpg_Class_Project.Models;

namespace rpg_Class_Project.Services.FightService
{
    public interface IFightService
    {
        Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto weaponAttackDto);

        Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto skillAttackDto);

        Task<ServiceResponse<FightResultDto>> Fight(FightRequestDto fightRequestDto);
        Task<ServiceResponse<List<HighscoreDto>>> GetHighScore();
    }
}