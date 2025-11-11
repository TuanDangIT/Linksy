using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Shared.BlobStorage
{
    public interface IBlobStorageService
    {
        Task<string> UploadAsync(IFormFile blob, string fileName, string containerName, CancellationToken cancellationToken = default);
        Task<BlobStorageDto> DownloadAsync(string fileName, string containerName, CancellationToken cancellationToken = default);
        Task DeleteAsync(string fileName, string containerName, CancellationToken cancellationToken = default);
    }
}
