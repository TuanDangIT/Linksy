using Linksy.Application.Abstractions;
using Linksy.Application.Shared.ModelBinders;
using Linksy.Application.Shared.Pagination;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Barcodes.Features.BrowseBarcodes
{
    public record class BrowseBarcodes : IQuery<BrowseBarcodesResponse>
    {
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
        public IEnumerable<string>? Orders { get; init; }
        [ModelBinder(BinderType = typeof(DictionaryModelBinder))]
        public Dictionary<string, string>? Filters { get; set; }
    }
}
