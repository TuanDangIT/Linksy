export const environment = {
  production: true,
  apiBaseUrl:
    typeof process !== 'undefined' && process.env?.['API_BASE_URL']
      ? process.env['API_BASE_URL']
      : 'http://localhost:8080/api/v1',
  azureBlobStorageBaseUrl:
    typeof process !== 'undefined' && process.env?.['AZURE_BLOB_STORAGE_BASE_URL']
      ? process.env['AZURE_BLOB_STORAGE_BASE_URL']
      : 'http://localhost:10000',
  redirectingShortenedUrlBaseUrl: 'http://localhost:8080',
  redirectingLandingPageBaseUrl: 'http://localhost:8080/lp',
  frontEndBaseUrl: 'http://localhost:4200',
  frontEndRedirectLandingPageUrl: 'http://localhost:4200/lp',
};
