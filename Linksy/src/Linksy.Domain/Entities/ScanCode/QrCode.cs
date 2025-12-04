using Linksy.Domain.Abstractions;
using Linksy.Domain.Entities.LandingPage;
using Linksy.Domain.Entities.Tracking;
using Linksy.Domain.Entities.Url;
using Linksy.Domain.ValueObjects;
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
        public Url.Url? Url { get; private set; } = default!;
        public int? UrlId { get; private set; }
        private QrCode(Image qrCodeImage, IEnumerable<string>? tags, int userId) : base(qrCodeImage, tags, userId) 
        {
        }
        private QrCode(Url.Url url, Image qrCodeImage, IEnumerable<string>? tags, int userId) : this(qrCodeImage, tags, userId)
        {
            Url = url;
        }
        private QrCode() { }
        public static QrCode CreateQrCode(Url.Url url, Image qrCodeImage, IEnumerable<string>? tags, int userId)
            => new(url, qrCodeImage, tags, userId);
        public static QrCode CreateQrCode(Image qrCodeImage, IEnumerable<string>? tags, int userId)
            => new(qrCodeImage, tags, userId);
        public void AddEngagement(QrCodeEngagement engagement)
        {
            IncrementScanCounter();
            _engagements.Add(engagement);
        }
    }
}
