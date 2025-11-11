using Linksy.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Entities.ScanCode
{
    public abstract class ScanCode : BaseEntityWithMultitenancy, IAuditable
    {
        public Url.Url Url { get; private set; } = default!;
        public int UrlId { get; private set; }
        public bool IsActive { get; private set; } = true;
        public string ImageUrlPath { get; private set; } = string.Empty;
        public int ScanCount { get; private set; } = 0;
        public string? Tags { get; private set; } = string.Empty;
        public IEnumerable<string>? TagsList
        {
            get => Tags?.Split(',') ?? [];
            set => Tags = value != null ? string.Join(',', value) : null;
        } 
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        protected ScanCode(Url.Url url, string imageUrlPath, IEnumerable<string>? tags, int userId) : base(userId)
        {
            Url = url;
            ImageUrlPath = imageUrlPath;
            TagsList = tags?.ToList();
        }
        protected ScanCode() { }
        public void IncrementScanCounter()
            => ScanCount++;
        public void SetImageUrlPath(string imageUrlPath)
            => ImageUrlPath = imageUrlPath;
        public void UpdateTags(IEnumerable<string> tags)
            => TagsList = [.. tags];
        public void SetActive(bool isActive)
            => IsActive = isActive;
    }
}
