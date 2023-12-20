using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rpg_Class_Project.Models
{
    public class Character
    {
        public int Id {get; set;} = 0;
        public string Name {get; set;} = "Link";
        public int HitPoints {get; set;} = 10;
        public int Strength {get; set;} = 10;
        public int Defense {get; set;} = 10;
        public int Intellegence {get; set;} = 10;
        public RpgClass Class {get; set;} = RpgClass.Novice;
        public User? User {get; set;}
        public Weapon? Weapon {get; set;}
        public List<Skill>? Skills {get; set;}
        public int Fights {get; set;}
        public int Victories {get; set;}
        public int Defeats {get; set;}
    }
}