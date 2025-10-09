using Linksy.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.Features.DeleteUrl
{
    public record class DeleteUrl(int Id) : ICommand;
}
