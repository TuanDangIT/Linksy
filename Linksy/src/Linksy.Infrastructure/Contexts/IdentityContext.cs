using Linksy.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Contexts
{
    public class IdentityContext : IIdentityContext
    {
        public bool IsAuthenticated { get; }
        public int Id { get; }
        public string Username { get; }
        public string Role { get; }
        public IdentityContext(ClaimsPrincipal principal)
        {
            if (principal.Identity?.IsAuthenticated is false)
            {
                IsAuthenticated = false;
            }
            else
            {
                IsAuthenticated = true;
            }
            Id = IsAuthenticated ? int.Parse(principal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value) : default;
            Role = IsAuthenticated ? principal.Claims.Single(x => x.Type == ClaimTypes.Role).Value : string.Empty;
            Username = IsAuthenticated ? principal.Claims.Single(x => x.Type == ClaimTypes.Name).Value : string.Empty;
        }
    }
}
