using Linksy.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Entities
{
    public class QrCodePage : BaseEntity, IAuditable
    {
        public string BackgroundColor { get; private set; } = string.Empty;
        public int VisitCount { get; private set; } = 0;
        public string Code { get; private set; } = string.Empty;
        public QrCode QrCode { get; private set; } = default!;
        public int QrCodeId { get; private set; }   
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public QrCodePage(string backgroundColor, string code, QrCode qrCode)
        {
            BackgroundColor = backgroundColor;
            Code = code;
            QrCode = qrCode;    
        }
        private QrCodePage() { }
        public void IncrementVisits()
            => VisitCount++;
    }
}
