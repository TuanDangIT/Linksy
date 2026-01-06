using Linksy.Application.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Barcodes.Features.CreateBarcode
{
    public record class CreateBarcodeResponse(int BarcodeId, int UrlId, ImageDto Image); 
}
