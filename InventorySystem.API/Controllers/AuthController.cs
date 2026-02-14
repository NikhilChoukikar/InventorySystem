using InventorySystem.Application.DTOs.Auth;
using InventorySystem.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using InventorySystem.Application.Services;

namespace InventorySystem.API.Controllers
{
 
   
        [ApiController]
        [Route("api/auth")]
        public class AuthController : ControllerBase
        {
            private readonly IAuthService _authService;

            public AuthController(IAuthService authService)
            {
                _authService = authService;
            }

            [HttpPost("login")]
            public async Task<IActionResult> Login(LoginRequestDto dto)
            {
                var result = await _authService.LoginAsync(dto);
                if (result == null)
                    return Unauthorized("Invalid credentials");

                return Ok(result);
            }


        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestDto dto)
        {
            var result = await _authService.RefreshTokenAsync(dto.RefreshToken);

            if (result == null)
                return Unauthorized("Invalid or expired refresh token");

            return Ok(result);
        }
    }


    }

