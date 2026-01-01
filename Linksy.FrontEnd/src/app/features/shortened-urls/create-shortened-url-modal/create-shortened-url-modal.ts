import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
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
import { ShortenedUrlService } from '../../../core/services/shortened-url-service';
import {
  CreateShortenedUrlRequest,
  UmtParameterRequest,
} from '../../../core/types/createShortenedUrlRequest';

@Component({
  selector: 'app-create-shortened-url-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './create-shortened-url-modal.html',
  styleUrl: './create-shortened-url-modal.css',
})
export class CreateShortenedUrlModal {
  @Input() isOpen = false;
  @Output() cancel = new EventEmitter<void>();
  @Output() created = new EventEmitter<void>();

  private readonly urlService = inject(ShortenedUrlService);

  submitting = signal(false);
  error = signal<string | null>(null);

  originalUrl = '';

  showCustomCode = signal(false);
  customCode = '';

  showTags = signal(false);
  tags = signal<string[]>(['']);

  showUmt = signal(false);
  umtParameters = signal<UmtParameterRequest[]>([
    { umtSource: '', umtMedium: '', umtCampaign: '' },
  ]);

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['isOpen']?.currentValue === true) {
      this.reset();
    }
  }

  reset(): void {
    this.error.set(null);
    this.submitting.set(false);

    this.originalUrl = '';

    this.showCustomCode.set(false);
    this.customCode = '';

    this.showTags.set(false);
    this.tags.set(['']);

    this.showUmt.set(false);
    this.umtParameters.set([{ umtSource: '', umtMedium: '', umtCampaign: '' }]);
  }

  onBackdropClick(): void {
    this.cancel.emit();
  }

  enableCustomCode(): void {
    this.showCustomCode.set(true);
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
    if (this.tags().length === 0) this.tags.set(['']);
  }

  enableUmt(): void {
    this.showUmt.set(true);
    if (this.umtParameters().length === 0) this.addUmt();
  }

  addUmt(): void {
    this.umtParameters.update((a) => [...a, { umtSource: '', umtMedium: '', umtCampaign: '' }]);
  }

  updateUmt(index: number, field: keyof UmtParameterRequest, value: string): void {
    this.umtParameters.update((a) => a.map((p, i) => (i === index ? { ...p, [field]: value } : p)));
  }

  removeUmt(index: number): void {
    this.umtParameters.update((a) => a.filter((_, i) => i !== index));
    if (this.umtParameters().length === 0) this.addUmt();
  }

  private isValidUrl(value: string): boolean {
    try {
      const u = new URL(value);
      return u.protocol === 'http:' || u.protocol === 'https:';
    } catch {
      return false;
    }
  }

  private toErrorMessage(err: unknown): string {
    if (err instanceof HttpErrorResponse) {
      const api = err.error as any;
      if (api?.message) return api.message;
      if (api?.errors) {
        const errors = Object.values(api.errors).flat();
        return errors.join(', ');
      }
      return `Error ${err.status}: ${err.message}`;
    }
    return 'Failed to create URL. Please try again.';
  }

  onSubmit(form: NgForm): void {
    this.error.set(null);

    const original = (this.originalUrl ?? '').trim();
    if (!original) {
      this.error.set('Original URL is required.');
      return;
    }
    if (!this.isValidUrl(original)) {
      this.error.set('Original URL must be a valid http/https URL.');
      return;
    }
    if (form.invalid) return;

    const payload: CreateShortenedUrlRequest = { originalUrl: original };

    if (this.showCustomCode()) {
      const code = (this.customCode ?? '').trim();
      if (code) payload.customCode = code;
    }

    if (this.showTags()) {
      const cleanTags = this.tags()
        .map((t) => (t ?? '').trim())
        .filter((t) => t.length > 0);
      if (cleanTags.length > 0) payload.tags = cleanTags;
    }

    if (this.showUmt()) {
      const raw = this.umtParameters().map((p) => ({
        umtSource: (p.umtSource ?? '').trim(),
        umtMedium: (p.umtMedium ?? '').trim(),
        umtCampaign: (p.umtCampaign ?? '').trim(),
      }));

      const nonEmpty = raw.filter((p) => p.umtSource || p.umtMedium || p.umtCampaign);

      const hasPartial = nonEmpty.some((p) => !p.umtSource || !p.umtMedium || !p.umtCampaign);
      if (hasPartial) {
        this.error.set('Each UMT parameter row must have Source, Medium, and Campaign.');
        return;
      }

      if (nonEmpty.length > 0) payload.umtParameters = nonEmpty;
    }

    this.submitting.set(true);

    this.urlService.createUrl(payload).subscribe({
      next: () => {
        this.submitting.set(false);
        this.created.emit();
      },
      error: (err) => {
        this.submitting.set(false);
        this.error.set(this.toErrorMessage(err));
      },
    });
  }
}
