using Linksy.Domain.Abstractions;
using Linksy.Domain.Entities.ScanCode;
using Linksy.Domain.Entities.Tracking;
using Linksy.Domain.Enums;
using Linksy.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Entities.Url
{
    public class UmtParameter : BaseEntity, IAuditable
    {
        public string? UmtSource { get; private set; } 
        public string? UmtMedium { get; private set; }
        public string? UmtCampaign { get; private set; }
        public int VisitCount { get; private set; } = 0;
        public Url Url { get; private set; } = default!;
        public int UrlId { get; private set; }
        public QrCode? QrCode { get; private set; }
        private readonly List<UmtParameterEngagement> _engagements = [];
        public IReadOnlyCollection<UmtParameterEngagement> Engagements => _engagements.AsReadOnly();
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        private UmtParameter(string? umtSource, string? umtMedium, string? umtCampaign)
        {
            UmtSource = umtSource;
            UmtMedium = umtMedium;
            UmtCampaign = umtCampaign;
        }
        private UmtParameter() { }
        public static UmtParameter CreateUmtParameter(string? umtSource, string? umtMedium, string? umtCampaign)
            => new(umtSource, umtMedium, umtCampaign);
        public void AddQrCode(QrCode qrCode)
        {
            if (QrCode is not null)
                throw new UmtParameterAlreadyHasQrCodeException(Id);
            QrCode = qrCode;
        }   
        public void IncrementVisits()
            => VisitCount++;
        public void UpdateSource(string source)
            => UmtSource = source;
        public void UpdateMedium(string medium)
            => UmtMedium = medium;
        public void UpdateCampaign(string campaign)
            => UmtCampaign = campaign;
        public void AddEngagement(UmtParameterEngagement engagement)
            => _engagements.Add(engagement);
    }
}
