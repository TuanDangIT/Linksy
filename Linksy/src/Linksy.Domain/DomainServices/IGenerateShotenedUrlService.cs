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
        Task<Url> GenerateShortenedUrl(string originalUrl, string? customCode, IEnumerable<UmtParameter>? umtParameters, int userId, CancellationToken cancellationToken = default);
        //Url GenerateShortenedUrl(string originalUrl, IEnumerable<UmtParameter>? umtParameters, int userId);
        //Url GenerateShortenedUrlWithCustomCode(string originalUrl, string customCode, IEnumerable<UmtParameter>? umtParameters, int userId);
    }
}
