using Linksy.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.QrCodes.Features.DeleteQrCode
{
    public record class DeleteQrCode(int QrCodeId, bool IncludeUrlInDeletion) : ICommand;
}
