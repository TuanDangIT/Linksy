using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Shared.Configuration
{
    public class LinksyConfig
    {
        public string BaseUrl { get; set; } = string.Empty;
        public TestUserConfig TestUser { get; set; } = default!;
        public ScanCodeConfig ScanCode { get; set; } = default!;
        public BlobStorageConfig BlobStorage { get; set; } = default!;
        public AnalyticsConfig Analytics { get; set; } = default!;
    }

    public class TestUserConfig
    {
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }

    public class ScanCodeConfig
    {
        public string QrCodeQueryParameter { get; set; } = string.Empty;
        public string BarcodeQueryParameter { get; set; } = string.Empty;
    }

    public class BlobStorageConfig
    {
        public string QrCodesPrefixPathFromContainer { get; set; } = string.Empty;
        public string BarcodesPrefixPathFromContainer { get; set; } = string.Empty;
        public string ImageLandingPageItemPathFromContainer { get; set; } = string.Empty;
    }

    public class AnalyticsConfig
    {
        public int MaxCustomRangeDays { get; set; }
        public int MinStartDateInYears { get; set; }
    }
}
