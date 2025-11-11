using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Linksy.Application.Shared.BlobStorage;
using Linksy.Infrastructure.BlobStorage.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.BlobStorage
{
    internal class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IAzureClientFactory<BlobServiceClient> _factory;
        private readonly BlobStorageOptions _blobStorageOptions;
        private readonly ILogger<BlobStorageService> _logger;
        private readonly int _maxFileSizeInBytes = 10 * 1024 * 1024; 

        public BlobStorageService(IAzureClientFactory<BlobServiceClient> factory, BlobStorageOptions blobStorageOptions, ILogger<BlobStorageService> logger)
        {
            _factory = factory;
            _blobStorageOptions = blobStorageOptions;
            _blobServiceClient = _factory.CreateClient(_blobStorageOptions.ClientName);
            _maxFileSizeInBytes = blobStorageOptions.MaxFileSizeInBytes;
            _logger = logger;
        }
        public async Task<BlobStorageDto> DownloadAsync(string fileName, string containerName, CancellationToken cancellationToken = default)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var file = containerClient.GetBlobClient(fileName);
            if (!(await file.ExistsAsync(cancellationToken)))
            {
                throw new RequestFailedException($"Request failed. File: {fileName} was not found.");
            }
            var stream = await file.OpenReadAsync(cancellationToken: cancellationToken);
            var content = await file.DownloadContentAsync(cancellationToken: cancellationToken);
            var contentType = content.Value.Details.ContentType;
            _logger.LogDebug("File: {fileName} downloaded from blob storage container: {containerName}.", fileName, containerName); 
            return new BlobStorageDto()
            {
                FileStream = stream,
                ContentType = contentType,
                FileName = fileName
            };
        }

        public async Task<string> UploadAsync(IFormFile blob, string fileName, string containerName, CancellationToken cancellationToken = default)
        {
            if (blob is null || blob.Length == 0)
            {
                throw new BlobStorageFileNullOrEmptyException();
            }
            if (blob.Length > _maxFileSizeInBytes)
            {
                throw new BlobStorageFileExceedMaxSizeException();
            }
            if (string.IsNullOrEmpty(blob.ContentType))
            {
                throw new BlobStorageFileContentTypeNotSpecifiedException();
            }
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync(publicAccessType: PublicAccessType.Blob, cancellationToken: cancellationToken);
            var blobClient = containerClient.GetBlobClient(fileName);
            await using (Stream data = blob.OpenReadStream())
            {
                var response = await blobClient.UploadAsync(data, new BlobHttpHeaders()
                {
                    ContentType = blob.ContentType
                }, cancellationToken: cancellationToken);
                using var rawResponse = response.GetRawResponse();
                if (rawResponse.IsError)
                {
                    throw new RequestFailedException($"Request failed. File: {fileName} was not uploaded to blob storage.");
                }
            }
            _logger.LogDebug("File: {fileName} uploaded to blob storage container: {containerName}.", fileName, containerName); 
            return blobClient.Uri.AbsolutePath;
        }

        public async Task DeleteAsync(string fileName, string containerName, CancellationToken cancellationToken = default)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            using var response = await blobClient.DeleteAsync(cancellationToken: cancellationToken);
            if (response.IsError)
            {
                throw new RequestFailedException($"Request failed. File: {fileName} was not deleted from blob storage.");
            }
        }
    }
}
