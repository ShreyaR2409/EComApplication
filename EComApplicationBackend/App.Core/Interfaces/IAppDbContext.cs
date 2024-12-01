﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<TEntity> Set<TEntity>()
           where TEntity : class;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        public IDbConnection GetConnection();
    }
}