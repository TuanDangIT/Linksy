using Linksy.Domain.Abstractions;
using Linksy.Domain.Entities.LandingPage;
using Linksy.Domain.Entities.ScanCode;
using Linksy.Domain.Entities.Tracking;
using Linksy.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Entities.Url
{
    //Shortened URL entity  
    public class Url : BaseEntityWithMultitenancy, IAuditable, IStatisticalUpdate
    {
        public bool IsActive { get; private set; } = true;
        public string OriginalUrl { get; private set; } = string.Empty;
        public string Code { get; private set; } = string.Empty;
        public int VisitCount { get; private set; } = 0;
        public string? Tags { get; private set; } = string.Empty;
        public IEnumerable<string>? TagsList
        {
            get => Tags?.Split(',') ?? [];
            set => Tags = value != null ? string.Join(',', value) : null;
        }
        public QrCode? QrCode { get; set; }
        public Barcode? Barcode { get; set; }
        private readonly List<ImageLandingPageItem> _imageLandingPageItems = [];
        public IEnumerable<ImageLandingPageItem> ImageLandingPageItems => _imageLandingPageItems;
        private readonly List<UrlLandingPageItem> _urlLandingPageItems = [];
        public IEnumerable<UrlLandingPageItem> UrlLandingPageItems => _urlLandingPageItems;
        private readonly List<UmtParameter> _umtParameters = [];
        public IEnumerable<UmtParameter> UmtParameters => _umtParameters;
        private readonly List<UrlEngagement> _engagements = [];
        public IEnumerable<UrlEngagement> Engagements => _engagements;
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        private Url(string originalUrl, string code, IEnumerable<string>? tags, IEnumerable<UmtParameter>? umtParameters, int userId) : base(userId)
        {
            OriginalUrl = originalUrl;
            Code = code;
            //Tags = tags?.ToList();
            TagsList = tags?.ToList();
            if (umtParameters is not null)
            {
                _umtParameters = [.. umtParameters];
            }
        }
        private Url()
        {
            
        }
        public static Url CreateShortenedUrl(string originalUrl, string code, IEnumerable<string>? tags, IEnumerable<UmtParameter>? umtParameters, int userId)
            => new(originalUrl, code, tags, umtParameters, userId);
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
        public void AddEngagement(UrlEngagement engagement)
            => _engagements!.Add(engagement);   
        public void AddUmtParameter(UmtParameter umtParameter)
        {
            if(_umtParameters.Any(u => u.UmtSource?.ToLower() == umtParameter.UmtSource?.ToLower() &&
                u.UmtMedium?.ToLower() == umtParameter.UmtMedium?.ToLower() &&
                u.UmtCampaign?.ToLower() == umtParameter.UmtCampaign?.ToLower()))
            {
                throw new CannotHaveDuplicatedUmtParameterException();
            }
            _umtParameters.Add(umtParameter);
        }
        public void UpdateTags(IEnumerable<string> tags)
            => TagsList = [.. tags];
        public void AddImageLandingPageItem(ImageLandingPageItem item)
            => _imageLandingPageItems.Add(item);
        public void AddUrlLandingPageItem(UrlLandingPageItem item)
            => _urlLandingPageItems.Add(item);
    }
}
