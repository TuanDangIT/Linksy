import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faEye, faTrash } from '@fortawesome/free-solid-svg-icons';
import { UtmParameterDetails } from '../../../../core/models/url';
import { environment } from '../../../../../environments/environment';
import { copyToClipboard } from '../../../../shared/utils/clipboard-utils';
import { faCopy } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-utm-parameter-list',
  standalone: true,
  imports: [CommonModule, FontAwesomeModule],
  templateUrl: './utm-parameter-list.html',
  styleUrl: './utm-parameter-list.css',
})
export class UtmParameterList {
  @Input() utmParameters: UtmParameterDetails[] | null = null;
  @Input() urlCode: string | null = null;

  @Output() addUtm = new EventEmitter<void>();
  @Output() addQrCode = new EventEmitter<number>();
  @Output() deleteUtm = new EventEmitter<number>();
  @Output() details = new EventEmitter<number>();

  faCopy = faCopy;
  faTrash = faTrash;
  faEye = faEye;

  copyUtmUrl(p: UtmParameterDetails): void {
    const code = (this.urlCode ?? '').trim();
    if (!code) return;

    copyToClipboard(
      this.buildShortUrl(code, {
        umtSource: p.umtSource ?? null,
        umtMedium: p.umtMedium ?? null,
        umtCampaign: p.umtCampaign ?? null,
      })
    );
  }

  private buildShortUrl(
    code: string,
    query: Record<string, string | null | undefined | boolean>
  ): string {
    const params = new URLSearchParams();
    for (const [key, value] of Object.entries(query)) {
      if (value === undefined || value === null) continue;
      if (typeof value === 'boolean') {
        if (value) params.set(key, 'true');
        continue;
      }
      const trimmed = value.trim();
      if (!trimmed) continue;
      params.set(key, trimmed);
    }
    const qs = params.toString();
    const base = environment.redirectingShortenedUrlBaseUrl;
    return qs ? `${base}/${code}?${qs}` : `${base}/${code}`;
  }
}
