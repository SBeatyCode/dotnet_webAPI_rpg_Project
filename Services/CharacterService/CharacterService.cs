using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rpg_Class_Project.Models;

namespace rpg_Class_Project.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private static List<Character> exampleCharacters = new List<Character>
        {
            new Character{Strength = 12, Defense = 12, Class = RpgClass.Rouge},
            new Character{Id = "1", Name = "Zelda", Intellegence = 15, Class = RpgClass.Wizarad },
            new Character{Id = "2", Name = "York", Strength = 15, Defense = 15, Intellegence = 5, Class = RpgClass.Warrior},
            new Character{Id = "3", Name = "Rose", Strength = 8, Defense = 8, Intellegence = 10, Class = RpgClass.Rouge},
            new Character{Id = "4", Name = "Grandma", Strength = 5, Defense = 15, Intellegence = 12, Class = RpgClass.Druid}
        };

        public List<Character> AddCCharacter(Character character)
        {
            exampleCharacters.Add(character);
            return exampleCharacters;
        }

        public List<Character> GetAllCharacters()
        {
            return exampleCharacters;
        }

        public Character GetCharacterById(string id)
        {
            return exampleCharacters.FirstOrDefault(c => c.Id == id);
        }
    }
}