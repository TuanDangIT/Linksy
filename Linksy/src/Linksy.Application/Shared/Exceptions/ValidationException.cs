using Linksy.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Shared.Exceptions
{
    public class ValidationException : LinksyException
    {
        public Dictionary<string, string[]> Errors { get; set; }
        public ValidationException(string title, Dictionary<string, string[]> errors) : base(title)
        {
            Errors = errors;
        }
    }
}
