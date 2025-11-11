using Linksy.Domain.Entities.Tracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Entities.ScanCode
{
    public class Barcode : ScanCode
    {
        private readonly List<BarcodeEngagement> _engagements = [];
        public IEnumerable<BarcodeEngagement> Engagements => _engagements;
        private Barcode(Url.Url url, string imageUrl, IEnumerable<string>? tags, int userId) : base(url, imageUrl, tags, userId)
        {
        }
        private Barcode() { }
        public static Barcode CreateBarcode(Url.Url url, string imageUrl, IEnumerable<string>? tags, int userId)
            => new(url, imageUrl, tags, userId);
        public void AddEngagement(BarcodeEngagement engagement)
            => _engagements.Add(engagement);
    }
}
