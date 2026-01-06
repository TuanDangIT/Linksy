import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';

import { QrcodeService } from '../../../core/services/qrcode-service';
import { QrcodeDetails as QrDetails } from '../../../core/models/qrcode';
import { environment } from '../../../../environments/environment';
import { ErrorBox } from '../../../shared/components/error-box/error-box';
import { toErrorList } from '../../../shared/utils/http-error-utils';
import { formatDate } from '../../../shared/utils/date-utils';
import { AnalyticsLineChart } from '../../../shared/components/analytics-line-chart/analytics-line-chart';

@Component({
  selector: 'app-qrcode-details',
  standalone: true,
  imports: [CommonModule, RouterLink, ErrorBox, AnalyticsLineChart],
  templateUrl: './qrcode-details.html',
  styleUrl: './qrcode-details.css',
})
export class QrcodeDetails {
  private readonly route = inject(ActivatedRoute);
  private readonly qrcodeService = inject(QrcodeService);

  qrCodeId: number | null = null;

  loading = signal(true);
  errors = signal<string[]>([]);
  data = signal<QrDetails | null>(null);

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
}
