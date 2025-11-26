using Linksy.Application.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPageItems.Features.AddImageLandingPageItem
{
    public record class AddImageLandingPageItemResponse(int LandingPageId, string ImageUrlPath, string FileName) : AddLandingPageResponse(LandingPageId);
}
