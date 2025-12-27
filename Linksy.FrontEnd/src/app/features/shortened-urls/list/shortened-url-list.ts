import { Component, inject, signal } from '@angular/core';
import { UrlListItem } from '../../../core/models/url';
import { debounceTime, distinctUntilChanged, Subject } from 'rxjs';
import { BrowseUrlsRequest } from '../../../core/types/browseUrls';
import { ShortenedUrlService } from '../../../core/services/shortened-url-service';

@Component({
  selector: 'app-shortened-url-list',
  imports: [],
  templateUrl: './shortened-url-list.html',
  styleUrl: './shortened-url-list.css',
})
export class ShortenedUrlList {
  error: string | null = null;
  urlService = inject(ShortenedUrlService);
  urls = signal<UrlListItem[]>([]);
  availableSorts = ['VisitCount', 'CreatedAt', 'Id'];
  availableFilters = ['Code', 'OriginalUrl', 'Id', 'CreatedAt', 'UpdatedAt', 'IsActive', 'Tags'];
  currentPage = signal(1);
  pageSize = signal(10);
  totalPages = signal(0);
  totalCount = signal(0);
  currentSort = signal<string>('CreatedAt');
  sortDirection = signal<'asc' | 'desc'>('desc');
  filters = signal<Record<string, string>>({
    Code: '',
    OriginalUrl: '',
    IsActive: '',
    Tags: '',
  });
  searchTerm = '';
  activeFilter: boolean | null = null;
  private searchSubject = new Subject<string>();

  ngOnInit(): void {
    this.loadUrls();

    // Debounce search input
    this.searchSubject.pipe(debounceTime(400), distinctUntilChanged()).subscribe((term) => {
      this.searchTerm = term;
      this.applyFilters();
    });
  }

  loadUrls(): void {
    // this.loading = true;
    this.error = null;

    const orders = this.currentSort
      ? [`${this.currentSort} ${this.sortDirection}`]
      : ['CreatedAt desc'];

    const params: BrowseUrlsRequest = {
      pageNumber: this.currentPage(),
      pageSize: this.pageSize(),
      orders,
      filters: this.filters(),
    };

    this.urlService.getUrls(params).subscribe({
      next: (response) => {
        console.log('Response:', response);
        this.urls.set(response.data.pagedResult.items);
        this.totalPages.set(response.data.pagedResult.totalPages);
        this.totalCount.set(response.data.pagedResult.totalCount);
        // this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load URLs. Please try again.';
        // this.loading = false;
        console.error(err);
      },
    });
  }

  onSearchInput(term: string): void {
    this.searchSubject.next(term);
  }

  applyFilters(): void {
    this.filters.set({});

    if (this.searchTerm) {
      this.filters()['Code'] = this.searchTerm;
    }

    if (this.activeFilter !== null) {
      this.filters()['IsActive'] = this.activeFilter.toString();
    }

    this.currentPage.set(1);
    this.loadUrls();
  }

  clearFilters(): void {
    this.searchTerm = '';
    this.activeFilter = null;
    this.filters.set({});
    this.currentPage.set(1);
    this.loadUrls();
  }

  sortBy(column: string): void {
    if (this.currentSort() === column) {
      this.sortDirection.set(this.sortDirection() === 'asc' ? 'desc' : 'asc');
    } else {
      this.currentSort.set(column);
      this.sortDirection.set('desc');
    }
    this.loadUrls();
  }

  getSortIcon(column: string): string {
    if (this.currentSort() !== column) return '↕';
    return this.sortDirection() === 'asc' ? '↑' : '↓';
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages()) {
      this.currentPage.set(page);
      this.loadUrls();
    }
  }

  get pageNumbers(): number[] {
    const pages: number[] = [];
    const maxVisible = 5;
    let start = Math.max(1, this.currentPage() - Math.floor(maxVisible / 2));
    let end = Math.min(this.totalPages(), start + maxVisible - 1);

    if (end - start + 1 < maxVisible) {
      start = Math.max(1, end - maxVisible + 1);
    }

    for (let i = start; i <= end; i++) {
      pages.push(i);
    }

    return pages;
  }

  copyToClipboard(code: string): void {
    navigator.clipboard.writeText(`${window.location.origin}/${code}`);
  }

  formatDate(date: string | null): string {
    if (!date) return '-';
    return new Date(date).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit',
    });
  }
}
