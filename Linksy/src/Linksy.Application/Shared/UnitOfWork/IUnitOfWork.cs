using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Shared.UnitOfWork
{
    public interface IUnitOfWork
    {
        IDbTransaction BeginTransaction();
    }
}
