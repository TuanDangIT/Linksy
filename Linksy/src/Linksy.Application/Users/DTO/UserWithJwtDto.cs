using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Users.DTO
{
    public class UserWithJwtDto
    {
        public UserDto User { get; set; } = default!;
        public JwtDto Jwt { get; set; } = default!;
    }
}
