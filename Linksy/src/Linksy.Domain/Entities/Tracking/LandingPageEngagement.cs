using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linksy.Domain.Entities.LandingPage;

namespace Linksy.Domain.Entities.Tracking
{
    public class LandingPageEngagement : Engagement
    {
        public LandingPage.LandingPage LandingPage { get; private set; } = default!;
        public int LandingPageId { get; private set; }
        private LandingPageEngagement(LandingPage.LandingPage landingPage, string? ipAddress) : base(ipAddress)
        {
            LandingPage = landingPage;
        }
        private LandingPageEngagement()
        {
        }
        public static LandingPageEngagement CreateLandingPageEngagement(LandingPage.LandingPage landingPage, string? ipAddress)
            => new(landingPage, ipAddress);
    }
}
