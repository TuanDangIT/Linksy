using Linksy.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.BlobStorage.Exceptions
{
    internal class BlobStorageFileNullOrEmptyException : LinksyException
    {
        public BlobStorageFileNullOrEmptyException() : base("File cannot be null nor empty.")
        {
        }
    }
}
