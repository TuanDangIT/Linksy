using Linksy.Domain.Entities.ScanCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Entities.Tracking
{
    public class BarcodeEngagement : Engagement
    {
        public Barcode Barcode { get; private set; } = default!;
        public int BarcodeId { get; private set; }
        private BarcodeEngagement(Barcode barcode, string? ipAddress) : base(ipAddress)
        {
            Barcode = barcode;
        }
        private BarcodeEngagement()
        {
        }
        public static BarcodeEngagement CreateBarcodeEngagement(Barcode barcode, string? ipAddress)
            => new(barcode, ipAddress);
    }
}
