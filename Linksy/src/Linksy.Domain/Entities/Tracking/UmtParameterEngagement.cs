using Linksy.Domain.Abstractions;
using Linksy.Domain.Entities.Url;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Entities.Tracking
{
    public class UmtParameterEngagement : Engagement
    {
        public UmtParameter UmtParameter { get; private set; } = default!;
        public int UmtParameterId { get; private set; }
        private UmtParameterEngagement(UmtParameter umtParameter, string? ipAddress) : base(ipAddress)
        {
            UmtParameter = umtParameter;
        }
        private UmtParameterEngagement()
        {
            
        }
        public static UmtParameterEngagement CreateUmtEngagementParameter(UmtParameter umtParameter, string? ipAddress)
            => new(umtParameter, ipAddress);
    }
}
