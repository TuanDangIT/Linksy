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
        public Url.Url Url { get; private set; } = default!;
        public int UrlId { get; private set; }
        private Barcode(Url.Url url, string imageUrl, IEnumerable<string>? tags, int userId) : base(imageUrl, tags, userId)
        {
            Url = url;
        }
        private Barcode() { }
        public static Barcode CreateBarcode(Url.Url url, string imageUrl, IEnumerable<string>? tags, int userId)
            => new(url, imageUrl, tags, userId);
        public void AddEngagement(BarcodeEngagement engagement)
            => _engagements.Add(engagement);
    }
}
