using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Services
{
    internal class MultiTenancyService : IMultiTenancyService
    {
        public int CurrentTenantId { get; set; }
        public void SetCurrentTenant(int tenantId)
            => CurrentTenantId = tenantId;
    }
}
