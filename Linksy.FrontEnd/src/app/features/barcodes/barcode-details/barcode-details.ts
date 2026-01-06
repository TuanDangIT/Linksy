import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';

import { BarcodeService } from '../../../core/services/barcode-service';
import { BarcodeDetails as BarcodeDetailsModel } from '../../../core/models/barcode';
import { environment } from '../../../../environments/environment';
import { ErrorBox } from '../../../shared/components/error-box/error-box';
import { toErrorList } from '../../../shared/utils/http-error-utils';
import { formatDate } from '../../../shared/utils/date-utils';
import { AnalyticsLineChart } from '../../../shared/components/analytics-line-chart/analytics-line-chart';

@Component({
  selector: 'app-barcode-details',
  standalone: true,
  imports: [CommonModule, RouterLink, ErrorBox, AnalyticsLineChart],
  templateUrl: './barcode-details.html',
  styleUrl: './barcode-details.css',
})
export class BarcodeDetails {
  private readonly route = inject(ActivatedRoute);
  private readonly barcodeService = inject(BarcodeService);

  barcodeId: number | null = null;

  loading = signal(true);
  errors = signal<string[]>([]);
  data = signal<BarcodeDetailsModel | null>(null);

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
}
