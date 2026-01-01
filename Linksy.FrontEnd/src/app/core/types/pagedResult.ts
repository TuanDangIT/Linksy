export interface PagedResult<T> {
  items: T[];
  currentPageNumber: number;
  pageSize: number;
  totalPages: number;
  totalItemsCount: number;
  itemsFrom: number;
  itemsTo: number;
}
