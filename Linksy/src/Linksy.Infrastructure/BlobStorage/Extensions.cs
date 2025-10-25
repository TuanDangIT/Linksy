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
        public static IServiceCollection AddBloblStorage(this IServiceCollection services, IConfiguration configuration)
        {
            var blobStorageOptions = services.GetOptions<BlobStorageOptions>(_blobStorageSectionName);
            services.AddSingleton(blobStorageOptions);
            var options = services.GetOptions<BlobStorageOptions>(_blobStorageSectionName);
            services.AddAzureClients(builder =>
            {
                builder.AddBlobServiceClient(options.ConnectionString).WithName(blobStorageOptions.ClientName);
            });
            services.AddSingleton<IBlobStorageService, BlobStorageService>();
            return services;
        }
    }
}
