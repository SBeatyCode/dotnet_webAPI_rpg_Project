using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rpg_Class_Project.Models;

namespace rpg_Class_Project.Services.CharacterService
{
    public interface ICharacterService
    {
        List<Character> GetAllCharacters();
        Character GetCharacterById(string id);
        List<Character> AddCCharacter(Character character);
    }
}