import { CommonModule } from '@angular/common';
import { Component, inject, signal, PLATFORM_ID } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

import { LandingPageService } from '../../../core/services/landing-page-service';
import {
  LandingPage as LandingPageModel,
  PublicLandingPageItem,
} from '../../../core/models/landingPage';
import { environment } from '../../../../environments/environment';
import { toErrorList } from '../../../shared/utils/http-utils';
import { blobUrl } from '../../../shared/utils/blob-utils';

@Component({
  selector: 'app-landing-page',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './landing-page.html',
  styleUrl: './landing-page.css',
})
export class LandingPage {
  private readonly route = inject(ActivatedRoute);
  private readonly landingPages = inject(LandingPageService);
  private readonly sanitizer = inject(DomSanitizer);

  loading = signal(true);
  notFound = signal(false);
  data = signal<LandingPageModel | null>(null);

  private readonly engagementCooldownMs = 30_000;
  private lastEngagementSent = new Map<number, number>();

  ngOnInit(): void {
    const code = this.route.snapshot.paramMap.get('code');
    if (!code) {
      this.loading.set(false);
      this.notFound.set(true);
      return;
    }

    this.load(code);
  }

  private load(code: string): void {
    this.loading.set(true);
    this.notFound.set(false);

    this.landingPages.getPublicLandingPage(code).subscribe({
      next: (res) => {
        this.data.set(res.data);
        this.loading.set(false);
      },
      error: (err) => {
        console.error(err);
        this.loading.set(false);
        this.notFound.set(true);
      },
    });
  }

  blobUrl(path: string): string {
    return blobUrl(path);
  }

  itemsSorted(): PublicLandingPageItem[] {
    const items = this.data()?.landingPageItems ?? [];
    return [...items].sort((a, b) => (a.order ?? 0) - (b.order ?? 0));
  }

  extractYouTubeId(url: string): string | null {
    const u = url;

    try {
      const parsed = new URL(u);
      const vParam = parsed.searchParams.get('v');
      if (vParam) return vParam;


      if (parsed.hostname === 'youtu.be') {
        const path = parsed.pathname.slice(1);
        if (path) return path;
      }
    } catch {
      return null;
    }

    return null;
  }

  youTubeEmbedUrl(videoUrl: string): SafeResourceUrl | null {
    const id = this.extractYouTubeId(videoUrl);
    if (!id) return null;
    return this.sanitizer.bypassSecurityTrustResourceUrl(`https://www.youtube.com/embed/${id}`);
  }

  buildItemClickUrl(urlCode: string): string {
    const code = urlCode;
    const base = environment.redirectingShortenedUrlBaseUrl;
    return `${base}/${code}`;
  }

  onItemClick(item: PublicLandingPageItem): void {
    this.recordEngagement(item.id);

    if (item.type === 'Url' || (item.type === 'Image' && item.urlCode)) {
      const url = this.buildItemClickUrl(item.urlCode!);
      window.open(url, '_blank', 'noopener,noreferrer');
    } 
  }

  getBackgroundStyle(): Record<string, string> {
    const lp = this.data();
    if (!lp) return {};

    if (lp.backgroundImage?.urlPath) {
      const imgUrl = this.blobUrl(lp.backgroundImage.urlPath);
      return imgUrl
        ? {
            'background-image': `url('${imgUrl}')`,
            'background-size': 'cover',
            'background-position': 'center',
          }
        : {};
    }

    if (lp.backgroundColor) {
      return { 'background-color': lp.backgroundColor };
    }

    return {};
  }

  recordEngagement(itemId: number): void {
    const lpId = this.data()!.id;

    const now = Date.now();
    const last = this.lastEngagementSent.get(itemId) ?? 0;
    if (now - last < this.engagementCooldownMs) return;

    this.lastEngagementSent.set(itemId, now);
    this.landingPages.increaseEngagement(lpId, itemId).subscribe({
      error: (err) => console.warn('Engagement call failed', err),
    });
  }
}
