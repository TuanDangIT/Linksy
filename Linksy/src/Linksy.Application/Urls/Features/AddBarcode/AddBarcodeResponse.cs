using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.Features.AddBarcode
{
    public record class AddBarcodeResponse(int QrCodeId, string ImageUrlPath, string FileName);
}
