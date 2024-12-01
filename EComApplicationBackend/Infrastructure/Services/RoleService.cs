using App.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class RoleService : IRoleService
    {
        private readonly AppDbContext _context;
        public RoleService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<string?> GetRoleNameByIdAsync(string roleId)
        {
            var role = await _context.Roles
                .Where(r => r.id == int.Parse(roleId))
                .Select(r => r.roletype)
                .FirstOrDefaultAsync();

            return role;
        }
    }
}
