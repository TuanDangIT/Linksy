using Linksy.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Entities
{
    public class UmtParameterEngagement : BaseEntity, IHasEngagementTime
    {
        public UmtParameter UmtParameter { get; private set; } = default!;
        public int UmtParameterId { get; private set; }
        public DateTime EngagedAt { get; private set; }
        private UmtParameterEngagement(UmtParameter umtParameter)
        {
            UmtParameter = umtParameter;
        }
        private UmtParameterEngagement()
        {
            
        }
        public static UmtParameterEngagement CreateUmtEngagementParameter(UmtParameter umtParameter)
            => new UmtParameterEngagement(umtParameter);
    }
}
