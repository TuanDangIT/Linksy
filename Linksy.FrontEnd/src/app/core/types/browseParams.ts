export interface BrowseParams {
  pageNumber?: number;
  pageSize?: number;
  orders?: string[];
  filters?: { [key: string]: string };
}
