using Linksy.Domain.Abstractions;
using Linksy.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Entities.ScanCode
{
    public abstract class ScanCode : BaseEntityWithMultitenancy, IAuditable
    {
        public Image ScanCodeImage { get; private set; } = default!;
        public int ScanCount { get; private set; } = 0;
        public string? Tags { get; private set; } = string.Empty;
        public IEnumerable<string>? TagsList
        {
            get => Tags?.Split(',') ?? [];
            set => Tags = value != null ? string.Join(',', value) : null;
        } 
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        protected ScanCode(Image scanCodeImage, IEnumerable<string>? tags, int userId) : base(userId)
        {
            ScanCodeImage = scanCodeImage;
            TagsList = tags?.ToList();
        }
        protected ScanCode() { }
        public void IncrementScanCounter()
            => ScanCount++;
        public void UpdateTags(IEnumerable<string> tags)
            => TagsList = [.. tags];
    }
}
