using Linksy.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Entities
{
    public class QrCode : ScanCode
    {
        public bool IsUmtOn { get; set; }
        private readonly List<UmtParameter> _umtParameters = [];
        public IEnumerable<UmtParameter> UmtParameters => _umtParameters;
        public QrCodePage? QrCodePage { get; set; }
        public QrCode(Url url, string imageUrl, List<string> tags, bool isUmtOn, List<UmtParameter> umtParameters) : base(url, imageUrl, tags)
        {
            IsUmtOn = isUmtOn;
            _umtParameters = umtParameters;
        }
        private QrCode() { }
    }
}
