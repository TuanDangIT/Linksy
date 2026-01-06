export type CreateLandingPageItemType = 'Text' | 'YouTube' | 'Image' | 'Url';

export interface CreateTextLandingPageItemRequest {
  content: string;
  backgroundColor: string;
  fontColor: string;
}

export interface CreateYouTubeLandingPageItemRequest {
  youTubeUrl: string;
}

export interface CreateUrlLandingPageItemRequest {
  content: string;
  backgroundColor: string;
  fontColor: string;
  urlId: number | null;
}
