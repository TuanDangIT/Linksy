using Linksy.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Entities.Tracking
{
    public class UrlEngagement : Engagement
    {
        public Url.Url Url { get; set; } = default!;
        public int UrlId { get; set; }
        private UrlEngagement(Url.Url url, string? ipAddress) : base(ipAddress)
        {
            Url = url;
        }
        private UrlEngagement()
        {
            
        }
        public static UrlEngagement CreateEngagement(Url.Url url, string? ipAddress)
            => new(url, ipAddress);
    }
}
