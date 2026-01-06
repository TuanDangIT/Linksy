import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink, Router } from '@angular/router';

import { BarcodeService } from '../../../core/services/barcode-service';
import { BarcodeDetails as BarcodeDetailsModel } from '../../../core/models/barcode';
import { environment } from '../../../../environments/environment';
import { ErrorBox } from '../../../shared/components/error-box/error-box';
import { buildShortUrl, saveBlob, toErrorList } from '../../../shared/utils/http-utils';
import { formatDate } from '../../../shared/utils/date-utils';
import { AnalyticsLineChart } from '../../../shared/components/analytics-line-chart/analytics-line-chart';
import { ToastService } from '../../../core/services/toast-service';
import { ConfirmModal } from '../../../shared/components/confirm-modal/confirm-modal';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faCopy } from '@fortawesome/free-solid-svg-icons';
import { copyToClipboard } from '../../../shared/utils/clipboard-utils';

@Component({
  selector: 'app-barcode-details',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    ErrorBox,
    AnalyticsLineChart,
    ConfirmModal,
    FontAwesomeModule,
  ],
  templateUrl: './barcode-details.html',
  styleUrl: './barcode-details.css',
})
export class BarcodeDetails {
  private readonly route = inject(ActivatedRoute);
  private readonly barcodeService = inject(BarcodeService);
  private readonly toast = inject(ToastService);
  private readonly router = inject(Router);

  barcodeId: number | null = null;

  loading = signal(true);
  errors = signal<string[]>([]);
  data = signal<BarcodeDetailsModel | null>(null);

  confirmOpen = signal(false);
  confirmTitle = signal('');
  confirmMessage = signal('');
  confirmConfirmText = signal('Confirm');
  confirmVariant = signal<'primary' | 'danger'>('primary');

  faCopy = faCopy;

  private pendingDeleteId = signal<number | null>(null);

  ngOnInit(): void {
    const rawId = this.route.snapshot.paramMap.get('id');
    const id = Number(rawId);

    if (!rawId || Number.isNaN(id) || id <= 0) {
      this.loading.set(false);
      this.errors.set(['Invalid barcode id.']);
      return;
    }

    this.barcodeId = id;
    this.load(id);
  }

  private load(id: number): void {
    this.loading.set(true);
    this.errors.set([]);

    this.barcodeService.getBarcodeById(id).subscribe({
      next: (res) => {
        this.data.set(res.data);
        this.loading.set(false);
      },
      error: (err) => {
        this.loading.set(false);
        this.errors.set(toErrorList(err));
      },
    });
  }

  blobUrl(path: string | null | undefined): string | null {
    const p = (path ?? '').trim();
    if (!p) return null;
    const base = (environment.azureBlobStorageBaseUrl ?? '').replace(/\/+$/, '');
    return `${base}${p}`;
  }

  formatDate(value: string | null | undefined): string {
    return formatDate(value ?? null);
  }

  onChartErrors(errs: string[]): void {
    this.errors.set([...this.errors(), ...errs]);
  }

  downloadBarcode(): void {
    const id = this.barcodeId;
    const d = this.data();
    if (!id || !d) return;

    this.barcodeService.downloadBarcode(id).subscribe({
      next: (res) => {
        const blob = res.body;
        if (!blob) {
          this.toast.error('Download failed.');
          return;
        }
        saveBlob(blob, this.data()!.image.fileName.split('/').pop() ?? `barcode-${id}`);
      },
      error: (err) => {
        console.error(err);
        this.toast.error('Failed to download barcode.');
      },
    });
  }

  openDeleteConfirm(): void {
    const id = this.barcodeId;
    if (!id) return;

    this.pendingDeleteId.set(id);
    this.confirmTitle.set('Delete Barcode');
    this.confirmMessage.set(`Delete barcode #${id}?`);
    this.confirmConfirmText.set('Delete');
    this.confirmVariant.set('danger');
    this.confirmOpen.set(true);
  }

  closeConfirm(): void {
    this.confirmOpen.set(false);
    this.pendingDeleteId.set(null);
  }

  confirmDelete(): void {
    const id = this.pendingDeleteId();
    if (!id) return;

    this.barcodeService.deleteBarcode(id).subscribe({
      next: () => {
        this.toast.success('Barcode deleted.');
        this.closeConfirm();
        this.router.navigate(['/barcodes']);
      },
      error: (err) => {
        console.error(err);
        this.toast.error('Failed to delete barcode.');
        this.closeConfirm();
      },
    });
  }

  copyBarcodeUrl(code: string): void {
    const c = (code ?? '').trim();
    copyToClipboard(buildShortUrl(c, { isBarcode: true }));
  }
}
