using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Users.DTO
{
    public class ConfirmEmailDto
    {
        public required string Email { get; set; }
        public required string Code { get; set; }
    }
}
