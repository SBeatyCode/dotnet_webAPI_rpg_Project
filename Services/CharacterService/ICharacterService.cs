using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rpg_Class_Project.Dtos.Character;
using rpg_Class_Project.Models;

namespace rpg_Class_Project.Services.CharacterService
{
    public interface ICharacterService
    {
        Task<ServiceResponse<List<GetCharacterResponseDTO>>> GetAllCharacters();
        Task<ServiceResponse<GetCharacterResponseDTO>> GetCharacterById(string id);
        Task<ServiceResponse<List<GetCharacterResponseDTO>>> AddCharacter(AddCharacterResponseDTO character);
        Task<ServiceResponse<GetCharacterResponseDTO>> UpdateCharacter(UpdateCharacterDTO updateCharacter);
        Task<ServiceResponse<GetCharacterResponseDTO>> DeleteCharacter(string id);
    }
}