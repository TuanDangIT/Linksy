import { UrlListItem } from '../models/url';
import { BrowseParams } from './browseParams';
import { PagedResult } from './pagedResult';

export interface BrowseUrlsResponse {
  pagedResult: PagedResult<UrlListItem>;
}

export interface BrowseUrlsRequest extends BrowseParams{
}
