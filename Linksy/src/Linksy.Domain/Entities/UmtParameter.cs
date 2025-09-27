using Linksy.Domain.Abstractions;
using Linksy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Entities
{
    public class UmtParameter : BaseEntity, IAuditable
    {
        public UmtType Type { get; private set; }
        public int VisitCount { get; private set; } = 0;
        public QrCode QrCode { get; private set; } = default!;
        public int QrCodeId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public UmtParameter(UmtType umtType, QrCode qrCode)
        {
            Type = umtType;
            QrCode = qrCode;
        }
        private UmtParameter() { }
        public void IncrementVisits()
            => VisitCount++;
    }
}
