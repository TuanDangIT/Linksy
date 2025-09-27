using Linksy.Domain.Abstractions;
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
        public ScanCode? ScanCode { get; set; }
        public LandingPageItem? LandingPageItem { get; set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public Url(string originalUrl, string code)
        {
            OriginalUrl = originalUrl;
            Code = code;    
        }
        private Url() { }   
        public void IncrementVisitsCounter()
            => VisitCount++;
        public void SetActive(bool isActive)
            => IsActive = isActive; 
    }
}
