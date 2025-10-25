using Linksy.Application.Abstractions;
using Linksy.Application.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.QrCodes.Features.CreateQrCode
{
    public record class CreateQrCode(CreateUrlDto Url, IEnumerable<string>? Tags) : ICommand<CreateQrCodeResponse>;
}
