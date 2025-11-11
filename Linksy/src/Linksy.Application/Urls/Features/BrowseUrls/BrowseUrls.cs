using Linksy.Application.Abstractions;
using Linksy.Application.Shared.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.Features.BrowseUrls
{
    public record class BrowseUrls : IQuery<BrowseUrlsResponse>
    {
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
        public IEnumerable<string>? Orders { get; init; }
        [ModelBinder(BinderType = typeof(DictionaryModelBinder))]
        public Dictionary<string, string>? Filters { get; set; }
    }
}
