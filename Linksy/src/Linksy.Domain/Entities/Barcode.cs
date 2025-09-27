using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Entities
{
    public class Barcode : ScanCode
    {
        public Barcode(Url url, string imageUrl, List<string> tags) : base(url, imageUrl, tags)
        {
        }
        private Barcode() { }
    }
}
