using Linksy.Application.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.UmtParameters.Features.GetUmtParameter
{
    public record class GetUmtParameterQrCodeDto
    {
        public int Id { get; init; }
        public int ScanCount { get; init; }
        public ImageDto Image { get; init; } = default!;
        public GetUmtParameterQrCodeDto(int id, int scanCount, ImageDto image)
        {
            Id = id;
            ScanCount = scanCount;
            Image = image;
        }
    }
}
