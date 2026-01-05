import { Injectable, signal } from '@angular/core';
import type { BrowseParams } from '../types/browseParams';
import type { PagedResult } from '../types/pagedResult';

export type SortDirection = 'asc' | 'desc';
export type IngestPagedResultOutcome = 'ok' | 'empty' | 'reload';

@Injectable()
export class PaginationService {
  currentPage = signal(1);
  pageSize = signal(10);

  totalPages = signal(0);
  totalCount = signal(0);
  itemsFrom = signal(0);
  itemsTo = signal(0);

  currentSort = signal<string | null>(null);
  sortDirection = signal<SortDirection>('desc');

  filters = signal<Record<string, string>>({});

  setInitialSort(column: string, direction: SortDirection = 'desc'): void {
    this.currentSort.set(column);
    this.sortDirection.set(direction);
  }

  sortBy(column: string): void {
    if (this.currentSort() === column) {
      this.sortDirection.set(this.sortDirection() === 'asc' ? 'desc' : 'asc');
    } else {
      this.currentSort.set(column);
      this.sortDirection.set('desc');
    }

    this.currentPage.set(1);
  }

  getSortIcon(column: string): string {
    if (this.currentSort() !== column) return '↕';
    return this.sortDirection() === 'asc' ? '↑' : '↓';
  }

  applyFilters(newFilters: Record<string, string>): void {
    this.filters.set(newFilters);
    this.currentPage.set(1);
  }

  clearFilters(): void {
    this.filters.set({});
    this.currentPage.set(1);
  }

  goToPage(page: number): boolean {
    if (page < 1 || page > this.totalPages()) return false;
    this.currentPage.set(page);
    return true;
  }

  prevPage(): boolean {
    return this.goToPage(this.currentPage() - 1);
  }

  nextPage(): boolean {
    return this.goToPage(this.currentPage() + 1);
  }

  canGoPrev(): boolean {
    return this.currentPage() > 1;
  }

  canGoNext(): boolean {
    return this.currentPage() < this.totalPages();
  }

  setPageSize(newSize: number | string): boolean {
    const parsed = Number(newSize);
    if (!Number.isFinite(parsed) || parsed <= 0) return false;

    this.pageSize.set(parsed);
    this.currentPage.set(1);
    return true;
  }

  rangeStart(): number {
    return this.itemsFrom();
  }

  rangeEnd(): number {
    return Math.min(this.totalCount(), this.itemsTo());
  }

  pageNumbers(maxVisible = 5): number[] {
    const pages: number[] = [];

    const total = this.totalPages();
    const current = this.currentPage();

    if (total <= 0) return pages;

    let start = Math.max(1, current - Math.floor(maxVisible / 2));
    let end = Math.min(total, start + maxVisible - 1);

    if (end - start + 1 < maxVisible) {
      start = Math.max(1, end - maxVisible + 1);
    }

    for (let i = start; i <= end; i++) pages.push(i);
    return pages;
  }

  buildBrowseParams(): BrowseParams {
    const orders = this.buildOrders();
    const activeFilters = this.buildActiveFilters();

    return {
      pageNumber: this.currentPage(),
      pageSize: this.pageSize(),
      orders: orders.length > 0 ? orders : undefined,
      filters: Object.keys(activeFilters).length > 0 ? activeFilters : undefined,
    };
  }

  ingestPagedResult<T>(paged: PagedResult<T>): IngestPagedResultOutcome {
    this.totalPages.set(paged.totalPages);
    this.totalCount.set(paged.totalItemsCount);
    this.itemsFrom.set(paged.itemsFrom);
    this.itemsTo.set(paged.itemsTo);

    if (paged.totalPages === 0) {
      this.currentPage.set(1);
      return 'empty';
    }

    if (this.currentPage() > paged.totalPages) {
      this.currentPage.set(paged.totalPages);
      return 'reload';
    }

    return 'ok';
  }

  private buildOrders(): string[] {
    const sort = this.currentSort();
    if (!sort) return [];

    const dir = this.sortDirection();
    return [dir === 'asc' ? sort : `${sort}:desc`];
  }

  private buildActiveFilters(): Record<string, string> {
    const source = this.filters();
    const result: Record<string, string> = {};

    for (const key of Object.keys(source)) {
      const value = source[key];
      if (value !== '' && value !== null && value !== undefined) {
        result[key] = value;
      }
    }

    return result;
  }
}
