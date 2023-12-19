using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Internal;
using Microsoft.AspNetCore.Http;
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
        private readonly IHttpContextAccessor _httpContextAccesor;

        public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor contextAccesor)
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccesor = contextAccesor;
        }

        public async Task<ServiceResponse<List<GetCharacterResponseDTO>>> AddCharacter(AddCharacterResponseDTO newCharacter)
        {
            ServiceResponse<List<GetCharacterResponseDTO>> serviceResponse = new ServiceResponse<List<GetCharacterResponseDTO>>();
            Character character = _mapper.Map<Character>(newCharacter);
            character.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserID());

            _context.Characters.Add(character);
            await _context.SaveChangesAsync();

            serviceResponse.Data = await _context.Characters
                .Where(c => c.User!.Id == GetUserID())
                .Select(c => _mapper.Map<GetCharacterResponseDTO>(c))
                .ToListAsync();
            serviceResponse.Success = true;
            serviceResponse.Message = $"Sucessfully added new character, '{character.Name}'!";

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterResponseDTO>>> GetAllCharacters()
        {
            ServiceResponse<List<GetCharacterResponseDTO>> serviceResponse = new ServiceResponse<List<GetCharacterResponseDTO>>();
            var dbCharacters = await _context.Characters
            .Include(c => c.Weapon)
            .Include(c => c.Skills)
            .Where(c => c.User!.Id == GetUserID()).ToListAsync();

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
            var foundCharacter = await _context.Characters
            .Include(c => c.Weapon)
            .Include(c => c.Skills)
            .FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserID());
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
            var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == updateCharacter.Id && c.User!.Id == GetUserID());

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
            var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserID());

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

        public async Task<ServiceResponse<GetCharacterResponseDTO>> AddCharacterSkill(AddCharacterSkillDto addCharacterSkillDto)
        {
            var response = new ServiceResponse<GetCharacterResponseDTO>();

            try
            {
                var character = await _context.Characters
                    .Include(c => c.Weapon)
                    .Include(c => c.Skills)
                    .FirstOrDefaultAsync(c => c.Id == addCharacterSkillDto.CharacterId &&
                        c.User!.Id == GetUserID());

                if(character == null)
                {
                    response.Data = null;
                    response.Success = false;
                    response.Message = $"Could not find a character with the Id of {addCharacterSkillDto.CharacterId}";
                }
                else
                {
                    var skill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == addCharacterSkillDto.SkillId);

                    if(skill == null)
                    {
                        response.Data = null;
                        response.Success = false;
                        response.Message = $"Could not find a skill with the Id of {addCharacterSkillDto.SkillId}";
                    }
                    else
                    {
                        character.Skills.Add(skill);
                        await _context.SaveChangesAsync();
                        
                        response.Data = _mapper.Map<GetCharacterResponseDTO>(character);
                        response.Success = true;
                        response.Message = $"Sucesfully added Skill with the Id {addCharacterSkillDto.SkillId} to character of Id {addCharacterSkillDto.CharacterId}";
                    }
                }
            }
            catch(Exception ex)
            {
                    response.Data = null;
                    response.Success = false;
                    response.Message = $"An Exception occured: " + ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Gets the UserID of the currently signed-in and authenticated User from HTTPContextAccesor as an int
        /// </summary>
        private int GetUserID() => int.Parse(_httpContextAccesor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}