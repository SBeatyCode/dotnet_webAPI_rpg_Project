using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using rpg_Class_Project.Data;
using rpg_Class_Project.Dtos.Fight;
using rpg_Class_Project.Models;

namespace rpg_Class_Project.Services.FightService
{
    public class FightService : IFightService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public FightService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto weaponAttackDto)
        {
            var result = new ServiceResponse<AttackResultDto>();

            try
            {
                var attacker  = await _context.Characters
                    .Include(c => c.Weapon)
                    .FirstOrDefaultAsync(c => c.Id == weaponAttackDto.AttackerId);

                var opponent  = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Id == weaponAttackDto.OpponentId);

                if(attacker == null || opponent == null)
                {
                    result.Data = null;
                    result.Success = false;
                    result.Message = $"One of the characters with the ID of {weaponAttackDto.AttackerId} or {weaponAttackDto.OpponentId} could not be found";
                }
                else
                {
                    opponent.Fights++;
                    attacker.Fights++;

                    int damage = attacker.Strength;
                    if(attacker.Weapon != null)
                        damage += attacker.Weapon.Damage;

                    damage -= opponent.Defense;

                    if(damage > 0)
                        opponent.HitPoints -= damage;

                    if(opponent.HitPoints <= 0)
                    {
                        result.Message = $"{opponent.Name} has been defeated!";
                        opponent.HitPoints = 0;
                        attacker.Victories++;
                        opponent.Defeats++;
                    }
                    else
                    {
                        result.Message = $"{opponent.Name} has taken {damage} point of damage!";
                    }

                    result.Data = new AttackResultDto
                    {
                        Attacker = attacker.Name,
                        Opponent = opponent.Name,
                        AttackerHP = attacker.HitPoints,
                        OpponentHP = opponent.HitPoints,
                        Damage = damage
                    };

                    result.Success = true;

                    await _context.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                    result.Data = null;
                    result.Success = false;
                    result.Message = $"An Exception occured: " + ex.Message;
            }

            return result;
        }

        public async Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto skillAttackDto)
        {
            var result = new ServiceResponse<AttackResultDto>();

            try
            {
                var attacker  = await _context.Characters
                    .Include(c => c.Skills)
                    .FirstOrDefaultAsync(c => c.Id == skillAttackDto.AttackerId);

                var opponent  = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Id == skillAttackDto.OpponentId);

                if(attacker == null || opponent == null)
                {
                    result.Data = null;
                    result.Success = false;
                    result.Message = $"One of the characters with the ID of {skillAttackDto.AttackerId} or {skillAttackDto.OpponentId} could not be found";
                }
                else if(attacker.Skills == null)
                {
                    result.Data = null;
                    result.Success = false;
                    result.Message = $"The characters with the ID of {skillAttackDto.AttackerId} has no Skills set";
                }
                else
                {
                    var skill = attacker.Skills.FirstOrDefault(s => s.Id == skillAttackDto.SkillId);

                    if(skill == null)
                    {
                        result.Data = null;
                        result.Success = false;
                        result.Message = $"The Skill with the ID {skillAttackDto.SkillId} could not be found.";
                    }
                    else
                    {
                        opponent.Fights++;
                        attacker.Fights++;

                        int damage = skill.Damage + attacker.Intellegence;

                        damage -= opponent.Defense;

                        if(damage > 0)
                            opponent.HitPoints -= damage;

                        if(opponent.HitPoints <= 0)
                        {
                            result.Message = $"{opponent.Name} has been defeated!";
                            opponent.HitPoints = 0;
                            attacker.Victories++;
                            opponent.Defeats++;
                        }
                        else
                        {
                            result.Message = $"{opponent.Name} has taken {damage} point of damage!";
                        }

                        result.Data = new AttackResultDto
                        {
                            Attacker = attacker.Name,
                            Opponent = opponent.Name,
                            AttackerHP = attacker.HitPoints,
                            OpponentHP = opponent.HitPoints,
                            Damage = damage
                        };

                        result.Success = true;

                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch(Exception ex)
            {
                result.Data = null;
                result.Success = false;
                result.Message = $"An Exception occured: " + ex.Message;
            }

            return result;
        }

        public async Task<ServiceResponse<FightResultDto>> Fight(FightRequestDto fightRequestDto)
        {
            var result = new ServiceResponse<FightResultDto>()
            {
                Data = new FightResultDto()
            };

            try
            {
                var characters = await _context.Characters
                    .Include(c => c.Weapon)
                    .Include(c => c.Skills)
                    .Where(c => fightRequestDto.CharacterIds.Contains(c.Id))
                    .ToListAsync();

                if(characters == null || characters.Count <= 0)
                {
                    result.Data = null;
                    result.Success = false;
                    result.Message = $"Characters could not be retrived.";
                }
                else
                {
                    bool stopFight = false;
                    while(!stopFight)
                    {
                        foreach(var attacker in characters)
                        {
                            if(attacker.HitPoints <= 0)
                                continue;

                            var opponents = characters.Where(c => c.Id != attacker.Id && c.HitPoints > 0).ToList();

                            int damage = 0;
                            bool useSkill = false;

                            if(attacker.Skills != null && attacker.Skills.Count > 0)
                            {
                                    useSkill = true;
                            }

                            if(useSkill)
                            {
                                int skillID = new Random().Next(attacker.Skills!.Count);
                                damage += attacker.Skills[skillID].Damage;
                                damage += attacker.Intellegence;

                                result.Data!.Log.Add($"{attacker.Name} attacks with their {attacker.Skills[skillID].Name} skill!");
                            }
                            else
                            {
                                damage = attacker.Strength;
                                if(attacker.Weapon != null)
                                {
                                    damage+= attacker.Weapon.Damage;
                                    result.Data!.Log.Add($"{attacker.Name} attacks with their weapon, {attacker.Weapon.Name}!");
                                }
                                else
                                {
                                    result.Data!.Log.Add($"{attacker.Name} attacks unarmed!");
                                }
                            }
                            
                            foreach(var opponent in opponents)
                            {
                                attacker.Fights++;
                                opponent.Fights++;

                                opponent.HitPoints = (damage - opponent.Defense);

                                result.Data.Log.Add($"{attacker.Name} attacks {opponent.Name} for {damage} damage!");

                                if(opponent.HitPoints <= 0)
                                {
                                    opponent.HitPoints = 0;
                                    opponent.Defeats++;
                                    attacker.Victories++;
                                    stopFight = true;
                                    result.Data.Log.Add($"{opponent.Name} has been defeated!");
                                    result.Data.Log.Add($"The Fight has finished!");
                                    break;
                                }
                            }
                            if(stopFight)
                                break;

                            stopFight = true;
                        }
                    }
                    result.Success = true;
                    result.Message = "The fight has been conducted!";


                    foreach(Character character in characters)
                    {
                        character.HitPoints = 100;
                    }

                    await _context.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                result.Data = null;
                result.Success = false;
                result.Message = $"An Exception occured: " + ex.Message;
            }

            return result;
        }

        public async Task<ServiceResponse<List<HighscoreDto>>> GetHighScore()
        {
            var result = new ServiceResponse<List<HighscoreDto>>();

            var characters = await _context.Characters
                .Where(c => c.Fights > 0)
                .OrderByDescending(c => c.Victories)
                .ThenBy(c => c.Defeats)
                .ToListAsync();

            if(characters == null)
            {
                result.Data = null;
                result.Success = false;
                result.Message = $"Could not retrieve characters, OR no fight data available currently.";
            }
            else
            {
                result = new ServiceResponse<List<HighscoreDto>>()
                {
                    Data = characters.Select(c => _mapper.Map<HighscoreDto>(c)).ToList()
                };
                result.Success = true;
                result.Message = "Fight results have been generated";
            }

            return result;
        }
    }
}