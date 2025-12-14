using Linksy.Application.Shared.DTO;
using Linksy.Domain.Entities.ScanCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Barcodes.Features.GetBarcode
{
    public record class GetBarcodeResponse : ScanCodeDto
    {
        public GetBarcodeUrlDto Url { get; init; }
        public GetBarcodeResponse(int id, int scanCount, ImageDto image, IEnumerable<string>? tags, DateTime createdAt, DateTime? updatedAt, GetBarcodeUrlDto url) : 
            base(id, scanCount, image, tags, createdAt, updatedAt)
        {
            Url = url;
        }
    }
}
