import { BlobFileRef } from '../types/blobFileRef';

export interface LandingPage {}

export interface LandingPageListItem {
  id: number;
  code: string;
  isPublished: boolean;
  engagementCount: number;
  viewCount: number;
  title: string;
  tags?: string[];
  createdAt: string;
  updatedAt?: string;
}

export interface LandingPageDetails {
  id: number;
  code: string;
  isActive: boolean;
  engagementCount: number;
  viewCount: number;
  title: string;
  titleFontColor: string;
  description?: string;
  descriptionFontColor?: string;
  logoImage?: BlobFileRef;
  backgroundColor?: string;
  backgroundImage?: BlobFileRef;
  tags?: string[];
  landingPageItems: LandingPageItem[];
  createdAt: string;
  updatedAt?: string;
}

export type LandingPageItemType = 'Text' | 'YouTube' | 'Url' | 'Image';

export interface LandingPageItemBase {
  id: number;
  type: LandingPageItemType;
  order: number;
  clickCount: number;
  createdAt: string;
  updatedAt?: string | null;
}

export interface TextLandingPageItem extends LandingPageItemBase {
  type: 'Text';
  content: string;
  backgroundColor: string;
  fontColor: string;
}

export interface YouTubeLandingPageItem extends LandingPageItemBase {
  type: 'YouTube';
  videoUrl: string;
}

export interface UrlLandingPageItem extends LandingPageItemBase {
  type: 'Url';
  content: string;
  backgroundColor: string;
  fontColor: string;
  url: string | null;
}

export interface ImageLandingPageItem extends LandingPageItemBase {
  type: 'Image';
  imageUrl: string;
  altText: string;
  url: string | null;
}

export type LandingPageItem =
  | TextLandingPageItem
  | YouTubeLandingPageItem
  | UrlLandingPageItem
  | ImageLandingPageItem;
