import { BlobFileRef } from './blobFileRef';

export interface UtmParameterRequest {
  umtSource?: string;
  umtMedium?: string;
  umtCampaign?: string;
}

export interface UrlRequest {
  originalUrl: string;
  customCode?: string;
  tags?: string[];
  umtParameters?: UtmParameterRequest[];
}

export interface CreateQrCodeRequest {
  url: UrlRequest;
  tags?: string[];
}

export interface CreateQrCodeResponse {
  qrCodeId: number;
  urlId: number;
  image: BlobFileRef;
}
