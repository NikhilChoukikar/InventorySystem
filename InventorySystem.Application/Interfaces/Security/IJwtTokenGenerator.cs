using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Application.Interfaces.Security
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(int userId, string email, int role);
    }
}
