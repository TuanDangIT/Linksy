using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.BlobStorage
{
    internal class BlobStorageOptions
    {
        public string ConnectionString { get; set; } = string.Empty;
        public int MaxFileSizeInBytes { get; set; }
        public string ClientName { get; set; } = string.Empty;  
    }
}
