import { CommonModule } from '@angular/common';
import {
  Component,
  EventEmitter,
  Input,
  Output,
  SimpleChanges,
  inject,
  signal,
} from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';

import { LandingPageItemService } from '../../../../core/services/landing-page-item-service';
import { CreateLandingPageItemType } from '../../../../core/types/createLandingPageItem';
import { ErrorBox } from '../../../../shared/components/error-box/error-box';
import { toErrorList } from '../../../../shared/utils/http-utils';
import { isHex } from '../../../../shared/utils/hex-utils';

@Component({
  selector: 'app-create-landing-page-item-modal',
  standalone: true,
  imports: [CommonModule, FormsModule, ErrorBox],
  templateUrl: './create-landing-page-item-modal.html',
  styleUrl: './create-landing-page-item-modal.css',
})
export class CreateLandingPageItemModal {
  @Input() isOpen = false;
  @Input() landingPageId: number | null = null;

  @Output() cancel = new EventEmitter<void>();
  @Output() created = new EventEmitter<void>();

  private readonly landingPageItems = inject(LandingPageItemService);

  submitting = signal(false);
  errors = signal<string[]>([]);

  type = signal<CreateLandingPageItemType>('Text');

  content = '';
  backgroundColor = '#ffffff';
  fontColor = '#000000';

  youTubeUrl = '';

  imageFile: File | null = null;
  altText = '';
  urlId = '';

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['isOpen']?.currentValue === true) this.reset();
  }

  reset(): void {
    this.errors.set([]);
    this.submitting.set(false);

    this.type.set('Text');

    this.content = '';
    this.backgroundColor = '#ffffff';
    this.fontColor = '#000000';

    this.youTubeUrl = '';

    this.imageFile = null;
    this.altText = '';
    this.urlId = '';
  }

  onBackdropClick(): void {
    this.cancel.emit();
  }

  onImageSelected(ev: Event): void {
    const input = ev.target as HTMLInputElement;
    this.imageFile = input.files?.[0] ?? null;
  }

  onSubmit(form: NgForm): void {
    this.errors.set([]);

    const landingPageId = this.landingPageId;
    if (!landingPageId || landingPageId <= 0) {
      this.errors.set(['Missing landing page id.']);
      return;
    }

    const t = this.type();
    const errs: string[] = [];

    const content = this.content.trim();
    const bg = this.backgroundColor.trim();
    const fc = this.fontColor.trim();

    if (t === 'Text' || t === 'Url') {
      if (!content) errs.push('Content is required.');
      if (!bg) errs.push('Background color is required.');
      if (!fc) errs.push('Font color is required.');
      if (bg && !isHex(bg)) errs.push('Background color must be hex (e.g. #FFAA00).');
      if (fc && !isHex(fc)) errs.push('Font color must be hex (e.g. #FFFFFF).');
    }

    if (t === 'YouTube') {
      const yt = this.youTubeUrl.trim();
      if (!yt) errs.push('YouTube URL is required.');
    }

    if (t === 'Image') {
      if (!this.imageFile) errs.push('Image file is required.');
      const alt = this.altText.trim();
      if (!alt) errs.push('Alt text is required.');
    }

    if (form.invalid || errs.length > 0) {
      this.errors.set(errs);
      return;
    }

    this.submitting.set(true);

    if (t === 'Text') {
      this.landingPageItems
        .createTextItem(landingPageId, { content, backgroundColor: bg, fontColor: fc })
        .subscribe({
          next: () => {
            this.submitting.set(false);
            this.created.emit();
          },
          error: (err) => {
            this.submitting.set(false);
            this.errors.set(toErrorList(err));
          },
        });
      return;
    }

    if (t === 'YouTube') {
      this.landingPageItems
        .createYouTubeItem(landingPageId, { youTubeUrl: this.youTubeUrl.trim() })
        .subscribe({
          next: () => {
            this.submitting.set(false);
            this.created.emit();
          },
          error: (err) => {
            this.submitting.set(false);
            this.errors.set(toErrorList(err));
          },
        });
      return;
    }

    if (t === 'Url') {
      const parsedUrlId = this.parseNullableInt(this.urlId);

      this.landingPageItems
        .createUrlItem(landingPageId, {
          content,
          backgroundColor: bg,
          fontColor: fc,
          urlId: parsedUrlId,
        })
        .subscribe({
          next: () => {
            this.submitting.set(false);
            this.created.emit();
          },
          error: (err) => {
            this.submitting.set(false);
            this.errors.set(toErrorList(err));
          },
        });
      return;
    }

    if (t === 'Image') {
      const fd = new FormData();
      fd.append('Image', this.imageFile!, this.imageFile!.name.trim());
      fd.append('AltText', this.altText.trim());

      const parsedUrlId = this.parseNullableInt(this.urlId);
      if (parsedUrlId != null) fd.append('UrlId', String(parsedUrlId));

      this.landingPageItems.createImageItem(landingPageId, fd).subscribe({
        next: () => {
          this.submitting.set(false);
          this.created.emit();
        },
        error: (err) => {
          this.submitting.set(false);
          this.errors.set(toErrorList(err));
        },
      });
      return;
    }
  }

  private parseNullableInt(value: string | null | undefined): number | null {
    const n = Number(value);
    if (Number.isNaN(n)) return null;
    return n;
  }
}
