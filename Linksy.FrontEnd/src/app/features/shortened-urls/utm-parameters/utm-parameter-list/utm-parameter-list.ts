import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output, signal } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faEye, faTrash } from '@fortawesome/free-solid-svg-icons';
import { UtmParameterDetails } from '../../../../core/models/url';
import { environment } from '../../../../../environments/environment';
import { copyToClipboard } from '../../../../shared/utils/clipboard-utils';
import { faCopy } from '@fortawesome/free-solid-svg-icons';
import { buildShortUrl } from '../../../../shared/utils/http-utils';
import { UtmParameterTypeAnalyticsCircleChartsModal } from "../utm-parameter-type-analytics-circle-charts-modal/utm-parameter-type-analytics-circle-charts-modal";

@Component({
  selector: 'app-utm-parameter-list',
  standalone: true,
  imports: [CommonModule, FontAwesomeModule, UtmParameterTypeAnalyticsCircleChartsModal],
  templateUrl: './utm-parameter-list.html',
  styleUrl: './utm-parameter-list.css',
})
export class UtmParameterList {
  @Input() utmParameters: UtmParameterDetails[] | null = null;
  @Input() urlCode: string | null = null;

  @Input() urlId: number | null = null;

  @Output() addUtm = new EventEmitter<void>();
  @Output() addQrCode = new EventEmitter<number>();
  @Output() deleteUtm = new EventEmitter<number>();
  @Output() details = new EventEmitter<number>();

  faCopy = faCopy;
  faTrash = faTrash;
  faEye = faEye;

  statsOpen = signal(false);

  openStats(): void {
    this.statsOpen.set(true);
  }

  closeStats(): void {
    this.statsOpen.set(false);
  }

  copyUtmUrl(p: UtmParameterDetails): void {
    const code = this.urlCode;
    if (!code) return;

    copyToClipboard(
      buildShortUrl(code, {
        umtSource: p.umtSource ?? null,
        umtMedium: p.umtMedium ?? null,
        umtCampaign: p.umtCampaign ?? null,
      })
    );
  }
}
