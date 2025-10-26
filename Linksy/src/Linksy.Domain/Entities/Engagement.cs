using Linksy.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Entities
{
    public class Engagement : BaseEntity, IHasEngagementTime
    {
        public Url Url { get; set; } = default!;
        public int UrlId { get; set; }
        public DateTime EngagedAt { get; private set; }
        private Engagement(Url url)
        {
            Url = url;
        }
        private Engagement()
        {
            
        }
        public static Engagement CreateEngagement(Url url)
            => new(url);
    }
}
