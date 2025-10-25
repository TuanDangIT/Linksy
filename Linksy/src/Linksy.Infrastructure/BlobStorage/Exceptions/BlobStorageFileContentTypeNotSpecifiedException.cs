using Linksy.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.BlobStorage.Exceptions
{
    internal class BlobStorageFileContentTypeNotSpecifiedException : LinksyException
    {
        public BlobStorageFileContentTypeNotSpecifiedException() : base("File content type is not specified.")
        {
        }
    }
}
