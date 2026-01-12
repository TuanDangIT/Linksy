# Getting started

## appsettings.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "LinksyConfiguration": {
    "BaseUrl": "https://localhost:5257",
    "TestUser": {
      "Username": "test",
      "Email": "test@test.com",
      "Password": "Test123!@#",
      "FirstName": "Test",
      "LastName": "Test"
    },
    "BlobStorage": {
      "QrCodesPrefixPathFromContainer": "qrcodes/",
      "BarcodesPrefixPathFromContainer": "barcodes/",
      "ImageLandingPageItemPathFromContainer": "landingpageitems/"
    },
    "Analytics": {
      "MaxCustomRangeDays": 730,
      "MinStartDateInYears": 10
    }
  },
  "Authentication": {
    "SigningKey": "ubeeg2aigeiDongei1Ni3oel5az2oes0vohd6ohweiphaoyahP231",
    "Issuer": "Linksy",
    "Audience": "Linksy",
    "ValidateAudience": true,
    "ValidateIssuer": true,
    "ValidateLifetime": true,
    "ExpiryInMinutes": 60,
    "RefreshTokenExpiryInDays": 7,
    "RefreshThresholdMinutes": 5
  },
  "ConnectionStrings": {
    "Postgres": "Host=localhost;User ID=postgres;Password=mysecretpassword;Database=LinksyDb;Port=5432",
    "AzureBlobStorage": "UseDevelopmentStorage=true"
  },
  "BlobAzureStorage": {
    "MaxFileSizeInBytes": 10485760,
    "ClientName": "LinkyBlobClient"
  },
  "AllowedOrigin": "http://localhost:4200"
}
```
