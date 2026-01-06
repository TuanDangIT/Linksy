import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { LandingPageService } from '../../../core/services/landing-page-service';
import { ToastService } from '../../../core/services/toast-service';
import { LandingPageDetails as LpDetails, LandingPageItem } from '../../../core/models/landingPage';
import { ErrorBox } from '../../../shared/components/error-box/error-box';
import { AnalyticsLineChart } from '../../../shared/components/analytics-line-chart/analytics-line-chart';
import { ConfirmModal } from '../../../shared/components/confirm-modal/confirm-modal';
import { environment } from '../../../../environments/environment';
import { formatDate } from '../../../shared/utils/date-utils';
import { toErrorList } from '../../../shared/utils/http-utils';
import { LandingPageItemService } from '../../../core/services/landing-page-item-service';
import { CreateLandingPageItemModal } from '../landing-page-items/create-landing-page-item-modal/create-landing-page-item-modal';   

@Component({
  selector: 'app-landing-page-details',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    FontAwesomeModule,
    ErrorBox,
    AnalyticsLineChart,
    ConfirmModal,
    CreateLandingPageItemModal,
  ],
  templateUrl: './landing-page-details.html',
  styleUrl: './landing-page-details.css',
})
export class LandingPageDetails {
  private readonly route = inject(ActivatedRoute);
  private readonly landingPages = inject(LandingPageService);
  private readonly toast = inject(ToastService);
  private readonly landingPageItems = inject(LandingPageItemService);

  landingPageId: number | null = null;

  loading = signal(true);
  errors = signal<string[]>([]);
  data = signal<LpDetails | null>(null);

  faTrash = faTrash;

  confirmOpen = signal(false);
  confirmTitle = signal('');
  confirmMessage = signal('');
  confirmConfirmText = signal('Confirm');
  confirmVariant = signal<'primary' | 'danger'>('primary');

  createItemOpen = signal(false);

  private pendingDeleteItem = signal<LandingPageItem | null>(null);

  ngOnInit(): void {
    const rawId = this.route.snapshot.paramMap.get('id');
    const id = Number(rawId);

    if (!rawId || Number.isNaN(id) || id <= 0) {
      this.loading.set(false);
      this.errors.set(['Invalid landing page id.']);
      return;
    }

    this.landingPageId = id;
    this.load(id);
  }

  private load(id: number): void {
    this.loading.set(true);
    this.errors.set([]);

    this.landingPages.getLandingPageById(id).subscribe({
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

  onAnalyticsErrors(errs: string[]): void {
    this.errors.set([...this.errors(), ...errs]);
  }

  isActive(lp: { isActive?: boolean; isPublished?: boolean } | null | undefined): boolean {
    if (!lp) return false;
    return lp.isActive ?? lp.isPublished ?? false;
  }

  landingPagePublicUrl(code: string): string {
    const c = (code ?? '').trim();
    const base = (environment.redirectingLandingPageBaseUrl ?? '').replace(/\/+$/, '');
    return `${base}/${c}`;
  }

  blobUrl(path: string | null | undefined): string | null {
    const p = (path ?? '').trim();
    if (!p) return null;
    const base = (environment.azureBlobStorageBaseUrl ?? '').replace(/\/+$/, '');
    return `${base}${p}`;
  }

  formatDate(date: string | null | undefined): string {
    return formatDate(date ?? null);
  }

  itemsSorted(): LandingPageItem[] {
    const items = this.data()?.landingPageItems ?? [];
    return [...items].sort((a, b) => (a.order ?? 0) - (b.order ?? 0));
  }

  openDeleteItemConfirm(item: LandingPageItem): void {
    this.pendingDeleteItem.set(item);
    this.confirmTitle.set('Delete landing page item');
    this.confirmMessage.set(`Delete item #${item.order} (${item.type})?`);
    this.confirmConfirmText.set('Delete');
    this.confirmVariant.set('danger');
    this.confirmOpen.set(true);
  }

  closeConfirm(): void {
    this.confirmOpen.set(false);
    this.pendingDeleteItem.set(null);
  }

  confirmAction(): void {
    const item = this.pendingDeleteItem();
    if (!item) return;

    const landingPageId = this.landingPageId;
    if (!landingPageId) return;

    this.landingPageItems.deleteLandingPageItem(item.id).subscribe({
      next: () => {
        this.toast.success('Landing page item deleted.');
        this.closeConfirm();
        this.load(landingPageId);
      },
      error: (err) => {
        console.error(err);
        this.toast.error('Delete landing page item failed. Please try again.');
        this.closeConfirm();
      },
    });
  }

  addLandingPageElement(): void {
    this.openCreateItemModal();
  }

  openCreateItemModal(): void {
    this.createItemOpen.set(true);
  }

  closeCreateItemModal(): void {
    this.createItemOpen.set(false);
  }

  onItemCreated(): void {
    this.closeCreateItemModal();
    if (this.landingPageId) this.load(this.landingPageId);
    this.toast.success('Landing page element created.');
  }
}
