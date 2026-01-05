import { Component, inject, signal } from '@angular/core';
import { UrlListItem } from '../../../core/models/url';
import { BrowseUrlsRequest } from '../../../core/types/browseUrls';
import { ShortenedUrlService } from '../../../core/services/shortened-url-service';
import { environment } from '../../../../environments/environment';
import { FilterModal } from '../../../shared/components/filter-modal/filter-modal';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faTrash, faUpload, faCopy, faEye } from '@fortawesome/free-solid-svg-icons';
import { ConfirmModal } from '../../../shared/components/confirm-modal/confirm-modal';
import { CreateShortenedUrlModal } from '../create-shortened-url-modal/create-shortened-url-modal';
import { copyToClipboard } from '../../../shared/utils/clipboard-utils';
import { formatDate } from '../../../shared/utils/date-utils';
import { ToastService } from '../../../core/services/toast-service';
import { PaginationService } from '../../../core/services/pagination-service';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-shortened-url-list',
  imports: [FilterModal, FontAwesomeModule, ConfirmModal, CreateShortenedUrlModal, RouterLink],
  templateUrl: './shortened-url-list.html',
  styleUrl: './shortened-url-list.css',
  providers: [PaginationService],
})
export class ShortenedUrlList {
  toast = inject(ToastService);
  urlService = inject(ShortenedUrlService);
  pagination = inject(PaginationService);
  router = inject(Router);

  urls = signal<UrlListItem[]>([]);

  availableFilters = ['Code', 'OriginalUrl', 'Id', 'CreatedAt', 'UpdatedAt', 'IsActive', 'Tags'];
  isFilterModalOpen = signal(false);

  pageSizeOptions = [10, 25, 50];

  currentPage = this.pagination.currentPage;
  pageSize = this.pagination.pageSize;
  totalPages = this.pagination.totalPages;
  totalCount = this.pagination.totalCount;
  itemsFrom = this.pagination.itemsFrom;
  itemsTo = this.pagination.itemsTo;

  currentSort = this.pagination.currentSort;
  sortDirection = this.pagination.sortDirection;
  filters = this.pagination.filters;

  faTrash = faTrash;
  faUpload = faUpload;
  faCopy = faCopy;
  faEye = faEye;

  confirmOpen = signal(false);
  confirmTitle = signal('');
  confirmMessage = signal('');
  confirmConfirmText = signal('Confirm');
  confirmVariant = signal<'primary' | 'danger'>('primary');

  isCreateModalOpen = signal(false);

  private pendingAction = signal<
    | { type: 'delete'; url: UrlListItem }
    | { type: 'activate'; url: UrlListItem }
    | { type: 'deactivate'; url: UrlListItem }
    | null
  >(null);

  ngOnInit(): void {
    this.pagination.setInitialSort('CreatedAt', 'desc');
    this.loadUrls();
  }

  loadUrls(): void {
    const params = this.pagination.buildBrowseParams() as BrowseUrlsRequest;

    this.urlService.getUrls(params).subscribe({
      next: (response) => {
        const paged = response.data.pagedResult;

        const outcome = this.pagination.ingestPagedResult(paged);

        if (outcome === 'empty') {
          this.urls.set([]);
          return;
        }

        if (outcome === 'reload') {
          this.loadUrls();
          return;
        }

        this.urls.set(paged.items);
      },
      error: (err) => {
        const msg = 'Failed to load URLs. Please try again.';
        console.error(msg, err);
        this.toast.error(msg);
      },
    });
  }

  applyFilters(newFilters: Record<string, string>): void {
    this.pagination.applyFilters(newFilters);
    this.closeFilterModal();
    this.loadUrls();
  }

  get appliedFilters(): Array<{ key: string; value: string }> {
    return Object.entries(this.filters()).map(([key, value]) => ({ key, value }));
  }

  clearFilters(): void {
    this.pagination.clearFilters();
    this.loadUrls();
  }

  openFilterModal(): void {
    this.isFilterModalOpen.set(true);
  }

  closeFilterModal(): void {
    this.isFilterModalOpen.set(false);
  }

  sortBy(column: string): void {
    this.pagination.sortBy(column);
    this.loadUrls();
  }

  getSortIcon(column: string): string {
    return this.pagination.getSortIcon(column);
  }

  goToPage(page: number): void {
    if (this.pagination.goToPage(page)) {
      this.loadUrls();
    }
  }

  prevPage(): void {
    if (this.pagination.prevPage()) {
      this.loadUrls();
    }
  }

  nextPage(): void {
    if (this.pagination.nextPage()) {
      this.loadUrls();
    }
  }

  canGoPrev(): boolean {
    return this.pagination.canGoPrev();
  }

  canGoNext(): boolean {
    return this.pagination.canGoNext();
  }

  setPageSize(newSize: number | string): void {
    if (this.pagination.setPageSize(newSize)) {
      this.loadUrls();
    }
  }

  rangeStart(): number {
    return this.pagination.rangeStart();
  }

  rangeEnd(): number {
    return this.pagination.rangeEnd();
  }

  get pageNumbers(): number[] {
    return this.pagination.pageNumbers(5);
  }

  copyToClipboard(code: string): void {
    copyToClipboard(`${environment.redirectingShortenedUrlBaseUrl}/${code}`);
  }

  formatDate(date: string | null): string {
    return formatDate(date);
  }

  openDeleteConfirm(url: UrlListItem): void {
    this.pendingAction.set({ type: 'delete', url });
    this.confirmTitle.set('Delete URL');
    this.confirmMessage.set(`Are you sure you want to delete URL #${url.id} (${url.code})?`);
    this.confirmConfirmText.set('Delete');
    this.confirmVariant.set('danger');
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

    let request$;

    switch (action.type) {
      case 'delete':
        request$ = this.urlService.deleteUrl(action.url.id);
        break;
      case 'activate':
        request$ = this.urlService.activateUrl(action.url.id);
        break;
      default:
        request$ = this.urlService.deactivateUrl(action.url.id);
    }

    request$.subscribe({
      next: () => {
        this.closeConfirm();
        if (action.type === 'delete') this.toast.success('URL deleted.');
        if (action.type === 'activate') this.toast.success('URL activated.');
        if (action.type === 'deactivate') this.toast.success('URL deactivated.');
        this.loadUrls();
      },
      error: (err) => {
        const msg = 'Action failed. Please try again.';
        console.error(msg, err);
        this.toast.error(msg);
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
    this.toast.success('URL created.');
    this.loadUrls();
  }

  openDetails(urlId: number): void {
    this.router.navigate(['/shortened-urls', urlId]);
  }
}
