using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using rpg_Class_Project.Dtos.Character;
using rpg_Class_Project.Dtos.Character.Weapon;
using rpg_Class_Project.Dtos.Fight;
using rpg_Class_Project.Dtos.Skill;
using rpg_Class_Project.Models;

namespace rpg_Class_Project
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, GetCharacterResponseDTO>();
            CreateMap<AddCharacterResponseDTO, Character>();
            CreateMap<Weapon, GetWeaponResponseDto>();
            CreateMap<Skill, GetSkillResponseDto>();
            CreateMap<Character, HighscoreDto>();
        }
    }
}