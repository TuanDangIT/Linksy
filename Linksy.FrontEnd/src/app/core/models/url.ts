export interface Url {}

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
