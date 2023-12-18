using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rpg_Class_Project.Dtos.Character;
using rpg_Class_Project.Dtos.Character.Weapon;
using rpg_Class_Project.Models;

namespace rpg_Class_Project.Services.WeaponService
{
    public interface IWeaponService
    {
        Task<ServiceResponse<GetCharacterResponseDTO>> AddWeapon(AddWeaponResponseDto newWeapon);
    }
}