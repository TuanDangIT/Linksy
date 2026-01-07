import { BlobFileRef } from '../types/blobFileRef';

export interface QrcodeListItem {
  id: number;
  tags?: string[];
  url?: BrowseQrCodeUrlRef;
  umtParameter?: BrowseQrCodeUmtParameterRef;
  scanCount: number;
  createdAt: string;
  updatedAt?: string;
}

export interface BrowseQrCodeUrlRef {
  id: number;
  originalUrl: string;
  code: string;
}

export interface BrowseQrCodeUmtParameterRef {
  umtParameterId: number;
  umtSource?: string;
  umtMedium?: string;
  umtCampaign?: string;
  url: BrowseQrCodeUmtParameterUrlRef;
}

export interface BrowseQrCodeUmtParameterUrlRef {
  id: number;
  originalUrl: string;
  code: string;
}

export interface QrcodeDetails {
  id: number;
  scanCount: number;
  image: BlobFileRef;
  tags?: string[];
  createdAt: string;
  updatedAt?: string;
  url?: QrCodeUrlDetails;
  umtParameter?: UtmParameterDetails;
}

export interface QrCodeUrlDetails {
  id: number;
  originalUrl: string;
  code: string;
}

export interface UtmParameterDetails {
  id: number;
  umtSource?: string;
  umtMedium?: string;
  umtCampaign?: string;
  urlId: number;
  originalUrl: string;
  code: string;
}
