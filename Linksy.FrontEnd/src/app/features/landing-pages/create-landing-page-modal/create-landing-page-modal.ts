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

import { LandingPageService } from '../../../core/services/landing-page-service';
import { ErrorBox } from '../../../shared/components/error-box/error-box';
import { isHex } from '../../../shared/utils/hex-utils';

@Component({
  selector: 'app-create-landing-page-modal',
  imports: [CommonModule, FormsModule, ErrorBox],
  templateUrl: './create-landing-page-modal.html',
  styleUrl: './create-landing-page-modal.css',
})
export class CreateLandingPageModal {
  @Input() isOpen = false;
  @Output() cancel = new EventEmitter<void>();
  @Output() created = new EventEmitter<void>();

  private readonly landingPages = inject(LandingPageService);

  submitting = signal(false);
  errors = signal<string[]>([]);

  code = '';
  title = '';
  titleFontColor = '';

  showDescription = signal(false);
  description = '';
  descriptionFontColor = '';

  showLogo = signal(false);
  logoImage: File | null = null;

  backgroundMode = signal<'color' | 'image'>('color');
  backgroundColor = '';
  backgroundImage: File | null = null;

  showTags = signal(false);
  tags = signal<string[]>(['']);

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['isOpen']?.currentValue === true) this.reset();
  }

  reset(): void {
    this.errors.set([]);
    this.submitting.set(false);

    this.code = '';
    this.title = '';
    this.titleFontColor = '';

    this.showDescription.set(false);
    this.description = '';
    this.descriptionFontColor = '';

    this.showLogo.set(false);
    this.logoImage = null;

    this.backgroundMode.set('color');
    this.backgroundColor = '';
    this.backgroundImage = null;

    this.showTags.set(false);
    this.tags.set(['']);
  }

  onBackdropClick(): void {
    this.cancel.emit();
  }

  enableDescription(): void {
    this.showDescription.set(true);
  }

  enableLogo(): void {
    this.showLogo.set(true);
  }

  setBackgroundMode(mode: 'color' | 'image'): void {
    this.backgroundMode.set(mode);
  }

  onLogoSelected(ev: Event): void {
    const input = ev.target as HTMLInputElement;
    this.logoImage = input.files?.[0] ?? null;
  }

  onBackgroundSelected(ev: Event): void {
    const input = ev.target as HTMLInputElement;
    this.backgroundImage = input.files?.[0] ?? null;
  }

  enableTags(): void {
    this.showTags.set(true);
    if (this.tags().length === 0) this.tags.set(['']);
  }

  addTag(): void {
    this.tags.update((t) => [...t, '']);
  }

  updateTag(index: number, value: string): void {
    this.tags.update((t) => t.map((x, i) => (i === index ? value : x)));
  }

  removeTag(index: number): void {
    this.tags.update((t) => t.filter((_, i) => i !== index));
    if (this.tags().length === 0) this.showTags.set(false);
  }

  onSubmit(form: NgForm): void {
    this.errors.set([]);

    const code = this.code.trim();
    const title = this.title.trim();
    const titleFontColor = this.titleFontColor.trim();

    const errs: string[] = [];
    if (!code) errs.push('Code is required.');
    if (!title) errs.push('Title is required.');
    if (!titleFontColor) errs.push('Title font color is required.');
    if (titleFontColor && !isHex(titleFontColor))
      errs.push('Title font color must be hex (e.g. #FFAA00).');

    if (this.showDescription()) {
      const desc = this.description.trim();
      const descColor = this.descriptionFontColor.trim();

      if (desc && !descColor) errs.push('Description font color is required when description is provided.');
      if (descColor && !isHex(descColor))
        errs.push('Description font color must be hex (e.g. #FFFFFF).');
    }

    const mode = this.backgroundMode();
    if (mode === 'color') {
      const bg = this.backgroundColor.trim();
      if (!bg) errs.push('Background color is required (or switch to image).');
      if (bg && !isHex(bg)) errs.push('Background color must be hex (e.g. #0EA5E9).');
    } else {
      if (!this.backgroundImage)
        errs.push('Background image is required when image mode is selected.');
    }

    if (form.invalid || errs.length > 0) {
      this.errors.set(errs);
      return;
    }

    const fd = new FormData();
    fd.append('Code', code.trim());
    fd.append('Title', title.trim());
    fd.append('TitleFontColor', titleFontColor.trim());

    if (this.showDescription()) {
      fd.append('Description', this.description.trim());
      fd.append('DescriptionFontColor', this.descriptionFontColor.trim());
    }

    if (this.showLogo() && this.logoImage) {
      fd.append('LogoImage', this.logoImage, this.logoImage.name.trim());
    }

    if (mode === 'color') {
      fd.append('BackgroundColor', this.backgroundColor.trim());
    } else if (this.backgroundImage) {
      fd.append('BackgroundImage', this.backgroundImage, this.backgroundImage.name.trim());
    }

    if (this.showTags()) {
      const cleanTags = this.tags()
        .map((t) => t.trim())
        .filter(Boolean);
      for (const t of cleanTags) fd.append('Tags', t);
    }

    this.submitting.set(true);

    this.landingPages.createLandingPage(fd).subscribe({
      next: () => {
        this.submitting.set(false);
        this.created.emit();
      },
      error: (err) => {
        this.submitting.set(false);
        this.errors.set(['Create landing page failed.']);
        console.error(err);
      },
    });
  }
}
