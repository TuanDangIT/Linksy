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
import { FormsModule } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faGear } from '@fortawesome/free-solid-svg-icons';
import { BaseChartDirective } from 'ng2-charts';
import type { ChartConfiguration } from 'chart.js';

import { AnalyticsService } from '../../../core/services/analytics-service';
import { ErrorBox } from '../error-box/error-box';
import { toErrorList } from '../../utils/http-error-utils';
import { toUtcDayEndIso, toUtcDayStartIso } from '../../utils/date-utils';
import { AnalyticsRequest, TimeInterval, TimeRange } from '../../../core/types/analytics';

export type AnalyticsEntityType = 'url' | 'utm';

const allowedIntervalsByRange: Record<Lowercase<TimeRange>, TimeInterval[]> = {
  day: ['Minutes30', 'Hourly'],
  week: ['Hourly', 'Daily'],
  month: ['Daily', 'Weekly'],
  year: ['Weekly', 'Monthly'],
  currentyear: ['Weekly', 'Monthly'],
  custom: ['Minutes30', 'Hourly', 'Daily', 'Weekly', 'Monthly'],
};

@Component({
  selector: 'app-analytics-line-chart',
  standalone: true,
  imports: [CommonModule, FormsModule, FontAwesomeModule, BaseChartDirective, ErrorBox],
  templateUrl: './analytics-line-chart.html',
  styleUrl: './analytics-line-chart.css',
})
export class AnalyticsLineChart {
  private readonly platformId = inject(PLATFORM_ID);
  readonly isBrowser = isPlatformBrowser(this.platformId);

  private readonly analytics = inject(AnalyticsService);

  @Input({ required: true }) entityType: AnalyticsEntityType = 'url';
  @Input({ required: true }) entityId: number | null = null;

  @Input() title = 'Visits';
  @Input() withTopMargin = false;

  @Input() initialTimeRange: TimeRange = 'Day';
  @Input() initialInterval: TimeInterval = 'Hourly';

  @Input() reloadKey: unknown;

  @Output() errors = new EventEmitter<string[]>();

  faGear = faGear;

  loading = signal(false);
  chartData = signal<ChartConfiguration<'line'>['data']>({
    labels: [],
    datasets: [
      { label: 'Visits', data: [], tension: 0.25 },
      { label: 'Unique IPs', data: [], tension: 0.25 },
    ],
  });

  chartOptions: ChartConfiguration<'line'>['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    interaction: { mode: 'index', intersect: false },
    plugins: { legend: { display: true } },
    scales: { y: { beginAtZero: true } },
  };

  settingsOpen = signal(false);
  settingsErrors = signal<string[]>([]);

  timeRange = signal<TimeRange>('Day');
  interval = signal<TimeInterval>('Hourly');
  fromDate = signal<string>(''); 
  toDate = signal<string>(''); 

  readonly timeRanges: Array<{ value: TimeRange; label: string }> = [
    { value: 'Day', label: 'Day (today)' },
    { value: 'Week', label: 'Week' },
    { value: 'Month', label: 'Month' },
    { value: 'Year', label: 'Year' },
    { value: 'CurrentYear', label: 'Current year' },
    { value: 'Custom', label: 'Custom' },
  ];

  ngOnInit(): void {
    this.timeRange.set(this.initialTimeRange ?? 'Day');

    const allowed = this.allowedIntervalsFor(this.timeRange());
    const initInterval = this.initialInterval ?? allowed[0];
    this.interval.set(allowed.includes(initInterval) ? initInterval : allowed[0]);

    this.load();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['entityId'] || changes['reloadKey']) this.load();
  }

  openSettings(): void {
    this.settingsErrors.set([]);
    this.settingsOpen.set(true);
  }

  closeSettings(): void {
    this.settingsOpen.set(false);
  }

  onBackdropClick(): void {
    this.closeSettings();
  }

  onTimeRangeChange(value: TimeRange): void {
    this.timeRange.set(value);

    const allowed = this.allowedIntervalsFor(value);
    if (!allowed.includes(this.interval())) this.interval.set(allowed[0]);

    if (value !== 'Custom') this.settingsErrors.set([]);
  }

  allowedIntervals(): TimeInterval[] {
    return this.allowedIntervalsFor(this.timeRange());
  }

  private allowedIntervalsFor(value: TimeRange): TimeInterval[] {
    const key = (value ?? 'Day').toLowerCase() as Lowercase<TimeRange>;
    return allowedIntervalsByRange[key];
  }

  private validateCustomRange(from: string, to: string): string[] {
    const errs: string[] = [];

    const startIso = toUtcDayStartIso(from);
    const endIso = toUtcDayEndIso(to);

    if (!startIso) errs.push('Start date is required when TimeRange is Custom.');
    if (!endIso) errs.push('End date is required when TimeRange is Custom.');
    if (errs.length > 0) return errs;

    const startMs = Date.parse(startIso!);
    const endMs = Date.parse(endIso!);
    const nowMs = Date.now();

    if (endMs < startMs) errs.push('End date must be greater than or equal to start date.');
    if (startMs > nowMs) errs.push('Start date cannot be in the future.');
    if (endMs > nowMs) errs.push('End date cannot be in the future.');

    return errs;
  }

  applySettings(): void {
    this.settingsErrors.set([]);

    const req: AnalyticsRequest = {
      timeRange: this.timeRange(),
      interval: this.interval(),
    };

    if (req.timeRange === 'Custom') {
      const from = (this.fromDate() ?? '').trim();
      const to = (this.toDate() ?? '').trim();

      const errs = this.validateCustomRange(from, to);
      if (errs.length > 0) {
        this.settingsErrors.set(errs);
        return;
      }

      req.startDate = toUtcDayStartIso(from) ?? undefined;
      req.endDate = toUtcDayEndIso(to) ?? undefined;
    } else {
      this.fromDate.set('');
      this.toDate.set('');
    }

    this.settingsOpen.set(false);
    this.load(req);
  }

  private load(override?: AnalyticsRequest): void {
    if (!this.isBrowser) return; 
    if (!this.entityId || this.entityId <= 0) return;

    const req: AnalyticsRequest = override ?? {
      timeRange: this.timeRange(),
      interval: this.interval(),
    };

    this.loading.set(true);

    const request$ =
      this.entityType === 'url'
        ? this.analytics.getUrlEngagements(this.entityId, req)
        : this.analytics.getUtmEngagements(this.entityId, req);

    request$.subscribe({
      next: (res) => {
        const points = res.data.dataPoints ?? [];
        const labels = points.map((p) => this.formatLabel(p.periodStart, req.interval));
        const counts = points.map((p) => p.count ?? 0);
        const uniqueIps = points.map((p) => p.uniqueIpCount ?? 0);

        this.chartData.set({
          labels,
          datasets: [
            { label: 'Visits', data: counts, tension: 0.25 },
            { label: 'Unique IPs', data: uniqueIps, tension: 0.25 },
          ],
        });

        this.loading.set(false);
      },
      error: (err: HttpErrorResponse) => {
        this.loading.set(false);
        this.errors.emit(toErrorList(err));
      },
    });
  }

  private formatLabel(periodStartIso: string, interval: TimeInterval): string {
    const d = new Date(periodStartIso);
    if (Number.isNaN(d.getTime())) return periodStartIso;

    const yyyy = d.getUTCFullYear();
    const mm = String(d.getUTCMonth() + 1).padStart(2, '0');
    const dd = String(d.getUTCDate()).padStart(2, '0');

    const hh = String(d.getUTCHours()).padStart(2, '0');
    const min = String(d.getUTCMinutes()).padStart(2, '0');

    return interval === 'Minutes30' || interval === 'Hourly'
      ? `${yyyy}-${mm}-${dd} ${hh}:${min}`
      : `${yyyy}-${mm}-${dd}`;
  }
}
