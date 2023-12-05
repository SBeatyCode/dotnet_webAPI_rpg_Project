using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rpg_Class_Project.Dtos.Character.User
{
    public class UserRegisterDto
    {
        public string Username {get; set;} = string.Empty;
        public string Password {get; set;} = string.Empty;
    }
}