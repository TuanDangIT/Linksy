using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Users.DTO
{
    public class JwtDto
    {
        public DateTime JwtTokenExpiryDate { get; set; }
        public required string JwtToken { get; set; }
        public DateTime RefreshTokenExpiryDate { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
    }
}
