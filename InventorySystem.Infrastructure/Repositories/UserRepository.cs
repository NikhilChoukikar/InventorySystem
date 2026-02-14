using InventorySystem.Application.Interfaces.Repositories;
using InventorySystem.Domain.Entities;
using InventorySystem.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;


namespace InventorySystem.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository   // 🔥 MUST IMPLEMENT
    {
        private readonly DapperContext _context;

    public UserRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        const string sql = @"
            SELECT *
            FROM Users
            WHERE Email = @Email AND IsActive = 1";

        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<User>(
            sql,
            new { Email = email });
    }

        public async Task SaveRefreshTokenAsync(int userId, string token, DateTime expiresAt)
        {
            var sql = @"
        INSERT INTO RefreshTokens (UserId, Token, ExpiresAt)
        VALUES (@UserId, @Token, @ExpiresAt)";

            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(sql, new
            {
                UserId = userId,
                Token = token,
                ExpiresAt = expiresAt
            });
        }

        public async Task<int?> ValidateRefreshTokenAsync(string token)
        {
            var sql = @"
        SELECT UserId FROM RefreshTokens
        WHERE Token = @Token
          AND IsRevoked = 0
          AND ExpiresAt > GETUTCDATE()";

            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<int?>(sql, new { Token = token });
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            var sql = @"
        SELECT *
        FROM Users
        WHERE Id = @Id
          AND IsActive = 1";

            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(
                sql,
                new { Id = id }
            );
        }

    }





  
}
