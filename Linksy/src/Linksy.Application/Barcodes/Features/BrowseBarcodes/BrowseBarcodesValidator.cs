using FluentValidation;
using Linksy.Application.Shared.Pagination;
using Linksy.Domain.Entities.ScanCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Barcodes.Features.BrowseBarcodes
{
    internal class BrowseBarcodesValidator : PaginationValidator<BrowseBarcodes, Barcode>
    {
        public BrowseBarcodesValidator(IPaginationConfiguration<Barcode> paginationConfiguration) : base(paginationConfiguration)
        {
        }
    }
}
