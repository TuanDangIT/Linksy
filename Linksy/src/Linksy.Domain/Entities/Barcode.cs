using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Entities
{
    public class Barcode : ScanCode
    {
        private Barcode(Url url, string imageUrl, IEnumerable<string> tags, int userId) : base(url, imageUrl, tags, userId)
        {
        }
        private Barcode() { }
        public static Barcode CreateBarcode(Url url, string imageUrl, IEnumerable<string> tags, int userId)
            => new(url, imageUrl, tags, userId);
    }
}
