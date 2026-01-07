import { environment } from "../../../environments/environment";

  export function blobUrl(path: string): string {
    const base = environment.azureBlobStorageBaseUrl;
    return `${base}${path}`;
  }