import { landingPageListItem } from "../models/landingPage";
import { BrowseParams } from "./browseParams";
import { PagedResult } from "./pagedResult";

export interface BrowseLandingPagesResponse {
  pagedResult: PagedResult<landingPageListItem>;
}

export interface BrowseLandingPagesRequest extends BrowseParams{
}