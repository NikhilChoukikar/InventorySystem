using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using global::InventorySystem.Domain.Entities;
using InventorySystem.Domain.Entities;


namespace InventorySystem.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);


        //  Task<int> CreateAsync(User user);
        Task<User?> GetByIdAsync(int id);


        Task SaveRefreshTokenAsync(int userId, string token, DateTime expiresAt);
        Task<int?> ValidateRefreshTokenAsync(string token);
    }

    

    }

