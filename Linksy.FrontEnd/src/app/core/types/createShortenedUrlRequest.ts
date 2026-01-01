export interface UmtParameterRequest {
  umtSource: string;
  umtMedium: string;
  umtCampaign: string;
}

export interface CreateShortenedUrlRequest {
  originalUrl: string;
  customCode?: string;
  tags?: string[];
  umtParameters?: UmtParameterRequest[];
}