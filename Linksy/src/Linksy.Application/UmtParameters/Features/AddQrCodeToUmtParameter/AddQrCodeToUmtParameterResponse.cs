using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.UmtParameters.Features.AddQrCodeToUmtParameter
{
    public record class AddQrCodeToUmtParameterResponse(int QrCodeId, string ImageUrlPath, string FileName);
}
