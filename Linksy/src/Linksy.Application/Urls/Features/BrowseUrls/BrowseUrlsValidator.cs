using FluentValidation;
using Linksy.Application.Shared.Pagination;
using Linksy.Domain.Entities.Url;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.Features.BrowseUrls
{
    internal class BrowseUrlsValidator : PaginationValidator<BrowseUrls, Url>
    {
        public BrowseUrlsValidator(IPaginationConfiguration<Url> paginationConfig) : base(paginationConfig)
        {
        }
    }
}
