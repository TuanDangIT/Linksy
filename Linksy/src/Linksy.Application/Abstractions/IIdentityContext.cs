using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Abstractions
{
    public interface IIdentityContext
    {
        bool IsAuthenticated { get; }
        public int Id { get; }
        string Role { get; }
        string Username { get; }
    }
}
