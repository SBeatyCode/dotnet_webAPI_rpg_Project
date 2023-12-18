using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rpg_Class_Project.Dtos.Character.Weapon
{
    public class AddWeaponResponseDto
    {
        public string Name {get; set;} = string.Empty;
        public int Damage {get; set;}
        public int CharacterId {get; set;}
    }
}