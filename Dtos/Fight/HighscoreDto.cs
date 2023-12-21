using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rpg_Class_Project.Dtos.Fight
{
    public class HighscoreDto
    {
        public int Id {get; set;}
        public string Name {get; set;} = string.Empty;
        public int Fights {get; set;}
        public int Victories {get; set;}
        public int Defeats {get; set;}
    }
}