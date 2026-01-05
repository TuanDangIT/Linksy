import { BlobFileRef } from "../types/blobFileRef";

export interface UrlListItem {
  id: number;
  originalUrl: string;
  code: string;
  visitsCount: number;
  isActive: boolean;
  hasQrCode: boolean;
  hasBarcode: boolean;
  hasLandingPageItem: boolean;
  hasUmtParameter: boolean;
  tags: string[] | null;
  createdAt: string;
  updatedAt: string | null;
}

export interface QrCodeDetails {
  id: number;
  qrCodeImage: BlobFileRef;
  scanCount: number;
  createdAt: string;
  updatedAt: string | null;
}

export interface BarcodeDetails {
  id: number;
  barcodeImage: BlobFileRef;
  scanCount: number;
  createdAt: string;
  updatedAt: string | null;
}

export interface UtmParameterDetails {
  id: number;
  umtSource: string;
  umtMedium: string | null;
  umtCampaign: string | null;
  visitCount: number;
  qrCodeId: number | null;
  qrCodeScanCount: number | null;
  createdAt: string;
  updatedAt: string | null;
}

export interface UrlDetails {
  id: number;
  originalUrl: string;
  code: string;
  visitCount: number;
  isActive: boolean;
  tags: string[];
  qrCode: QrCodeDetails | null;
  barcode: BarcodeDetails | null;
  landingPageItems: unknown[];
  umtParameters: UtmParameterDetails[];
  createdAt: string;
  updatedAt: string | null;
}
