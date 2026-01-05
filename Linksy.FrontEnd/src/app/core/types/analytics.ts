export type TimeRange = 'Day' | 'Week' | 'Month' | 'Year' | 'CurrentYear' | 'Custom';
export type TimeInterval = 'Minutes30' | 'Hourly' | 'Daily' | 'Weekly' | 'Monthly' | 'Quarterly';

export interface EngagementPoint {
  periodStart: string;
  count: number;
  uniqueIpCount: number;
}

export interface EngagementResponse {
  timeRange: string;
  timeInterval: string;
  startDate: string;
  endDate: string;
  dataPoints: EngagementPoint[];
  summary: AnalyticsSummary;
}

export interface AnalyticsRequest {
  timeRange: TimeRange;
  interval: TimeInterval;
  startDate?: string | null;
  endDate?: string | null;
}

export interface AnalyticsSummary {
  total: number;
  totalUniqueIps: number;
  averageCountPerInterval: number;
  peak: number;
}
