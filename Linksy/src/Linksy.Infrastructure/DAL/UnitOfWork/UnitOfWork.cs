using Linksy.Application.Shared.UnitOfWork;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.DAL.UnitOfWork
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly LinksyDbContext _dbContext;

        public UnitOfWork(LinksyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IDbTransaction BeginTransaction()
            => _dbContext.Database.BeginTransaction().GetDbTransaction();
    }
}
