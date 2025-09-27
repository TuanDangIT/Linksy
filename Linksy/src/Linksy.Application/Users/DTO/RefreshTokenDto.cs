using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Users.DTO
{
    public class RefreshTokenDto
    {
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
    }
}
