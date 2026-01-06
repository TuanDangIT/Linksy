import { BlobFileRef } from '../types/blobFileRef';

export interface Barcode {}

export interface BarcodeListItem {
  id: number;
  tags?: string[];
  url: BrowseBarcodeUrlRef;
  scanCount: number;
  createdAt: string;
  updatedAt?: string;
}

export interface BrowseBarcodeUrlRef {
  id: number;
  originalUrl: string;
  code: string;
}

export interface BarcodeDetails {
  id: number;
  scanCount: number;
  image: BlobFileRef;
  tags?: string[];
  createdAt: string;
  updatedAt?: string;
  url: BarcodeUrlDetails;
}

export interface BarcodeUrlDetails {
  id: number;
  originalUrl: string;
  code: string;
}
