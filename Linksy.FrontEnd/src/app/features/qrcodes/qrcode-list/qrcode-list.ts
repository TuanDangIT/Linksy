import { Component, inject, signal } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faTrash, faDownload, faCopy, faEye } from '@fortawesome/free-solid-svg-icons';

import { QrcodeService } from '../../../core/services/qrcode-service';
import { QrcodeListItem } from '../../../core/models/qrcode';
import { BrowseQrcodesRequest } from '../../../core/types/browseQrcodes';
import { environment } from '../../../../environments/environment';

import { FilterModal } from '../../../shared/components/filter-modal/filter-modal';
import { ConfirmModal } from '../../../shared/components/confirm-modal/confirm-modal';
import { PaginationService } from '../../../core/services/pagination-service';
import { ToastService } from '../../../core/services/toast-service';
import { copyToClipboard } from '../../../shared/utils/clipboard-utils';
import { formatDate } from '../../../shared/utils/date-utils';

import { CreateQrCodeModal } from '../create-qrcode-modal/create-qrcode-modal';
import { Router, RouterLink } from '@angular/router';
import {
  buildShortUrl,
  getFileNameFromContentDisposition,
  saveBlob,
} from '../../../shared/utils/http-utils';

@Component({
  selector: 'app-qrcode-list',
  imports: [FilterModal, FontAwesomeModule, ConfirmModal, CreateQrCodeModal, RouterLink],
  templateUrl: './qrcode-list.html',
  styleUrl: './qrcode-list.css',
  providers: [PaginationService],
})
export class QrcodeList {
  private readonly toast = inject(ToastService);
  private readonly qrcodeService = inject(QrcodeService);
  private readonly pagination = inject(PaginationService);
  private readonly router = inject(Router);

  qrcodes = signal<QrcodeListItem[]>([]);

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

  private pendingDelete = signal<QrcodeListItem | null>(null);

  ngOnInit(): void {
    this.pagination.setInitialSort('CreatedAt', 'desc');
    this.loadQrcodes();
  }

  loadQrcodes(): void {
    const params = this.pagination.buildBrowseParams() as BrowseQrcodesRequest;

    this.qrcodeService.getQrcodes(params).subscribe({
      next: (response) => {
        const paged = response.data.pagedResult;
        const outcome = this.pagination.ingestPagedResult(paged);

        if (outcome === 'empty') {
          this.qrcodes.set([]);
          return;
        }

        if (outcome === 'reload') {
          this.loadQrcodes();
          return;
        }

        this.qrcodes.set(paged.items);
      },
      error: (err) => {
        console.error(err);
        this.toast.error('Failed to load QR codes. Please try again.');
      },
    });
  }

  applyFilters(newFilters: Record<string, string>): void {
    this.pagination.applyFilters(newFilters);
    this.closeFilterModal();
    this.loadQrcodes();
  }

  get appliedFilters(): Array<{ key: string; value: string }> {
    return Object.entries(this.filters()).map(([key, value]) => ({ key, value }));
  }

  clearFilters(): void {
    this.pagination.clearFilters();
    this.loadQrcodes();
  }

  openFilterModal(): void {
    this.isFilterModalOpen.set(true);
  }

  closeFilterModal(): void {
    this.isFilterModalOpen.set(false);
  }

  sortBy(column: 'Id' | 'ScanCount' | 'CreatedAt'): void {
    this.pagination.sortBy(column);
    this.loadQrcodes();
  }

  getSortIcon(column: 'Id' | 'ScanCount' | 'CreatedAt'): string {
    return this.pagination.getSortIcon(column);
  }

  goToPage(page: number): void {
    if (this.pagination.goToPage(page)) this.loadQrcodes();
  }

  prevPage(): void {
    if (this.pagination.prevPage()) this.loadQrcodes();
  }

  nextPage(): void {
    if (this.pagination.nextPage()) this.loadQrcodes();
  }

  canGoPrev(): boolean {
    return this.pagination.canGoPrev();
  }

  canGoNext(): boolean {
    return this.pagination.canGoNext();
  }

  setPageSize(newSize: number | string): void {
    if (this.pagination.setPageSize(newSize)) this.loadQrcodes();
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

  copyQrCodeUrl(qr: QrcodeListItem, isUrlType: boolean): void {
    const code = (isUrlType ? qr.url?.code : qr.umtParameter?.url?.code) ?? '';

    if (!code) {
      this.toast.error('Missing code to copy.');
      return;
    }

    if (isUrlType) {
      copyToClipboard(buildShortUrl(code, { isQrCode: true }));
      return;
    }

    copyToClipboard(
      buildShortUrl(code, {
        umtSource: qr.umtParameter?.umtSource ?? null,
        umtMedium: qr.umtParameter?.umtMedium ?? null,
        umtCampaign: qr.umtParameter?.umtCampaign ?? null,
        isQrCode: true,
      })
    );
  }

  formatDate(date: string | null | undefined): string {
    return formatDate(date ?? null);
  }

  openDeleteConfirm(qr: QrcodeListItem): void {
    this.pendingDelete.set(qr);
    this.confirmTitle.set('Delete QR Code');
    this.confirmMessage.set(`Are you sure you want to delete QR code #${qr.id}?`);
    this.confirmConfirmText.set('Delete');
    this.confirmVariant.set('danger');
    this.confirmOpen.set(true);
  }

  closeConfirm(): void {
    this.confirmOpen.set(false);
    this.pendingDelete.set(null);
  }

  confirmDelete(): void {
    const qr = this.pendingDelete();
    if (!qr) return;

    this.qrcodeService.deleteQrCode(qr.id).subscribe({
      next: () => {
        this.closeConfirm();
        this.toast.success('QR code deleted.');
        this.loadQrcodes();
      },
      error: (err) => {
        console.error(err);
        this.toast.error('Delete failed. Please try again.');
        this.closeConfirm();
      },
    });
  }

  download(qrcodeId: number): void {
    this.qrcodeService.downloadQrCode(qrcodeId).subscribe({
      next: (res) => {
        const blob = res.body;
        if (!blob) {
          this.toast.error('Download failed.');
          return;
        }

        const fromHeader = getFileNameFromContentDisposition(
          res.headers.get('content-disposition')
        );

        saveBlob(blob, fromHeader?.split('/').pop() ?? `qrcode-${qrcodeId}`);
      },
      error: (err) => {
        console.error(err);
        this.toast.error('Failed to download QR code.');
      },
    });
  }

  openCreateModal(): void {
    this.isCreateModalOpen.set(true);
  }

  closeCreateModal(): void {
    this.isCreateModalOpen.set(false);
  }

  onQrCreated(): void {
    this.closeCreateModal();
    this.currentPage.set(1);
    this.toast.success('QR code created.');
    this.loadQrcodes();
  }

  openDetails(qrcodeId: number): void {
    this.router.navigate(['/qrcodes', qrcodeId]);
  }
}
