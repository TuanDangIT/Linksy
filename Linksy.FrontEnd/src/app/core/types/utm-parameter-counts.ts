export interface UtmCampaignCount {
  campaign: string;
  count: number;
}
export interface UtmCampaignCountsResponse {
  campaignCounts: UtmCampaignCount[];
}

export interface UtmMediumCount {
  medium: string;
  count: number;
}
export interface UtmMediumCountsResponse {
  mediumCounts: UtmMediumCount[];
}

export interface UtmSourceCount {
  source: string;
  count: number;
}
export interface UtmSourceCountsResponse {
  sourceCounts: UtmSourceCount[];
}