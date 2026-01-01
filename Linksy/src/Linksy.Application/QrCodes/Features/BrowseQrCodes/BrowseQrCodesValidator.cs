using FluentValidation;
using Linksy.Application.Shared.Pagination;
using Linksy.Domain.Entities.ScanCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.QrCodes.Features.BrowseQrCodes
{
    internal class BrowseQrCodesValidator : PaginationValidator<BrowseQrCodes, QrCode>
    {
        public BrowseQrCodesValidator(IPaginationConfiguration<QrCode> paginationConfig) : base(paginationConfig)
        {
        }
    }
}
