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

export interface CreateBarcodeRequest {
  url: UrlRequest;
  tags?: string[];
}

export interface CreateBarcodeResponse {
  barcodeId: number;
  urlId: number;
  image: BlobFileRef;
}
