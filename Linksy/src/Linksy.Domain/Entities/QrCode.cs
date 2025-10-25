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
        private readonly IEnumerable<LandingPageItem> _landingPageItems = [];
        public IEnumerable<LandingPageItem> LandingPageItems => _landingPageItems;
        //private readonly List<UmtParameter>? UmtParameters = [];
        //public IEnumerable<UmtParameter>? UmtParametersList => UmtParameters;    
        private QrCode(Url url, string imageUrl, IEnumerable<string>? tags, int userId) : base(url, imageUrl, tags, userId)
        {
        }
        private QrCode() { }
        public static QrCode CreateQrCode(Url url, string imageUrl, IEnumerable<string>? tags, int userId)
            => new(url, imageUrl, tags, userId);
    }
}
