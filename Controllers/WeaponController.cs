using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rpg_Class_Project.Dtos.Character;
using rpg_Class_Project.Dtos.Character.Weapon;
using rpg_Class_Project.Models;
using rpg_Class_Project.Services.WeaponService;

namespace rpg_Class_Project.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeaponController : ControllerBase
    {
        private IWeaponService _weaponService;

        public WeaponController(IWeaponService weaponService)
        {
            _weaponService = weaponService;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<GetCharacterResponseDTO>>> AddWeapon(AddWeaponResponseDto newWeapon)
        {
            var response = await _weaponService.AddWeapon(newWeapon);

            if(response.Data == null)
                return NotFound(response);
            else
                return Ok(response);
        }
    }
}