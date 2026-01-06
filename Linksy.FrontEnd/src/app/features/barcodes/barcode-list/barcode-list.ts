import { Component, inject, signal } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faTrash, faDownload, faCopy, faEye } from '@fortawesome/free-solid-svg-icons';

import { BarcodeService } from '../../../core/services/barcode-service';
import { BarcodeListItem } from '../../../core/models/barcode';
import { BrowseBarcodesRequest } from '../../../core/types/browseBarcodes';
import { environment } from '../../../../environments/environment';

import { FilterModal } from '../../../shared/components/filter-modal/filter-modal';
import { ConfirmModal } from '../../../shared/components/confirm-modal/confirm-modal';
import { PaginationService } from '../../../core/services/pagination-service';
import { ToastService } from '../../../core/services/toast-service';
import { copyToClipboard } from '../../../shared/utils/clipboard-utils';
import { formatDate } from '../../../shared/utils/date-utils';

import { CreateBarcodeModal } from '../create-barcode-modal/create-barcode-modal';
import { Router, RouterLink } from '@angular/router';

import {
  buildShortUrl,
  getFileNameFromContentDisposition,
  saveBlob,
} from '../../../shared/utils/http-utils';

@Component({
  selector: 'app-barcode-list',
  imports: [FilterModal, FontAwesomeModule, ConfirmModal, CreateBarcodeModal, RouterLink],
  templateUrl: './barcode-list.html',
  styleUrl: './barcode-list.css',
  providers: [PaginationService],
})
export class BarcodeList {
  private readonly toast = inject(ToastService);
  private readonly barcodeService = inject(BarcodeService);
  private readonly pagination = inject(PaginationService);
  private readonly router = inject(Router);

  barcodes = signal<BarcodeListItem[]>([]);

  availableFilters = [
    'Url.Code',
    'Url.OriginalUrl',
    'Id',
    'CreatedAt',
    'UpdatedAt',
    'IsActive',
    'ScanCount',
    'Tags',
    'ImageUrlPath',
  ];
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
  faDownload = faDownload;
  faCopy = faCopy;
  faEye = faEye;

  confirmOpen = signal(false);
  confirmTitle = signal('');
  confirmMessage = signal('');
  confirmConfirmText = signal('Confirm');
  confirmVariant = signal<'primary' | 'danger'>('primary');

  isCreateModalOpen = signal(false);

  private pendingDelete = signal<BarcodeListItem | null>(null);

  ngOnInit(): void {
    this.pagination.setInitialSort('CreatedAt', 'desc');
    this.loadBarcodes();
  }

  loadBarcodes(): void {
    const params = this.pagination.buildBrowseParams() as BrowseBarcodesRequest;

    this.barcodeService.getBarcodes(params).subscribe({
      next: (response) => {
        const paged = response.data.pagedResult;
        const outcome = this.pagination.ingestPagedResult(paged);

        if (outcome === 'empty') {
          this.barcodes.set([]);
          return;
        }

        if (outcome === 'reload') {
          this.loadBarcodes();
          return;
        }

        this.barcodes.set(paged.items);
      },
      error: (err) => {
        console.error(err);
        this.toast.error('Failed to load barcodes. Please try again.');
      },
    });
  }

  applyFilters(newFilters: Record<string, string>): void {
    this.pagination.applyFilters(newFilters);
    this.closeFilterModal();
    this.loadBarcodes();
  }

  get appliedFilters(): Array<{ key: string; value: string }> {
    return Object.entries(this.filters()).map(([key, value]) => ({ key, value }));
  }

  clearFilters(): void {
    this.pagination.clearFilters();
    this.loadBarcodes();
  }

  openFilterModal(): void {
    this.isFilterModalOpen.set(true);
  }

  closeFilterModal(): void {
    this.isFilterModalOpen.set(false);
  }

  sortBy(column: 'Id' | 'ScanCount' | 'CreatedAt'): void {
    this.pagination.sortBy(column);
    this.loadBarcodes();
  }

  getSortIcon(column: 'Id' | 'ScanCount' | 'CreatedAt'): string {
    return this.pagination.getSortIcon(column);
  }

  goToPage(page: number): void {
    if (this.pagination.goToPage(page)) this.loadBarcodes();
  }

  prevPage(): void {
    if (this.pagination.prevPage()) this.loadBarcodes();
  }

  nextPage(): void {
    if (this.pagination.nextPage()) this.loadBarcodes();
  }

  canGoPrev(): boolean {
    return this.pagination.canGoPrev();
  }

  canGoNext(): boolean {
    return this.pagination.canGoNext();
  }

  setPageSize(newSize: number | string): void {
    if (this.pagination.setPageSize(newSize)) this.loadBarcodes();
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

  copyBarcodeUrl(code: string): void {
    const c = (code ?? '').trim();
    copyToClipboard(buildShortUrl(c, { isBarcode: true }));
  }

  formatDate(date: string | null | undefined): string {
    return formatDate(date ?? null);
  }

  openDeleteConfirm(b: BarcodeListItem): void {
    this.pendingDelete.set(b);
    this.confirmTitle.set('Delete Barcode');
    this.confirmMessage.set(`Are you sure you want to delete barcode #${b.id}?`);
    this.confirmConfirmText.set('Delete');
    this.confirmVariant.set('danger');
    this.confirmOpen.set(true);
  }

  closeConfirm(): void {
    this.confirmOpen.set(false);
    this.pendingDelete.set(null);
  }

  confirmDelete(): void {
    const b = this.pendingDelete();
    if (!b) return;

    this.barcodeService.deleteBarcode(b.id).subscribe({
      next: () => {
        this.closeConfirm();
        this.toast.success('Barcode deleted.');
        this.loadBarcodes();
      },
      error: (err) => {
        console.error(err);
        this.toast.error('Delete failed. Please try again.');
        this.closeConfirm();
      },
    });
  }

  download(barcodeId: number): void {
    this.barcodeService.downloadBarcode(barcodeId).subscribe({
      next: (res) => {
        const blob = res.body;
        if (!blob) {
          this.toast.error('Download failed.');
          return;
        }

        const fromHeader = getFileNameFromContentDisposition(
          res.headers.get('content-disposition')
        );

        saveBlob(blob, fromHeader?.split('/').pop() ?? `barcode-${barcodeId}`);
      },
      error: (err) => {
        console.error(err);
        this.toast.error('Failed to download barcode.');
      },
    });
  }

  openCreateModal(): void {
    this.isCreateModalOpen.set(true);
  }

  closeCreateModal(): void {
    this.isCreateModalOpen.set(false);
  }

  onBarcodeCreated(): void {
    this.closeCreateModal();
    this.currentPage.set(1);
    this.toast.success('Barcode created.');
    this.loadBarcodes();
  }

  openDetails(qrcodeId: number): void {
    this.router.navigate(['/barcodes', qrcodeId]);
  }
}
