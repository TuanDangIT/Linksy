using Linksy.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.Exceptions
{
    internal class UrlQrCodeAlreadyExistsException(int urlId) : LinksyException($"URL with ID {urlId} already has a QR Code.")
    {
    }
}
