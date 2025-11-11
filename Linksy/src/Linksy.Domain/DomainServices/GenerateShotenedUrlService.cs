using Linksy.Domain.Entities.Url;
using Linksy.Domain.Exceptions;
using Linksy.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Linksy.Domain.DomainServices
{
    internal class GenerateShotenedUrlService : IGenerateShotenedUrlService
    {
        private const string _characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const int _shotenedUrlLength = 6;
        private readonly TimeProvider _timeProvider;
        private readonly IUrlRepository _urlRepository;

        public GenerateShotenedUrlService(TimeProvider timeProvider, IUrlRepository urlRepository)
        {
            _timeProvider = timeProvider;
            _urlRepository = urlRepository;
        }
        public async Task<Url> GenerateShortenedUrl(string originalUrl, string? customCode, IEnumerable<string> tags, IEnumerable<UmtParameter>? umtParameters, int userId, CancellationToken cancellationToken = default)
        {
            Url url;
            if (customCode is not null)
            {
                var isCustomCodeTaken = await _urlRepository.IsUrlCodeInUseAsync(customCode, cancellationToken);
                if (isCustomCodeTaken)
                {
                    throw new CustomCodeInUseException(customCode);
                }
                url = GenerateShortenedUrlWithCustomCode(originalUrl, customCode, tags, umtParameters, userId);
            }
            else
            {
                url = GenerateShortenedUrl(originalUrl, tags, umtParameters, userId);
            }
            return url;
        }
        private Url GenerateShortenedUrl(string originalUrl, IEnumerable<string> tags, IEnumerable<UmtParameter>? umtParameters, int userId)
            => Url.CreateShortenedUrl(originalUrl, GenerateCode(originalUrl), tags, umtParameters, userId);
        private Url GenerateShortenedUrlWithCustomCode(string originalUrl, string customCode, IEnumerable<string> tags, IEnumerable<UmtParameter>? umtParameters, int userId)
            => Url.CreateShortenedUrl(originalUrl, customCode, tags, umtParameters, userId);
        private string GenerateCode(string url)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(url + _timeProvider.GetUtcNow().Ticks));
                var result = new StringBuilder(_shotenedUrlLength);

                for (int i = 0; i < _shotenedUrlLength; i++)
                {
                    result.Append(_characters[hashBytes[i] % _characters.Length]);
                }

                return result.ToString();
            }
        }
    }
}
