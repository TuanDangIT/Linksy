using Linksy.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.BlobStorage.Exceptions
{
    internal class BlobStorageFileExceedMaxSizeException : LinksyException
    {
        public BlobStorageFileExceedMaxSizeException() : base($"File size must be below 10MB.")
        {
        }
    }
}
