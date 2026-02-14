using BCrypt.Net;
using FluentValidation;
using InventorySystem.Application.DTOs.Auth;
using InventorySystem.Application.Interfaces.Repositories;
using InventorySystem.Application.Interfaces.Security;
using InventorySystem.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventorySystem.Application.Common.Exceptions;
using BCrypt.Net;


namespace InventorySystem.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwt;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly IValidator<LoginRequestDto> _loginValidator;

        public AuthService(
            IUserRepository userRepository,
            IJwtTokenGenerator jwt,
            IRefreshTokenGenerator refreshTokenGenerator,
            IValidator<LoginRequestDto> loginValidator)
        {
            _userRepository = userRepository;
            _jwt = jwt;
            _refreshTokenGenerator = refreshTokenGenerator;
            _loginValidator = loginValidator;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto)
        {
            var validationResult = await _loginValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                throw new FluentValidation.ValidationException(validationResult.Errors);

            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null) return null;

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return null;

            var accessToken = _jwt.GenerateToken(user.Id, user.Email, (int)user.Role);
            var refreshToken = _refreshTokenGenerator.Generate();
            var refreshExpiry = DateTime.UtcNow.AddDays(7);

            await _userRepository.SaveRefreshTokenAsync(
                user.Id,
                refreshToken,
                refreshExpiry
            );

            return new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15)
            };
        }

        public async Task<int?> ValidateRefreshTokenAsync(string refreshToken)
        {
            return await _userRepository.ValidateRefreshTokenAsync(refreshToken);
        }

        public async Task<LoginResponseDto?> RefreshTokenAsync(string refreshToken)
        {
            var userId = await _userRepository.ValidateRefreshTokenAsync(refreshToken);
            if (userId == null)
                return null;

            var user = await _userRepository.GetByIdAsync(userId.Value);
            if (user == null)
                return null;

            var newAccessToken = _jwt.GenerateToken(
                user.Id,
                user.Email,
               (int)user.Role
            );

            return new LoginResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15)
            };
        }
    }
}
    

