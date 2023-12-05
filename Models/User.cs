using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rpg_Class_Project.Models
{
    public class User
    {
        public int Id {get; set;} = 0;
        public string Username {get; set;} = string.Empty;
        public byte[] PasswordHash {get; set;} = new byte[0];
        public byte[] PasswordSalt {get; set;} = new byte[0];
        public List<Character> Characters {get; set;} = new List<Character>();
    }
}