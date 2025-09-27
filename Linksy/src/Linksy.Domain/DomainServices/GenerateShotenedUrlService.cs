using Linksy.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.DomainServices
{
    internal class GenerateShotenedUrlService : IGenerateShotenedUrlService
    {
        private const string _characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const int _shotenedUrlLength = 6;
        private readonly TimeProvider _timeProvider;

        public GenerateShotenedUrlService(TimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }
        public Url GenerateShortenedUrl(string originalUrl)
        {
            var generatedCode = GenerateCode(originalUrl);  
            return new Url(originalUrl, generatedCode);
        }
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
