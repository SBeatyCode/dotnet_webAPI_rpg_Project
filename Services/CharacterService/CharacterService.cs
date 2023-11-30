using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Internal;
using rpg_Class_Project.Dtos.Character;
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
        private readonly IMapper _mapper;

        public CharacterService(IMapper mapper)
        {
            _mapper = mapper;
            //
        }

        public async Task<ServiceResponse<List<GetCharacterResponseDTO>>> AddCharacter(AddCharacterResponseDTO character)
        {
            ServiceResponse<List<GetCharacterResponseDTO>> serviceResponse = new ServiceResponse<List<GetCharacterResponseDTO>>();
            Character newCharacter = _mapper.Map<Character>(character);
            newCharacter.Id = exampleCharacters.Count().ToString(); //because of zero-index this gives the next available id
            exampleCharacters.Add(newCharacter);
            //serviceResponse.Data = _mapper.Map<List<GetCharacterResponseDTO>>(exampleCharacters); //Possible other solution?
            serviceResponse.Data = exampleCharacters.Select(c => _mapper.Map<GetCharacterResponseDTO>(c)).ToList();
            serviceResponse.Success = true;
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterResponseDTO>>> GetAllCharacters()
        {
            ServiceResponse<List<GetCharacterResponseDTO>> serviceResponse = new ServiceResponse<List<GetCharacterResponseDTO>>();

            if(exampleCharacters.Count() <= 0 || exampleCharacters == null)
            {
                serviceResponse.Data = null;
                serviceResponse.Success = false;
                serviceResponse.Message = "List of characters was empty or null";
            }
            else
            {
                serviceResponse.Data = exampleCharacters.Select(c => _mapper.Map<GetCharacterResponseDTO>(c)).ToList();
                serviceResponse.Success = true;
                serviceResponse.Message = "Returned list of all characters";
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterResponseDTO>> GetCharacterById(string id)
        {
            ServiceResponse<GetCharacterResponseDTO> serviceResponse = new ServiceResponse<GetCharacterResponseDTO>();
            var foundCharacter = exampleCharacters.FirstOrDefault(c => c.Id == id);
            serviceResponse.Data = _mapper.Map<GetCharacterResponseDTO>(foundCharacter);

            if(foundCharacter == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = $"Character with the id of '{id}' was not found.";
            }
            else
            {
                serviceResponse.Success = true;
                serviceResponse.Message = $"Character with the id of '{id}' was found.";
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterResponseDTO>> UpdateCharacter(UpdateCharacterDTO updateCharacter)
        {
            ServiceResponse<GetCharacterResponseDTO> serviceResponse = new ServiceResponse<GetCharacterResponseDTO>();
            Character? character = exampleCharacters.FirstOrDefault(c => c.Id == updateCharacter.Id);

            if(character != null)
            {
                character.Name = updateCharacter.Name;
                character.HitPoints = updateCharacter.HitPoints;
                character.Strength = updateCharacter.Strength;
                character.Defense = updateCharacter.Defense;
                character.Intellegence = updateCharacter.Intellegence;
                character.Class = updateCharacter.Class;

                serviceResponse.Data = _mapper.Map<GetCharacterResponseDTO>(character);
                serviceResponse.Success = true;
                serviceResponse.Message = $"The character with id of {updateCharacter.Id} has been updated!";
            }
            else
            {
                serviceResponse.Success = false;
                serviceResponse.Message = $"Character with the id of {updateCharacter.Id} could not be found.";
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterResponseDTO>> DeleteCharacter(string id)
        {
            ServiceResponse<GetCharacterResponseDTO> serviceResponse = new ServiceResponse<GetCharacterResponseDTO>();
            Character? character = exampleCharacters.FirstOrDefault(c => c.Id == id);

            if(character != null)
            {
                exampleCharacters.Remove(character);

                serviceResponse.Data = _mapper.Map<GetCharacterResponseDTO>(character);
                serviceResponse.Success = true;
                serviceResponse.Message = $"The character with id of {id} has been deleted!";
            }
            else
            {
                serviceResponse.Success = false;
                serviceResponse.Message = $"Character with the id of {id} could not be found.";
            }

            return serviceResponse;
        }
    }
}