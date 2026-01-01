import { Component, inject, signal } from '@angular/core';
import { UrlListItem } from '../../../core/models/url';
import { debounceTime, distinctUntilChanged, Subject } from 'rxjs';
import { BrowseUrlsRequest } from '../../../core/types/browseUrls';
import { ShortenedUrlService } from '../../../core/services/shortened-url-service';
import { environment } from '../../../../environments/environment';
import { FilterModal } from '../../../shared/components/filter-modal/filter-modal';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faTrash, faUpload } from '@fortawesome/free-solid-svg-icons';
import { ConfirmModal } from '../../../shared/components/confirm-modal/confirm-modal';
import { CreateShortenedUrlModal } from '../create-shortened-url-modal/create-shortened-url-modal';

@Component({
  selector: 'app-shortened-url-list',
  imports: [FilterModal, FontAwesomeModule, ConfirmModal, CreateShortenedUrlModal],
  templateUrl: './shortened-url-list.html',
  styleUrl: './shortened-url-list.css',
})
export class ShortenedUrlList {
  error: string | null = null;
  urlService = inject(ShortenedUrlService);

  urls = signal<UrlListItem[]>([]);

  availableFilters = ['Code', 'OriginalUrl', 'Id', 'CreatedAt', 'UpdatedAt', 'IsActive', 'Tags'];
  isFilterModalOpen = signal(false);

  currentPage = signal(1);
  pageSize = signal(10);
  totalPages = signal(0);
  totalCount = signal(0);
  itemsFrom = signal(0);
  itemsTo = signal(0);

  pageSizeOptions = [10, 25, 50];

  currentSort = signal<string>('CreatedAt');
  sortDirection = signal<'asc' | 'desc'>('desc');

  filters = signal<Record<string, string>>({});
  searchTerm = '';
  activeFilter: boolean | null = null;

  faTrash = faTrash;
  faUpload = faUpload;

  confirmOpen = signal(false);
  confirmTitle = signal('');
  confirmMessage = signal('');
  confirmConfirmText = signal('Confirm');

  isCreateModalOpen = signal(false);

  private pendingAction = signal<
    | { type: 'delete'; url: UrlListItem }
    | { type: 'activate'; url: UrlListItem }
    | { type: 'deactivate'; url: UrlListItem }
    | null
  >(null);

  private searchSubject = new Subject<string>();

  ngOnInit(): void {
    this.loadUrls();

    this.searchSubject.pipe(debounceTime(400), distinctUntilChanged()).subscribe((term) => {
      this.searchTerm = term;
    });
  }

  loadUrls(): void {
    this.error = null;

    const orders: string[] = [];
    if (this.currentSort()) {
      const sortStr =
        this.sortDirection() === 'asc' ? this.currentSort() : `${this.currentSort()}:desc`;
      orders.push(sortStr);
    }

    const activeFilters: Record<string, string> = {};
    Object.keys(this.filters()).forEach((key) => {
      const value = this.filters()[key];
      if (value !== '' && value !== null && value !== undefined) {
        activeFilters[key] = value;
      }
    });

    const params: BrowseUrlsRequest = {
      pageNumber: this.currentPage(),
      pageSize: this.pageSize(),
      orders: orders.length > 0 ? orders : undefined,
      filters: Object.keys(activeFilters).length > 0 ? activeFilters : undefined,
    };

    this.urlService.getUrls(params).subscribe({
      next: (response) => {
        const paged = response.data.pagedResult;

        this.totalPages.set(paged.totalPages);
        this.totalCount.set(paged.totalItemsCount);
        this.itemsFrom.set(paged.itemsFrom);
        this.itemsTo.set(paged.itemsTo);

        if (paged.totalPages === 0) {
          this.currentPage.set(1);
          this.urls.set([]);
          return;
        }

        if (this.currentPage() > paged.totalPages) {
          this.currentPage.set(paged.totalPages);
          this.loadUrls();
          return;
        }

        this.urls.set(paged.items);
      },
      error: (err) => {
        this.error = 'Failed to load URLs. Please try again.';
        console.error(err);
      },
    });
  }

  onSearchInput(term: string): void {
    this.searchSubject.next(term);
  }

  applyFilters(newFilters: Record<string, string>): void {
    this.filters.set(newFilters);
    this.currentPage.set(1);
    this.closeFilterModal();
    this.loadUrls();
  }

  get appliedFilters(): Array<{ key: string; value: string }> {
    return Object.entries(this.filters()).map(([key, value]) => ({ key, value }));
  }

  clearFilters(): void {
    this.searchTerm = '';
    this.activeFilter = null;
    this.filters.set({});
    this.currentPage.set(1);
    this.loadUrls();
  }

  openFilterModal(): void {
    this.isFilterModalOpen.set(true);
  }

  closeFilterModal(): void {
    this.isFilterModalOpen.set(false);
  }

  sortBy(column: string): void {
    if (this.currentSort() === column) {
      this.sortDirection.set(this.sortDirection() === 'asc' ? 'desc' : 'asc');
    } else {
      this.currentSort.set(column);
      this.sortDirection.set('desc');
    }

    this.currentPage.set(1);
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

  prevPage(): void {
    this.goToPage(this.currentPage() - 1);
  }

  nextPage(): void {
    this.goToPage(this.currentPage() + 1);
  }

  canGoPrev(): boolean {
    return this.currentPage() > 1;
  }

  canGoNext(): boolean {
    return this.currentPage() < this.totalPages();
  }

  setPageSize(newSize: number | string): void {
    const parsed = Number(newSize);
    if (!Number.isFinite(parsed) || parsed <= 0) return;

    this.pageSize.set(parsed);
    this.currentPage.set(1);
    this.loadUrls();
  }

  rangeStart(): number {
    return this.itemsFrom();
  }

  rangeEnd(): number {
    return Math.min(this.totalCount(), this.itemsTo());
  }

  get pageNumbers(): number[] {
    const pages: number[] = [];
    const maxVisible = 5;

    let start = Math.max(1, this.currentPage() - Math.floor(maxVisible / 2));
    let end = Math.min(this.totalPages(), start + maxVisible - 1);

    if (end - start + 1 < maxVisible) {
      start = Math.max(1, end - maxVisible + 1);
    }

    for (let i = start; i <= end; i++) pages.push(i);
    return pages;
  }

  copyToClipboard(code: string): void {
    navigator.clipboard.writeText(`${environment.redirectingShortenedUrlBaseUrl}/${code}`);
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

  openDeleteConfirm(url: UrlListItem): void {
    this.pendingAction.set({ type: 'delete', url });
    this.confirmTitle.set('Delete URL');
    this.confirmMessage.set(`Are you sure you want to delete URL #${url.id} (${url.code})?`);
    this.confirmConfirmText.set('Delete');
    this.confirmOpen.set(true);
  }

  openToggleActiveConfirm(url: UrlListItem): void {
    const type = url.isActive ? 'deactivate' : 'activate';
    this.pendingAction.set({ type, url });

    this.confirmTitle.set(url.isActive ? 'Deactivate URL' : 'Activate URL');
    this.confirmMessage.set(
      url.isActive
        ? `Deactivate URL #${url.id} (${url.code})?`
        : `Activate URL #${url.id} (${url.code})?`
    );
    this.confirmConfirmText.set(url.isActive ? 'Deactivate' : 'Activate');
    this.confirmOpen.set(true);
  }

  closeConfirm(): void {
    this.confirmOpen.set(false);
    this.pendingAction.set(null);
  }

  confirmAction(): void {
    const action = this.pendingAction();
    if (!action) return;

    this.error = null;

    const request$ =
      action.type === 'delete'
        ? this.urlService.deleteUrl(action.url.id)
        : action.type === 'activate'
        ? this.urlService.activateUrl(action.url.id)
        : this.urlService.deactivateUrl(action.url.id);

    request$.subscribe({
      next: () => {
        this.closeConfirm();
        this.loadUrls();
      },
      error: (err) => {
        this.error = 'Action failed. Please try again.';
        console.error(err);
        this.closeConfirm();
      },
    });
  }

  openCreateModal(): void {
    this.isCreateModalOpen.set(true);
  }

  closeCreateModal(): void {
    this.isCreateModalOpen.set(false);
  }

  onUrlCreated(): void {
    this.closeCreateModal();
    this.currentPage.set(1);
    this.loadUrls();
  }
}
