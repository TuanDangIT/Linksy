export interface UtmParameterRequest {
  umtSource: string;
  umtMedium: string;
  umtCampaign: string;
}

export interface CreateShortenedUrlRequest {
  originalUrl: string;
  customCode?: string;
  tags?: string[];
  umtParameters?: UtmParameterRequest[];
}

export interface CreateShortenedUrlResponse {
  urlId: number;
  shortenedUrl: string;
}
