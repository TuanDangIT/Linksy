using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Linksy.Domain.Entities.User
{
    public class Role : IdentityRole<int>
    {
        public Role(int id, string roleName, string normalizedRoleName)
        {
            Id = id;
            Name = roleName;
            NormalizedName = normalizedRoleName;
        }
        private Role()
        {

        }
    }
}
