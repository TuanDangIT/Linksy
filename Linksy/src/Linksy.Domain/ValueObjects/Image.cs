using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.ValueObjects
{
    public class Image
    {
        public string UrlPath { get; set; }
        public string FileName { get; set; }
        public Image(string urlPath, string fileName)
        {
            UrlPath = urlPath;
            FileName = fileName;
        }
    }
}
