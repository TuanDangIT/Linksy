using Linksy.Domain.Entities.ScanCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Shared.ScanCodes
{
    public interface IScanCodeService
    {
        Task<(string QrCodeUrlPath, string FileName)> GenerateQrCodeAsync(QrCode qrCode, string url, CancellationToken cancellationToken = default);
        Task<(string BarcodeUrlPath, string FileName)> GenerateBarcodeAsync(Barcode barcode, string url, CancellationToken cancellationToken = default);
        Task DeleteAsync(string fileName, string containerName, CancellationToken cancellationToken = default);
        string GetScanCodeFileName(int scanCodeId, string scanCodeType);
    }
}
