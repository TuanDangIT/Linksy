using Linksy.Domain.Abstractions;
using Linksy.Domain.Entities.LandingPage;
using Linksy.Domain.Entities.Tracking;
using Linksy.Domain.Entities.Url;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Entities.ScanCode
{
    public class QrCode : ScanCode
    {
        private readonly List<QrCodeEngagement> _engagements = [];
        public IEnumerable<QrCodeEngagement> Engagements => _engagements;
        public UmtParameter? UmtParameter { get; private set; } 
        public int? UmtParameterId { get; private set; }
        private QrCode(Url.Url url, string imageUrl, IEnumerable<string>? tags, int userId) : base(url, imageUrl, tags, userId)
        {
        }
        private QrCode() { }
        public static QrCode CreateQrCode(Url.Url url, string imageUrl, IEnumerable<string>? tags, int userId)
            => new(url, imageUrl, tags, userId);
        public void AddEngagement(QrCodeEngagement engagement)
            => _engagements.Add(engagement);
    }
}
