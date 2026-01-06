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

import { BarcodeService } from '../../../core/services/barcode-service';
import { CreateBarcodeRequest, UtmParameterRequest } from '../../../core/types/createBarcode';
import { ErrorBox } from '../../../shared/components/error-box/error-box';
import { isValidUrl, toErrorList } from '../../../shared/utils/http-utils';

@Component({
  selector: 'app-create-barcode-modal',
  standalone: true,
  imports: [CommonModule, FormsModule, ErrorBox],
  templateUrl: './create-barcode-modal.html',
  styleUrl: './create-barcode-modal.css',
})
export class CreateBarcodeModal {
  @Input() isOpen = false;
  @Output() cancel = new EventEmitter<void>();
  @Output() created = new EventEmitter<void>();

  private readonly barcodeService = inject(BarcodeService);

  submitting = signal(false);
  errors = signal<string[]>([]);

  originalUrl = '';

  showCustomCode = signal(false);
  customCode = '';

  showTags = signal(false);
  tags = signal<string[]>(['']);

  showUtm = signal(false);
  utmParameters = signal<UtmParameterRequest[]>([
    { umtSource: '', umtMedium: '', umtCampaign: '' },
  ]);

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['isOpen']?.currentValue === true) this.reset();
  }

  reset(): void {
    this.errors.set([]);
    this.submitting.set(false);

    this.originalUrl = '';

    this.showCustomCode.set(false);
    this.customCode = '';

    this.showTags.set(false);
    this.tags.set(['']);

    this.showUtm.set(false);
    this.utmParameters.set([{ umtSource: '', umtMedium: '', umtCampaign: '' }]);
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

  enableUtm(): void {
    this.showUtm.set(true);
    if (this.utmParameters().length === 0) this.addUtm();
  }

  addUtm(): void {
    this.utmParameters.update((a) => [...a, { umtSource: '', umtMedium: '', umtCampaign: '' }]);
  }

  updateUtm(index: number, field: keyof UtmParameterRequest, value: string): void {
    this.utmParameters.update((a) => a.map((p, i) => (i === index ? { ...p, [field]: value } : p)));
  }

  removeUtm(index: number): void {
    this.utmParameters.update((a) => a.filter((_, i) => i !== index));
    if (this.utmParameters().length === 0) this.addUtm();
  }

  onSubmit(form: NgForm): void {
    this.errors.set([]);

    const original = (this.originalUrl ?? '').trim();
    if (!original) {
      this.errors.set(['Original URL is required.']);
      return;
    }
    if (!isValidUrl(original)) {
      this.errors.set(['Original URL must be a valid http/https URL.']);
      return;
    }
    if (form.invalid) return;

    const payload: CreateBarcodeRequest = {
      url: { originalUrl: original },
    };

    if (this.showCustomCode()) {
      const code = (this.customCode ?? '').trim();
      if (code) payload.url.customCode = code;
    }

    if (this.showTags()) {
      const cleanTags = this.tags()
        .map((t) => (t ?? '').trim())
        .filter(Boolean);
      if (cleanTags.length > 0) {
        payload.tags = cleanTags;
        payload.url.tags = cleanTags;
      }
    }

    if (this.showUtm()) {
      const cleaned = this.utmParameters()
        .map((p) => ({
          umtSource: (p.umtSource ?? '').trim(),
          umtMedium: (p.umtMedium ?? '').trim(),
          umtCampaign: (p.umtCampaign ?? '').trim(),
        }))
        .map((p) => ({
          ...(p.umtSource ? { umtSource: p.umtSource } : {}),
          ...(p.umtMedium ? { umtMedium: p.umtMedium } : {}),
          ...(p.umtCampaign ? { umtCampaign: p.umtCampaign } : {}),
        }))
        .filter((p) => Object.keys(p).length > 0);

      if (cleaned.length === 0) {
        this.errors.set(['At least one of UTM Source, Medium, or Campaign must be filled.']);
        return;
      }

      payload.url.umtParameters = cleaned;
    }

    this.submitting.set(true);

    this.barcodeService.createBarcode(payload).subscribe({
      next: () => {
        this.submitting.set(false);
        this.created.emit();
      },
      error: (err) => {
        this.submitting.set(false);
        this.errors.set(toErrorList(err));
      },
    });
  }
}
