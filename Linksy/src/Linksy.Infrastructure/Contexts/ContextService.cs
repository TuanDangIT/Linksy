using Linksy.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Contexts
{
    internal class ContextService : IContextService
    {
        public string RequestId => $"{Guid.NewGuid()}";
        public string TraceId { get; } = string.Empty;
        public IIdentityContext? Identity { get; }
        public ContextService()
        {

        }
        public ContextService(HttpContext context) 
        {
            TraceId = context.TraceIdentifier;
            Identity = new IdentityContext(context.User);
        }
    }
}
