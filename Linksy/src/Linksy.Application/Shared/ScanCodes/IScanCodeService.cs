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
        Task<(string QrCodeUrlPath, string FileName)> GenerateQrCodeAsync(string code, int userId, CancellationToken cancellationToken = default);
        Task<(string QrCodeUrlPath, string FileName)> GenerateQrCodeAsync(string code, string? umtSource, string? umtMedium, string? umtCampaign, int userId, CancellationToken cancellationToken = default);
        Task<(string BarcodeUrlPath, string FileName)> GenerateBarcodeAsync(string code, int userId, CancellationToken cancellationToken = default);
        Task DeleteAsync(string fileName, int userId, CancellationToken cancellationToken = default);
    }
}
