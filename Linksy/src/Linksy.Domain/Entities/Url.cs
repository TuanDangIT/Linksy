using Linksy.Domain.Abstractions;
using Linksy.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Entities
{
    public class Url : BaseEntityWithMultitenancy, IAuditable
    {
        public bool IsActive { get; private set; } = true;
        public string OriginalUrl { get; private set; } = string.Empty;
        public string Code { get; private set; } = string.Empty;
        public int VisitCount { get; private set; } = 0;
        public QrCode? QrCode { get; set; }
        public Barcode? Barcode { get; set; }
        public LandingPage? LandingPage { get; set; }
        public LandingPageItem? LandingPageItem { get; set; }
        private readonly List<UmtParameter>? _umtParameters = [];
        public IEnumerable<UmtParameter>? UmtParameters => _umtParameters;
        private readonly List<Engagement>? _engagements = [];
        public IEnumerable<Engagement>? Engagements => _engagements;
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        private Url(string originalUrl, string code, IEnumerable<UmtParameter>? umtParameters, int userId) /*: this(code, userId)*/ : base(userId)
        {
            Code = code;
            OriginalUrl = originalUrl;
            _umtParameters = umtParameters?.ToList();
        }
        //private Url(string code, int userId) : base(userId)
        //{
        //    Code = code;
        //}
        private Url() { }   
        public static Url CreateShortenedUrl(string originalUrl, string code, IEnumerable<UmtParameter>? umtParameters, int userId)
            => new(originalUrl, code, umtParameters, userId);
        //public static Url CreateLandingPageUrl(string code, int userId)
        //    => new(code, userId); 
        //public static Url CreateLandingPageItemUrl(string code, int userId)
        //    => new(code, userId);
        public void IncrementVisitsCounter()
            => VisitCount++;
        public void ChangeOrginalUrl(string newOriginalUrl)
            => OriginalUrl = newOriginalUrl;
        public void SetActive(bool isActive)
            => IsActive = isActive; 
        public void AddQrCode(QrCode qrCode)
            => QrCode = qrCode; 
        public void AddBarcode(Barcode barcode)
            => Barcode = barcode;
    }
}
