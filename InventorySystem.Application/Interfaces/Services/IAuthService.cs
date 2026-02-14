using global::InventorySystem.Application.DTOs.Auth;
using InventorySystem.Application.DTOs.Auth;
using InventorySystem.Application.Interfaces.Repositories;
using InventorySystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Application.Interfaces.Services
{

        public interface IAuthService
        {
            Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto);

            // 👇 ADD THIS
            Task<int?> ValidateRefreshTokenAsync(string refreshToken);
        
        // 👇 ADD THIS
        Task<LoginResponseDto?> RefreshTokenAsync(string refreshToken);



    }
}


