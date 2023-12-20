using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rpg_Class_Project.Dtos.Fight
{
    public class AttackResultDto
    {
        public string Attacker {get; set;} = string.Empty;
        public string Opponent {get; set;} = string.Empty;
        public int AttackerHP {get; set;}
        public int OpponentHP {get; set;}
        public int Damage {get; set;}
    }
}