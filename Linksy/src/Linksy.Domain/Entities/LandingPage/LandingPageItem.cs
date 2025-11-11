using Linksy.Domain.Abstractions;
using Linksy.Domain.Entities.ScanCode;
using Linksy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Entities.LandingPage
{
    public abstract class LandingPageItem : BaseEntity, IAuditable
    {
        public LandingPageItemType Type { get; protected set; }
        public int Order { get; private set; }
        public int ClickCount { get; private set; } = 0;
        public LandingPage LandingPage { get; private set; } = default!;
        public int LandingPageId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        protected LandingPageItem(LandingPageItemType type, LandingPage landingPage, int order)
        {
            Type = type;
            LandingPage = landingPage;
            Order = order;
        }
        protected LandingPageItem() { }
        public void IncrementClickCount()
            => ClickCount++;
    }
}
