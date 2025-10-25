using Linksy.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Barcodes.Exceptions
{
    public class BarcodeNotFoundException : LinksyException
    {
        public BarcodeNotFoundException(int barcodeId) : base($"Barocode with ID: {barcodeId} was not found.")
        {
        }
    }
}
