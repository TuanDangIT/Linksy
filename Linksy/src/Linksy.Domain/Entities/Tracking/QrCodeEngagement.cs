using Linksy.Domain.Entities.ScanCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Entities.Tracking
{
    public class QrCodeEngagement : Engagement
    {
        public QrCode QrCode { get; set; } = default!;
        public int QrCodeId { get; set; }
        public QrCodeEngagement(QrCode qrCode, string? ipAddress) : base(ipAddress)
        {
            QrCode = qrCode;
        }
        private QrCodeEngagement()
        {
            
        }
        public static QrCodeEngagement CreateQrCodeEngagement(QrCode qrCode, string? ipAddress)
            => new(qrCode, ipAddress);
    }
}
