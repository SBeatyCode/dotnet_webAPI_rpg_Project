using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Internal;
using Microsoft.EntityFrameworkCore;
using rpg_Class_Project.Data;
using rpg_Class_Project.Dtos.Character;
using rpg_Class_Project.Models;

namespace rpg_Class_Project.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public CharacterService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ServiceResponse<List<GetCharacterResponseDTO>>> AddCharacter(AddCharacterResponseDTO newCharacter)
        {
            ServiceResponse<List<GetCharacterResponseDTO>> serviceResponse = new ServiceResponse<List<GetCharacterResponseDTO>>();
            Character character = _mapper.Map<Character>(newCharacter);

            _context.Characters.Add(character);
            await _context.SaveChangesAsync();

            serviceResponse.Data = await _context.Characters.Select(c => _mapper.Map<GetCharacterResponseDTO>(c)).ToListAsync();
            serviceResponse.Success = true;
            serviceResponse.Message = $"Sucessfully added new character, '{character.Name}'!";

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterResponseDTO>>> GetAllCharacters(int userId)
        {
            ServiceResponse<List<GetCharacterResponseDTO>> serviceResponse = new ServiceResponse<List<GetCharacterResponseDTO>>();
            var dbCharacters = await _context.Characters.Where(c => c.User.Id == userId).ToListAsync();

            if(dbCharacters.Count() <= 0 || dbCharacters == null)
            {
                serviceResponse.Data = null;
                serviceResponse.Success = false;
                serviceResponse.Message = "List of characters was empty or null";
            }
            else
            {
                serviceResponse.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterResponseDTO>(c)).ToList();
                serviceResponse.Success = true;
                serviceResponse.Message = "Returned list of all characters";
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterResponseDTO>> GetCharacterById(int id)
        {
            ServiceResponse<GetCharacterResponseDTO> serviceResponse = new ServiceResponse<GetCharacterResponseDTO>();
            var foundCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);
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
            var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == updateCharacter.Id);

            if(character != null)
            {
                character.Name = updateCharacter.Name;
                character.HitPoints = updateCharacter.HitPoints;
                character.Strength = updateCharacter.Strength;
                character.Defense = updateCharacter.Defense;
                character.Intellegence = updateCharacter.Intellegence;
                character.Class = updateCharacter.Class;

                await _context.SaveChangesAsync();

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

        public async Task<ServiceResponse<GetCharacterResponseDTO>> DeleteCharacter(int id)
        {
            ServiceResponse<GetCharacterResponseDTO> serviceResponse = new ServiceResponse<GetCharacterResponseDTO>();
            var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);

            if(character != null)
            {
                serviceResponse.Data = _mapper.Map<GetCharacterResponseDTO>(character);

                _context.Characters.Remove(character);
                await _context.SaveChangesAsync();

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