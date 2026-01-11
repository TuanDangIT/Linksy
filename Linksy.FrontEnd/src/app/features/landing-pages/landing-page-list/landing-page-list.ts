import { Component, inject, signal } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faCopy, faEye, faUpload, faTrash } from '@fortawesome/free-solid-svg-icons';

import { LandingPageService } from '../../../core/services/landing-page-service';
import { LandingPageListItem } from '../../../core/models/landingPage';
import { BrowseLandingPagesRequest } from '../../../core/types/browseLandingPages';
import { environment } from '../../../../environments/environment';

import { FilterModal } from '../../../shared/components/filter-modal/filter-modal';
import { PaginationService } from '../../../core/services/pagination-service';
import { ToastService } from '../../../core/services/toast-service';
import { copyToClipboard } from '../../../shared/utils/clipboard-utils';
import { formatDate } from '../../../shared/utils/date-utils';
import { ConfirmModal } from '../../../shared/components/confirm-modal/confirm-modal';
import { CreateLandingPageModal } from '../create-landing-page-modal/create-landing-page-modal';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-landing-page-list',
  imports: [
    FilterModal,
    FontAwesomeModule,
    CreateLandingPageModal,
    ConfirmModal,
    FontAwesomeModule,
    RouterLink,
  ],
  templateUrl: './landing-page-list.html',
  styleUrl: './landing-page-list.css',
  providers: [PaginationService],
})
export class LandingPageList {
  private readonly toast = inject(ToastService);
  private readonly landingPages = inject(LandingPageService);
  private readonly pagination = inject(PaginationService);
  private readonly router = inject(Router);

  landingPageItems = signal<LandingPageListItem[]>([]);

  availableFilters = [
    'Code',
    'Id',
    'Title',
    'IsPublished',
    'Tags',
    'CreatedAt',
    'UpdatedAt',
    'ViewCount',
    'EngagementCount',
  ];

  isFilterModalOpen = signal(false);
  isCreateModalOpen = signal(false);

  pageSizeOptions = [10, 25, 50];

  currentPage = this.pagination.currentPage;
  pageSize = this.pagination.pageSize;
  totalPages = this.pagination.totalPages;
  totalCount = this.pagination.totalCount;

  currentSort = this.pagination.currentSort;
  sortDirection = this.pagination.sortDirection;
  filters = this.pagination.filters;

  faCopy = faCopy;
  faEye = faEye;
  faUpload = faUpload;
  faTrash = faTrash;

  confirmOpen = signal(false);
  confirmTitle = signal('');
  confirmMessage = signal('');
  confirmConfirmText = signal('Confirm');
  confirmVariant = signal<'primary' | 'danger'>('primary');

  private pendingAction = signal<
    | { type: 'publish'; landingPage: LandingPageListItem }
    | { type: 'unpublish'; landingPage: LandingPageListItem }
    | { type: 'delete'; landingPage: LandingPageListItem }
    | null
  >(null);

  ngOnInit(): void {
    this.pagination.setInitialSort('CreatedAt', 'desc');
    this.loadLandingPages();
  }

  loadLandingPages(): void {
    const params = this.pagination.buildBrowseParams() as BrowseLandingPagesRequest;

    this.landingPages.getLandingPages(params).subscribe({
      next: (response) => {
        const paged = response.data.pagedResult;
        const outcome = this.pagination.ingestPagedResult(paged);

        if (outcome === 'empty') {
          this.landingPageItems.set([]);
          return;
        }

        if (outcome === 'reload') {
          this.loadLandingPages();
          return;
        }

        this.landingPageItems.set(paged.items);
      },
      error: (err) => {
        console.error(err);
        this.toast.error('Failed to load landing pages. Please try again.');
      },
    });
  }

  applyFilters(newFilters: Record<string, string>): void {
    this.pagination.applyFilters(newFilters);
    this.closeFilterModal();
    this.loadLandingPages();
  }

  get appliedFilters(): Array<{ key: string; value: string }> {
    return Object.entries(this.filters()).map(([key, value]) => ({ key, value }));
  }

  clearFilters(): void {
    this.pagination.clearFilters();
    this.loadLandingPages();
  }

  openFilterModal(): void {
    this.isFilterModalOpen.set(true);
  }

  closeFilterModal(): void {
    this.isFilterModalOpen.set(false);
  }

  openCreateModal(): void {
    this.isCreateModalOpen.set(true);
  }

  closeCreateModal(): void {
    this.isCreateModalOpen.set(false);
  }

  onCreated(): void {
    this.closeCreateModal();
    this.currentPage.set(1);
    this.toast.success('Landing page created.');
    this.loadLandingPages();
  }

  sortBy(column: 'Id' | 'CreatedAt' | 'ViewCount' | 'EngagementCount'): void {
    this.pagination.sortBy(column);
    this.loadLandingPages();
  }

  getSortIcon(column: 'Id' | 'CreatedAt' | 'ViewCount' | 'EngagementCount'): string {
    return this.pagination.getSortIcon(column);
  }

  goToPage(page: number): void {
    if (this.pagination.goToPage(page)) this.loadLandingPages();
  }

  prevPage(): void {
    if (this.pagination.prevPage()) this.loadLandingPages();
  }

  nextPage(): void {
    if (this.pagination.nextPage()) this.loadLandingPages();
  }

  canGoPrev(): boolean {
    return this.pagination.canGoPrev();
  }

  canGoNext(): boolean {
    return this.pagination.canGoNext();
  }

  setPageSize(newSize: number | string): void {
    if (this.pagination.setPageSize(newSize)) this.loadLandingPages();
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
  copyLandingPageUrl(code: string): void {
    if (!code) return;
    copyToClipboard(this.buildLandingPageUrl(code));
  }

  openLandingPage(code: string): void {
    if (!code) return;
    window.open(this.buildLandingPageUrl(code), '_blank', 'noopener,noreferrer');
  }

  formatDate(date: string | null | undefined): string {
    return formatDate(date ?? null);
  }

  openTogglePublishedConfirm(lp: LandingPageListItem): void {
    const type = lp.isPublished ? 'unpublish' : 'publish';
    this.pendingAction.set({ type, landingPage: lp });

    this.confirmTitle.set(lp.isPublished ? 'Unpublish landing page' : 'Publish landing page');
    this.confirmMessage.set(
      lp.isPublished
        ? `Unpublish landing page #${lp.id} (${lp.code})?`
        : `Publish landing page #${lp.id} (${lp.code})?`
    );
    this.confirmConfirmText.set(lp.isPublished ? 'Unpublish' : 'Publish');
    this.confirmVariant.set('primary');
    this.confirmOpen.set(true);
  }

  closeConfirm(): void {
    this.confirmOpen.set(false);
    this.pendingAction.set(null);
  }

  confirmAction(): void {
    const action = this.pendingAction();
    if (!action) return;

    const id = action.landingPage.id;

    let request$;
    switch (action.type) {
      case 'delete':
        request$ = this.landingPages.deleteLandingPage(id);
        break;
      case 'publish':
        request$ = this.landingPages.publishLandingPage(id);
        break;
      default:
        request$ = this.landingPages.unpublishLandingPage(id);
        break;
    }

    request$.subscribe({
      next: () => {
        this.closeConfirm();
        if (action.type === 'delete') this.toast.success('Landing page deleted.');
        if (action.type === 'publish') this.toast.success('Landing page published.');
        if (action.type === 'unpublish') this.toast.success('Landing page unpublished.');
        this.loadLandingPages();
      },
      error: (err) => {
        console.error(err);
        this.toast.error('Action failed. Please try again.');
        this.closeConfirm();
      },
    });
  }

  openDeleteConfirm(lp: LandingPageListItem): void {
    this.pendingAction.set({ type: 'delete', landingPage: lp });

    this.confirmTitle.set('Delete landing page');
    this.confirmMessage.set(`Are you sure you want to delete landing page #${lp.id} (${lp.code})?`);
    this.confirmConfirmText.set('Delete');
    this.confirmVariant.set('danger');
    this.confirmOpen.set(true);
  }

  openDetails(id: number): void {
    this.router.navigate(['/landing-pages', id]);
  }

  private buildLandingPageUrl(code: string): string {
    const base = environment.frontEndRedirectLandingPageUrl;
    return `${base}/${code}`;
  }
}
