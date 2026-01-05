export interface AddScanCodeToUrlRequest {
  tags: string[];
}

export interface AddQrCodeToUrlResponse {
  qrCodeId: number;
  imageUrlPath: string;
  fileName: string;
}

export interface AddBarcodeToUrlResponse {
  barcodeId: number;
  imageUrlPath: string;
  fileName: string;
}