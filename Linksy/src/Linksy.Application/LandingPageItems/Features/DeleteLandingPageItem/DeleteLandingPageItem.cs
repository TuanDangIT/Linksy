using Linksy.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPageItems.Features.DeleteLandingPageItem
{
    public record class DeleteLandingPageItem(int LandingPageItemId) : ICommand;
}
