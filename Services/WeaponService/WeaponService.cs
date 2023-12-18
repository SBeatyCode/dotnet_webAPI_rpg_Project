using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rpg_Class_Project.Dtos.Character;
using rpg_Class_Project.Models;
using rpg_Class_Project.Dtos.Character.Weapon;
using AutoMapper;
using rpg_Class_Project.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace rpg_Class_Project.Services.WeaponService
{
    public class WeaponService : IWeaponService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccesor;

        public WeaponService(IMapper mapper, DataContext dataContext, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context = dataContext;
            _httpContextAccesor = httpContextAccessor;
        }

        public async Task<ServiceResponse<GetCharacterResponseDTO>> AddWeapon(AddWeaponResponseDto newWeapon)
        {
            ServiceResponse<GetCharacterResponseDTO> response = new ServiceResponse<GetCharacterResponseDTO>();

            try
            {
                var character = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Id == newWeapon.CharacterId &&
                        c.User!.Id == int.Parse(_httpContextAccesor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!));

                if(character == null)
                {
                    response.Data = null;
                    response.Success = false;
                    response.Message = $"A character with the id of {newWeapon.CharacterId} could not be found.";
                }
                else
                {
                    Weapon weapon = new Weapon
                    {
                        Name = newWeapon.Name,
                        Damage = newWeapon.Damage,
                        Character = character
                    };

                    _context.Weapons.Add(weapon);
                    await _context.SaveChangesAsync();

                    response.Data = _mapper.Map<GetCharacterResponseDTO>(character);
                    response.Success = true;
                    response.Message = $"The weapon {weapon.Name} was given to {character.Name} succesfully!";
                }
            }
            catch(Exception ex)
            {
                    response.Data = null;
                    response.Success = false;
                    response.Message = "An exception occured: " + ex.Message;
            }


            return response;
        }
    }
}