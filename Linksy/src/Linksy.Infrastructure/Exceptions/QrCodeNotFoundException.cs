using Linksy.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Exceptions
{
    internal class QrCodeNotFoundException : LinksyException
    {
        public QrCodeNotFoundException(int qrCodeId) : base($"QR Code with ID: {qrCodeId} was not found.")
        {
        }
    }
}
