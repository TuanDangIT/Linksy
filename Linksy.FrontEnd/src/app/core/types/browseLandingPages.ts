import { LandingPageListItem } from '../models/landingPage';
import { BrowseParams } from './browseParams';
import { PagedResult } from './pagedResult';

export interface BrowseLandingPagesResponse {
  pagedResult: PagedResult<LandingPageListItem>;
}

export interface BrowseLandingPagesRequest extends BrowseParams {}
