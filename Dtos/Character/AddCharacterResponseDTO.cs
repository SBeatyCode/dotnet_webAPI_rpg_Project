using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rpg_Class_Project.Models;

namespace rpg_Class_Project.Dtos.Character
{
    public class AddCharacterResponseDTO
    {
        //public int Id {get; set;} = 0;
        public string Name {get; set;} = "Link";
        public int HitPoints {get; set;} = 10;
        public int Strength {get; set;} = 10;
        public int Defense {get; set;} = 10;
        public int Intellegence {get; set;} = 10;
        public RpgClass Class {get; set;} = RpgClass.Novice;
    }
}