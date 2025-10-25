using Linksy.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Entities
{
    public abstract class ScanCode : BaseEntityWithMultitenancy, IAuditable
    {
        public Url Url { get; private set; } = default!;
        public int UrlId { get; private set; }
        public string ImageUrlPath { get; private set; } = string.Empty;
        public int ScanCount { get; private set; } = 0;
        public List<string>? Tags { get; private set; } = [];
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        protected ScanCode(Url url, string imageUrlPath, IEnumerable<string>? tags, int userId) : base(userId)
        {
            Url = url;
            ImageUrlPath = imageUrlPath;
            Tags = tags?.ToList();
        }
        protected ScanCode() { } 
        public void IncrementScanCounter()
            => ScanCount++;
        public void SetImageUrlPath(string imageUrlPath)
            => ImageUrlPath = imageUrlPath;
    }
}
