import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink, Router } from '@angular/router';

import { QrcodeService } from '../../../core/services/qrcode-service';
import { QrcodeDetails as QrDetails } from '../../../core/models/qrcode';
import { environment } from '../../../../environments/environment';
import { ErrorBox } from '../../../shared/components/error-box/error-box';
import { buildShortUrl, saveBlob, toErrorList } from '../../../shared/utils/http-utils';
import { formatDate } from '../../../shared/utils/date-utils';
import { AnalyticsLineChart } from '../../../shared/components/analytics-line-chart/analytics-line-chart';
import { ConfirmModal } from '../../../shared/components/confirm-modal/confirm-modal';
import { ToastService } from '../../../core/services/toast-service';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faCopy } from '@fortawesome/free-solid-svg-icons';
import { copyToClipboard } from '../../../shared/utils/clipboard-utils';

@Component({
  selector: 'app-qrcode-details',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    ErrorBox,
    AnalyticsLineChart,
    ConfirmModal,
    FontAwesomeModule,
  ],
  templateUrl: './qrcode-details.html',
  styleUrl: './qrcode-details.css',
})
export class QrcodeDetails {
  private readonly route = inject(ActivatedRoute);
  private readonly qrcodeService = inject(QrcodeService);
  private readonly toast = inject(ToastService);
  private readonly router = inject(Router);

  qrCodeId: number | null = null;

  loading = signal(true);
  errors = signal<string[]>([]);
  data = signal<QrDetails | null>(null);

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
      this.errors.set(['Invalid QR code id.']);
      return;
    }

    this.qrCodeId = id;
    this.load(id);
  }

  private load(id: number): void {
    this.loading.set(true);
    this.errors.set([]);

    this.qrcodeService.getQrCodeById(id).subscribe({
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

  typeLabel(): 'URL' | 'UTM Parameter' {
    const d = this.data();
    if (!d) return 'URL';
    return d.url ? 'URL' : 'UTM Parameter';
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

  downloadQrCode(): void {
    const id = this.qrCodeId;
    const d = this.data();
    if (!id || !d) return;

    this.qrcodeService.downloadQrCode(id).subscribe({
      next: (res) => {
        const blob = res.body;
        if (!blob) {
          this.toast.error('Download failed.');
          return;
        }

        saveBlob(blob, this.data()!.image.fileName.split('/').pop() ?? `qrcode-${id}`);
      },
      error: (err) => {
        console.error(err);
        this.toast.error('Failed to download QR code.');
      },
    });
  }

  openDeleteConfirm(): void {
    const id = this.qrCodeId;
    if (!id) return;

    this.pendingDeleteId.set(id);
    this.confirmTitle.set('Delete QR Code');
    this.confirmMessage.set(`Delete QR code #${id}?`);
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

    this.qrcodeService.deleteQrCode(id).subscribe({
      next: () => {
        this.toast.success('QR code deleted.');
        this.closeConfirm();
        this.router.navigate(['/qrcodes']);
      },
      error: (err) => {
        console.error(err);
        this.toast.error('Failed to delete QR code.');
        this.closeConfirm();
      },
    });
  }

  copyQrCodeUrl(code: string, isUrlType: boolean): void {
    const c = (code ?? '').trim();

    if (isUrlType) {
      copyToClipboard(buildShortUrl(c, { isQrCode: true }));
    } else {
      const d = this.data();
      copyToClipboard(
        buildShortUrl(c, {
          umtSource: d!.umtParameter!.umtSource ?? null,
          umtMedium: d!.umtParameter!.umtMedium ?? null,
          umtCampaign: d!.umtParameter!.umtCampaign ?? null,
          isQrCode: true,
        })
      );
    }
  }
}
