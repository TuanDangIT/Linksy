import { CommonModule, isPlatformBrowser } from '@angular/common';
import {
  Component,
  EventEmitter,
  Input,
  Output,
  SimpleChanges,
  inject,
  signal,
  PLATFORM_ID,
} from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faDownload, faTrash, faPlus } from '@fortawesome/free-solid-svg-icons';
import { UtmParameterService } from '../../../../core/services/utm-parameter-service';
import { QrcodeService } from '../../../../core/services/qrcode-service';
import { ErrorBox } from '../../../../shared/components/error-box/error-box';
import { toErrorList } from '../../../../shared/utils/http-error-utils';
import { ConfirmModal } from '../../../../shared/components/confirm-modal/confirm-modal';
import { environment } from '../../../../../environments/environment';
import { UtmParameterDetails } from '../../../../core/models/utm-parameter';
import { ToastService } from '../../../../core/services/toast-service';
import { formatDate } from '../../../../shared/utils/date-utils';
import { faCopy } from '@fortawesome/free-solid-svg-icons';
import { copyToClipboard } from '../../../../shared/utils/clipboard-utils';
import type { ChartConfiguration } from 'chart.js';
import { faGear } from '@fortawesome/free-solid-svg-icons';
import { AnalyticsService } from '../../../../core/services/analytics-service';
import { AnalyticsLineChart } from '../../../../shared/components/analytics-line-chart/analytics-line-chart';

@Component({
  selector: 'app-utm-parameter-details-modal',
  standalone: true,
  imports: [
    CommonModule,
    FontAwesomeModule,
    ErrorBox,
    ConfirmModal,
    AnalyticsLineChart
],
  templateUrl: './utm-parameter-details-modal.html',
  styleUrl: './utm-parameter-details-modal.css',
})
export class UtmParameterDetailsModal {
  @Input() isOpen = false;
  @Input() urlId: number | null = null;
  @Input() utmParameterId: number | null = null;
  @Input() tags: string[] | null = null;

  @Output() cancel = new EventEmitter<void>();
  @Output() changed = new EventEmitter<void>();

  private readonly platformId = inject(PLATFORM_ID);
  isBrowser = isPlatformBrowser(this.platformId);

  private readonly utmService = inject(UtmParameterService);
  private readonly qrcodeService = inject(QrcodeService);
  private readonly toast = inject(ToastService);
  private readonly analyticsService = inject(AnalyticsService);

  faDownload = faDownload;
  faTrash = faTrash;
  faPlus = faPlus;
  faCopy = faCopy;
  faGear = faGear;

  loading = signal(false);
  errors = signal<string[]>([]);
  data = signal<UtmParameterDetails | null>(null);

  confirmOpen = signal(false);
  confirmTitle = signal('');
  confirmMessage = signal('');
  confirmConfirmText = signal('Delete');
  confirmVariant = signal<'primary' | 'danger'>('danger');
  private pendingQrDeleteId = signal<number | null>(null);

  analyticsLoading = signal(false);

  analyticsChartData = signal<ChartConfiguration<'line'>['data']>({
    labels: [],
    datasets: [
      { label: 'Visits', data: [], tension: 0.25 },
      { label: 'Unique IPs', data: [], tension: 0.25 },
    ],
  });

  analyticsChartOptions: ChartConfiguration<'line'>['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    interaction: { mode: 'index', intersect: false },
    plugins: { legend: { display: true } },
    scales: { y: { beginAtZero: true } },
  };

  ngOnChanges(changes: SimpleChanges): void {
    const opened = changes['isOpen']?.currentValue === true;

    if (opened) {
      this.load();
      return;
    }

    if (this.isOpen && (changes['urlId'] || changes['utmParameterId'])) {
      this.load();
    }

    if (changes['isOpen']?.currentValue === false) {
      this.errors.set([]);
      this.data.set(null);
      this.loading.set(false);
      this.closeConfirm();
    }
  }

  onBackdropClick(): void {
    this.cancel.emit();
  }

  private load(): void {
    this.errors.set([]);

    const urlId = this.urlId ?? 0;
    const utmId = this.utmParameterId ?? 0;

    if (!urlId || urlId <= 0 || !utmId || utmId <= 0) {
      this.data.set(null);
      this.loading.set(false);
      this.errors.set(['Invalid URL id or UTM parameter id.']);
      return;
    }

    this.loading.set(true);
    this.data.set(null);

    this.utmService.getUtmParameterDetails(urlId, utmId).subscribe({
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

  downloadQrCode(qrcodeId: number, fallbackFileName?: string): void {
    this.qrcodeService.downloadQrCode(qrcodeId).subscribe({
      next: (res) => {
        const blob = res.body;
        if (!blob) {
          this.toast.error('Download failed.');
          return;
        }
        const fromHeader = this.getFileNameFromContentDisposition(
          res.headers.get('content-disposition')
        );
        this.saveBlob(blob, fromHeader ?? fallbackFileName ?? `qrcode-${qrcodeId}`);
      },
      error: (err) => {
        console.error(err);
        this.toast.error('Failed to download QR code.');
      },
    });
  }

  openDeleteQrConfirm(qrcodeId: number): void {
    this.pendingQrDeleteId.set(qrcodeId);
    this.confirmTitle.set('Delete QR Code');
    this.confirmMessage.set(`Delete QR code #${qrcodeId}?`);
    this.confirmConfirmText.set('Delete');
    this.confirmVariant.set('danger');
    this.confirmOpen.set(true);
  }

  closeConfirm(): void {
    this.confirmOpen.set(false);
    this.pendingQrDeleteId.set(null);
  }

  confirmDeleteQr(): void {
    const id = this.pendingQrDeleteId();
    if (!id) return;

    this.qrcodeService.deleteQrCode(id).subscribe({
      next: () => {
        this.closeConfirm();
        this.toast.success('QR code deleted.');
        this.changed.emit();
        this.load();
      },
      error: (err) => {
        console.error(err);
        this.toast.error('Failed to delete QR code.');
        this.closeConfirm();
      },
    });
  }

  addQrCode(): void {
    const utmId = this.utmParameterId ?? 0;
    if (!utmId || utmId <= 0) return;

    const cleanTags = (this.tags ?? []).map((t) => (t ?? '').trim()).filter(Boolean);

    this.utmService.addQrCodeToUtmParameter(utmId, { tags: cleanTags }).subscribe({
      next: () => {
        this.toast.success('QR code added.');
        this.changed.emit();
        this.load();
      },
      error: (err) => {
        console.error(err);
        this.toast.error('Failed to add QR code.');
        this.errors.set(toErrorList(err));
      },
    });
  }

  formatDate(date: string | null): string {
    return formatDate(date);
  }

  copyUtmUrlFromDetails(): void {
    const d = this.data();
    if (!d) return;

    copyToClipboard(
      this.buildShortUrl(d.url.code, {
        umtSource: d.umtSource,
        umtMedium: d.umtMedium ?? null,
        umtCampaign: d.umtCampaign ?? null,
        isQrCode: !!d.qrCode,
      })
    );
  }

  copyQrOnlyUrlFromDetails(): void {
    const d = this.data();
    if (!d) return;
    copyToClipboard(
      this.buildShortUrl(d.url.code, {
        umtSource: d.umtSource ?? null,
        umtMedium: d.umtMedium ?? null,
        umtCampaign: d.umtCampaign ?? null,
        isQrCode: true,
      })
    );
  }

  onAnalyticsErrors(errs: string[]): void {
    this.errors.set([...this.errors(), ...errs]);
  }

  private buildShortUrl(
    code: string,
    query: Record<string, string | null | undefined | boolean>
  ): string {
    const params = new URLSearchParams();
    for (const [key, value] of Object.entries(query)) {
      if (value === undefined || value === null) continue;
      if (typeof value === 'boolean') {
        params.set(key, 'true');
      } else {
        params.set(key, value.toString());
      }
    }
    const qs = params.toString();
    const base = environment.redirectingShortenedUrlBaseUrl;
    return qs ? `${base}/${code}?${qs}` : `${base}/${code}`;
  }

  private getFileNameFromContentDisposition(value: string | null): string | null {
    if (!value) return null;

    const utf8 = /filename\*\s*=\s*UTF-8''([^;]+)/i.exec(value);
    if (utf8?.[1]) {
      try {
        return decodeURIComponent(utf8[1]);
      } catch {
        return utf8[1];
      }
    }

    const plain = /filename\s*=\s*"?([^";]+)"?/i.exec(value);
    return plain?.[1] ?? null;
  }

  private saveBlob(blob: Blob, fileName: string): void {
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = fileName;
    document.body.appendChild(a);
    a.click();
    a.remove();
    URL.revokeObjectURL(url);
  }
}
