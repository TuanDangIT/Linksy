using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Exceptions
{
    internal class UmtParameterAlreadyHasQrCodeException : LinksyException
    {
        public UmtParameterAlreadyHasQrCodeException(int umtParameterId) : base($"The UTM parameter with ID {umtParameterId} already has an associated QR code.")
        {
        }
    }
}
