using Linksy.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Entities.Tracking
{
    public abstract class Engagement : BaseEntity, IHasEngagementTime
    {
        public string? IpAddress { get; private set; } 
        public DateTime EngagedAt { get; private set; }
        protected Engagement(string? ipAddress)
        {
            IpAddress = ipAddress;
        }
        protected Engagement()
        {
            
        }
    }
}
