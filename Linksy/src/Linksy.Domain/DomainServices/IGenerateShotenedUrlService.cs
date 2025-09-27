using Linksy.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.DomainServices
{
    public interface IGenerateShotenedUrlService
    {
        Url GenerateShortenedUrl(string originalUrl);
    }
}
