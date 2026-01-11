import { CommonModule, isPlatformBrowser } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
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
import { forkJoin } from 'rxjs';
import { BaseChartDirective } from 'ng2-charts';
import type { ChartConfiguration } from 'chart.js';

import { AnalyticsService } from '../../../../core/services/analytics-service';
import { ErrorBox } from '../../../../shared/components/error-box/error-box';
import { toErrorList } from '../../../../shared/utils/http-utils';

type DoughnutData = ChartConfiguration<'doughnut'>['data'];

type ChartVm = {
  title: string;
  data: DoughnutData;
};

@Component({
  selector: 'app-utm-parameter-type-analytics-circle-charts-modal',
  standalone: true,
  imports: [CommonModule, BaseChartDirective, ErrorBox],
  templateUrl: './utm-parameter-type-analytics-circle-charts-modal.html',
  styleUrl: './utm-parameter-type-analytics-circle-charts-modal.css',
})
export class UtmParameterTypeAnalyticsCircleChartsModal {
  @Input() isOpen = false;
  @Input() urlId: number | null = null;

  @Output() cancel = new EventEmitter<void>();

  private readonly platformId = inject(PLATFORM_ID);
  private readonly isBrowser = isPlatformBrowser(this.platformId);

  private readonly analytics = inject(AnalyticsService);

  loading = signal(false);
  errors = signal<string[]>([]);
  charts = signal<ChartVm[]>([]);

  chartOptions: ChartConfiguration<'doughnut'>['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: { display: true, position: 'bottom' },
      tooltip: { enabled: true },
    },
  };

  ngOnChanges(changes: SimpleChanges): void {
    const opened = changes['isOpen']?.currentValue === true;

    if (opened) {
      this.load();
      return;
    }

    if (this.isOpen && changes['urlId']) {
      this.load();
      return;
    }

    if (changes['isOpen']?.currentValue === false) {
      this.reset();
    }
  }

  onBackdropClick(): void {
    this.cancel.emit();
  }

  private reset(): void {
    this.loading.set(false);
    this.errors.set([]);
    this.charts.set([]);
  }

  private load(): void {
    this.errors.set([]);
    this.charts.set([]);

    if (!this.isBrowser) return;

    const urlId = this.urlId;
    if (!urlId || urlId <= 0) return;

    this.loading.set(true);

    forkJoin({
      campaigns: this.analytics.getUrlUtmCampaignCounts(urlId),
      mediums: this.analytics.getUrlUtmMediumCounts(urlId),
      sources: this.analytics.getUrlUtmSourceCounts(urlId),
    }).subscribe({
      next: (res) => {
        const charts: ChartVm[] = [];
        
        const campaignItems = res.campaigns.data?.campaignCounts ?? [];
        const mediumItems = res.mediums.data?.mediumCounts ?? [];
        const sourceItems = res.sources.data?.sourceCounts ?? [];

        const campaignChart = this.toChart(
          'UTM Campaign',
          campaignItems.map((x) => ({ label: x.campaign, count: x.count }))
        );
        if (campaignChart) charts.push(campaignChart);

        const mediumChart = this.toChart(
          'UTM Medium',
          mediumItems.map((x) => ({ label: x.medium, count: x.count }))
        );
        if (mediumChart) charts.push(mediumChart);

        const sourceChart = this.toChart(
          'UTM Source',
          sourceItems.map((x) => ({ label: x.source, count: x.count }))
        );
        if (sourceChart) charts.push(sourceChart);

        this.charts.set(charts);
        this.loading.set(false);
      },
      error: (err: HttpErrorResponse) => {
        this.loading.set(false);
        this.errors.set(toErrorList(err));
      },
    });
  }

  private toChart(title: string, items: Array<{ label: string; count: number }>): ChartVm | null {
    const cleaned = (items ?? [])
      .map((x) => ({ label: (x.label ?? '').trim(), count: Number(x.count ?? 0) }))
      .filter((x) => x.label.length > 0 && x.count > 0);

    if (cleaned.length === 0) return null;

    const labels = cleaned.map((x) => x.label);
    const data = cleaned.map((x) => x.count);

    const colors = this.buildPalette(labels.length);

    return {
      title,
      data: {
        labels,
        datasets: [
          {
            data,
            backgroundColor: colors,
            borderWidth: 0,
          },
        ],
      },
    };
  }

  private buildPalette(n: number): string[] {
    const hsl = { h: 174, s: 83, l: 32 };

    const out: string[] = [];
    for (let i = 0; i < n; i++) {
      const t = n <= 1 ? 0.5 : i / (n - 1);
      const l = Math.round(30 + t * 40); 
      out.push(`hsl(${hsl.h} ${hsl.s}% ${l}%)`);
    }
    return out;
  }
}
