using Linksy.Application.Shared.BlobStorage;
using Linksy.Infrastructure.Auth;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.BlobStorage
{
    internal static class Extensions
    {
        private const string _blobStorageSectionName = "BlobAzureStorage";
        private const string _blobStorageConnectionStringName = "AzureBlobStorage";
        public static IServiceCollection AddBloblStorage(this IServiceCollection services, IConfiguration configuration)
        {
            var blobStorageOptions = services.GetOptions<BlobStorageOptions>(_blobStorageSectionName);
            services.AddSingleton(blobStorageOptions);
            services.AddAzureClients(builder =>
            {
                builder.AddBlobServiceClient(configuration.GetConnectionString(_blobStorageConnectionStringName)).WithName(blobStorageOptions.ClientName);
            });
            services.AddSingleton<IBlobStorageService, BlobStorageService>();
            return services;
        }
    }
}
