﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Interfaces
{
    public interface IRoleService
    {
        Task<string?> GetRoleNameByIdAsync(int roleId);

    }
}
