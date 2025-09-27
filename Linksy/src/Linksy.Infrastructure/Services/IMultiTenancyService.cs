using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Services
{
    public interface IMultiTenancyService
    {
        int CurrentTenantId { get; }
        void SetCurrentTenant(int tenantId);
    }
}
