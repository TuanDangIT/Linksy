using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.Features.GetUrl
{
    public record class GetUrlLandingPageDto(int Id, string Title, DateTime CreatedAt, DateTime? UpdatedAt);
}
