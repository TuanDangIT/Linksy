import { CommonModule, isPlatformBrowser } from '@angular/common';
import { Component, inject, signal, PLATFORM_ID } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faCopy, faDownload, faTrash, faUpload, faEye } from '@fortawesome/free-solid-svg-icons';
import { ShortenedUrlService } from '../../../core/services/shortened-url-service';
import { UrlDetails } from '../../../core/models/url';
import { environment } from '../../../../environments/environment';
import { copyToClipboard } from '../../../shared/utils/clipboard-utils';
import { formatDate } from '../../../shared/utils/date-utils';
import { ErrorBox } from '../../../shared/components/error-box/error-box';
import { buildShortUrl, saveBlob, toErrorList } from '../../../shared/utils/http-utils';
import { ConfirmModal } from '../../../shared/components/confirm-modal/confirm-modal';
import { ToastService } from '../../../core/services/toast-service';
import { QrcodeService } from '../../../core/services/qrcode-service';
import { BarcodeService } from '../../../core/services/barcode-service';
import { UtmParameterService } from '../../../core/services/utm-parameter-service';
import { UtmParameterList } from '../utm-parameters/utm-parameter-list/utm-parameter-list';
import { CreateUtmParameterModal } from '../utm-parameters/create-utm-parameter-modal/create-utm-parameter-modal';
import { UtmParameterDetailsModal } from '../utm-parameters/utm-parameter-details-modal/utm-parameter-details-modal';
import type { ChartConfiguration } from 'chart.js';
import { faGear } from '@fortawesome/free-solid-svg-icons';
import { AnalyticsLineChart } from '../../../shared/components/analytics-line-chart/analytics-line-chart';
import { blobUrl } from '../../../shared/utils/blob-utils';

type PendingAction =
  | { type: 'url-delete' | 'url-activate' | 'url-deactivate' }
  | { type: 'qrcode-delete'; id: number }
  | { type: 'barcode-delete'; id: number }
  | { type: 'utm-delete'; id: number };

@Component({
  selector: 'app-shortened-url-details',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    FontAwesomeModule,
    ErrorBox,
    ConfirmModal,
    UtmParameterList,
    CreateUtmParameterModal,
    UtmParameterDetailsModal,
    AnalyticsLineChart,
  ],
  templateUrl: './shortened-url-details.html',
  styleUrl: './shortened-url-details.css',
})
export class ShortenedUrlDetails {
  private readonly platformId = inject(PLATFORM_ID);
  isBrowser = isPlatformBrowser(this.platformId);

  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly toast = inject(ToastService);

  private readonly urlService = inject(ShortenedUrlService);
  private readonly qrcodeService = inject(QrcodeService);
  private readonly barcodeService = inject(BarcodeService);
  private readonly utmService = inject(UtmParameterService);

  faCopy = faCopy;
  faUpload = faUpload;
  faTrash = faTrash;
  faDownload = faDownload;
  faEye = faEye;
  faGear = faGear;

  urlId: number | null = null;

  loading = signal(true);
  errors = signal<string[]>([]);
  url = signal<UrlDetails | null>(null);

  confirmOpen = signal(false);
  confirmTitle = signal('');
  confirmMessage = signal('');
  confirmConfirmText = signal('Confirm');
  confirmVariant = signal<'primary' | 'danger'>('primary');
  private pendingAction = signal<PendingAction | null>(null);

  createUtmOpen = signal(false);
  utmDetailsOpen = signal(false);
  selectedUtmId = signal<number | null>(null);

  visitsChartOptions: ChartConfiguration<'line'>['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    interaction: { mode: 'index', intersect: false },
    plugins: { legend: { display: true } },
    scales: { y: { beginAtZero: true } },
  };

  ngOnInit(): void {
    const rawId = this.route.snapshot.paramMap.get('id');
    const id = Number(rawId);

    if (!rawId || Number.isNaN(id) || id <= 0) {
      this.loading.set(false);
      this.errors.set(['Invalid URL id.']);
      return;
    }

    this.urlId = id;
    this.loadUrl(id);
  }

  private loadUrl(id: number): void {
    this.loading.set(true);
    this.errors.set([]);

    this.urlService.getUrlById(id).subscribe({
      next: (res) => {
        this.url.set(res.data);
        this.loading.set(false);
      },
      error: (err) => {
        this.loading.set(false);
        this.errors.set(toErrorList(err));
      },
    });
  }

  redirectUrl(): string {
    const u = this.url();
    if (!u) return '';
    const base = environment.redirectingShortenedUrlBaseUrl;
    return `${base}/${u.code}`;
  }

  copyRedirectUrl(): void {
    const value = this.redirectUrl();
    if (value) copyToClipboard(value);
  }

  blobUrl(path: string): string {
    return blobUrl(path);
  }

  formatDate(date: string | null): string {
    return formatDate(date);
  }

  openToggleActiveConfirm(): void {
    const u = this.url();
    if (!u) return;

    this.pendingAction.set({ type: u.isActive ? 'url-deactivate' : 'url-activate' });

    this.confirmTitle.set(u.isActive ? 'Deactivate URL' : 'Activate URL');
    this.confirmMessage.set(
      u.isActive ? `Deactivate URL #${u.id} (${u.code})?` : `Activate URL #${u.id} (${u.code})?`
    );
    this.confirmConfirmText.set(u.isActive ? 'Deactivate' : 'Activate');
    this.confirmVariant.set('primary');
    this.confirmOpen.set(true);
  }

  openDeleteConfirm(): void {
    const u = this.url();
    if (!u) return;

    this.pendingAction.set({ type: 'url-delete' });
    this.confirmTitle.set('Delete URL');
    this.confirmMessage.set(`Are you sure you want to delete URL #${u.id} (${u.code})?`);
    this.confirmConfirmText.set('Delete');
    this.confirmVariant.set('danger');
    this.confirmOpen.set(true);
  }

  closeConfirm(): void {
    this.confirmOpen.set(false);
    this.pendingAction.set(null);
  }

  confirmAction(): void {
    const action = this.pendingAction();
    const u = this.url();
    if (!action) return;

    let request$;

    switch (action.type) {
      case 'url-delete':
        if (!u) return;
        request$ = this.urlService.deleteUrl(u.id);
        break;
      case 'url-activate':
        if (!u) return;
        request$ = this.urlService.activateUrl(u.id);
        break;
      case 'url-deactivate':
        if (!u) return;
        request$ = this.urlService.deactivateUrl(u.id);
        break;
      case 'qrcode-delete':
        request$ = this.qrcodeService.deleteQrCode(action.id);
        break;
      case 'barcode-delete':
        request$ = this.barcodeService.deleteBarcode(action.id);
        break;
      case 'utm-delete':
        request$ = this.utmService.deleteUtmParameter(action.id);
        break;
    }

    request$.subscribe({
      next: () => {
        const type = action.type;
        this.closeConfirm();

        if (type === 'url-delete') {
          this.toast.success('URL deleted.');
          this.router.navigate(['/shortened-urls']);
          return;
        }

        if (type === 'url-activate') this.toast.success('URL activated.');
        if (type === 'url-deactivate') this.toast.success('URL deactivated.');
        if (type === 'qrcode-delete') this.toast.success('QR code deleted.');
        if (type === 'barcode-delete') this.toast.success('Barcode deleted.');
        if (type === 'utm-delete') this.toast.success('UTM parameter deleted.');

        this.reload();
      },
      error: (err) => {
        console.error(err);
        this.toast.error('Action failed. Please try again.');
        this.closeConfirm();
      },
    });
  }

  addQrCodeToUrl(): void {
    const u = this.url();
    if (!u) return;

    const tags = (u.tags ?? []).filter(Boolean);

    this.urlService.addQrCodeToUrl(u.id, { tags }).subscribe({
      next: () => {
        this.toast.success('QR code added.');
        this.reload();
      },
      error: (err) => {
        console.error(err);
        this.toast.error('Failed to add QR code.');
        this.errors.set(toErrorList(err));
      },
    });
  }

  addBarcodeToUrl(): void {
    const u = this.url();
    if (!u) return;

    const tags = (u.tags ?? []).filter(Boolean);

    this.urlService.addBarcodeToUrl(u.id, { tags }).subscribe({
      next: () => {
        this.toast.success('Barcode added.');
        this.reload();
      },
      error: (err) => {
        console.error(err);
        this.toast.error('Failed to add barcode.');
        this.errors.set(toErrorList(err));
      },
    });
  }

  downloadQrCode(qrcodeId: number, fileName: string): void {
    this.qrcodeService.downloadQrCode(qrcodeId).subscribe({
      next: (res) => {
        const blob = res.body;
        if (!blob) {
          this.toast.error('Download failed.');
          return;
        }

        saveBlob(blob, fileName.split('/').pop() ?? `qrcode-${qrcodeId}`);
      },
      error: (err) => {
        console.error(err);
        this.toast.error('Failed to download QR code.');
      },
    });
  }

  downloadBarcode(barcodeId: number, fileName: string): void {
    this.barcodeService.downloadBarcode(barcodeId).subscribe({
      next: (res) => {
        const blob = res.body;
        if (!blob) {
          this.toast.error('Download failed.');
          return;
        }

        saveBlob(blob, fileName.split('/').pop() ?? `barcode-${barcodeId}`);
      },
      error: (err) => {
        console.error(err);
        this.toast.error('Failed to download barcode.');
      },
    });
  }

  openDeleteQrCodeConfirm(qrcodeId: number): void {
    this.pendingAction.set({ type: 'qrcode-delete', id: qrcodeId });
    this.confirmTitle.set('Delete QR Code');
    this.confirmMessage.set(`Delete QR code #${qrcodeId}?`);
    this.confirmConfirmText.set('Delete');
    this.confirmVariant.set('danger');
    this.confirmOpen.set(true);
  }

  openDeleteBarcodeConfirm(barcodeId: number): void {
    this.pendingAction.set({ type: 'barcode-delete', id: barcodeId });
    this.confirmTitle.set('Delete Barcode');
    this.confirmMessage.set(`Delete barcode #${barcodeId}?`);
    this.confirmConfirmText.set('Delete');
    this.confirmVariant.set('danger');
    this.confirmOpen.set(true);
  }

  openDeleteUtmConfirm(utmParameterId: number): void {
    this.pendingAction.set({ type: 'utm-delete', id: utmParameterId });
    this.confirmTitle.set('Delete UTM Parameter');
    this.confirmMessage.set(`Delete UTM parameter #${utmParameterId}?`);
    this.confirmConfirmText.set('Delete');
    this.confirmVariant.set('danger');
    this.confirmOpen.set(true);
  }

  addQrCodeToUtmParameter(utmParameterId: number): void {
    const u = this.url();
    if (!u) return;

    const tags = (u.tags ?? []).filter(Boolean);

    this.utmService.addQrCodeToUtmParameter(utmParameterId, { tags }).subscribe({
      next: () => {
        this.toast.success('QR code added to UTM parameter.');
        this.reload();
      },
      error: (err) => {
        console.error(err);
        this.toast.error('Failed to add QR code to UTM parameter.');
        this.errors.set(toErrorList(err));
      },
    });
  }

  openCreateUtmModal(): void {
    this.createUtmOpen.set(true);
  }

  closeCreateUtmModal(): void {
    this.createUtmOpen.set(false);
  }

  onUtmCreated(): void {
    this.createUtmOpen.set(false);
    this.toast.success('UTM parameter added.');
    this.reload();
  }

  openUtmDetails(utmId: number): void {
    this.selectedUtmId.set(utmId);
    this.utmDetailsOpen.set(true);
  }

  closeUtmDetails(): void {
    this.utmDetailsOpen.set(false);
    this.selectedUtmId.set(null);
  }

  onUtmDetailsChanged(): void {
    this.reload();
  }

  copyShortenedUrl(): void {
    const u = this.url();
    if (!u) return;
    copyToClipboard(buildShortUrl(u.code, {}));
  }

  copyQrUrl(): void {
    const u = this.url();
    if (!u) return;
    copyToClipboard(buildShortUrl(u.code, { isQrCode: true }));
  }

  copyBarcodeUrl(): void {
    const u = this.url();
    if (!u) return;
    copyToClipboard(buildShortUrl(u.code, { isBarcode: true }));
  }

  onAnalyticsErrors(errs: string[]): void {
    this.errors.set([...this.errors(), ...errs]);
  }

  private reload(): void {
    if (this.urlId == null) return;
    this.loadUrl(this.urlId);
  }
}
