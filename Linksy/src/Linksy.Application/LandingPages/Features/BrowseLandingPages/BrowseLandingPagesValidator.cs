using FluentValidation;
using Linksy.Application.Shared.Pagination;
using Linksy.Domain.Entities.LandingPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPages.Features.BrowseLandingPages
{
    internal class BrowseLandingPagesValidator : PaginationValidator<BrowseLandingPages, LandingPage>
    {
        public BrowseLandingPagesValidator(IPaginationConfiguration<LandingPage> paginationConfig) : base(paginationConfig)
        {
        }
    }
}
