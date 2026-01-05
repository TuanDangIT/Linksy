import { BlobFileRef } from "../types/blobFileRef";

export interface UtmParameterUrlRef {
  id: number;
  originalUrl: string;
  code: string;
}

export interface UtmParameterQrCodeRef {
  id: number;
  scanCount: number;
  image: BlobFileRef;
  createdAt: string;
}

export interface UtmParameterDetails {
  id: number;
//   isActive: boolean;
  umtSource: string;
  umtMedium: string | null;
  umtCampaign: string | null;
  visitsCount: number;
  url: UtmParameterUrlRef;
  qrCode: UtmParameterQrCodeRef | null;
  createdAt: string;
  updatedAt: string | null;
}
