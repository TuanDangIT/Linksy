using Linksy.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Entities.Tracking
{
    public class LandingPageView : BaseEntity
    {
        public string? IpAddress { get; private set; }
        public DateTime ViewedAt { get; private set; }
        public LandingPage.LandingPage LandingPage { get; private set; } = default!;
        public int LandingPageId { get; private set; }
        private LandingPageView(LandingPage.LandingPage landingPage, string? ipAddress, DateTime viewedAt)
        {
            LandingPage = landingPage;
            IpAddress = ipAddress;
            ViewedAt = viewedAt;
        }
        private LandingPageView()
        {
        }
        public static LandingPageView CreateLandingPageView(LandingPage.LandingPage landingPage, string? ipAddress, DateTime viewedAt)
            => new(landingPage, ipAddress, viewedAt);
    }
}
