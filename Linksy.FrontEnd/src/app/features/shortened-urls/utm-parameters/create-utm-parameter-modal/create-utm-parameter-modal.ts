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
import { UtmParameterService } from '../../../../core/services/utm-parameter-service';
import { ErrorBox } from '../../../../shared/components/error-box/error-box';
import { toErrorList } from '../../../../shared/utils/http-utils';

@Component({
  selector: 'app-create-utm-parameter-modal',
  standalone: true,
  imports: [CommonModule, FormsModule, ErrorBox],
  templateUrl: './create-utm-parameter-modal.html',
  styleUrl: './create-utm-parameter-modal.css',
})
export class CreateUtmParameterModal {
  @Input() isOpen = false;
  @Input() urlId: number | null = null;

  @Output() cancel = new EventEmitter<void>();
  @Output() created = new EventEmitter<void>();

  private readonly utmService = inject(UtmParameterService);

  submitting = signal(false);
  errors = signal<string[]>([]);

  utmSource = '';
  utmMedium = '';
  utmCampaign = '';

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['isOpen']?.currentValue === true) this.reset();
  }

  private reset(): void {
    this.errors.set([]);
    this.submitting.set(false);
    this.utmSource = '';
    this.utmMedium = '';
    this.utmCampaign = '';
  }

  onBackdropClick(): void {
    this.cancel.emit();
  }

  onSubmit(form: NgForm): void {
    this.errors.set([]);

    const urlId = this.urlId;
    if (!urlId || urlId <= 0) {
      this.errors.set(['Invalid URL id.']);
      return;
    }

    const umtSource = (this.utmSource ?? '').trim();
    const umtMedium = (this.utmMedium ?? '').trim();
    const umtCampaign = (this.utmCampaign ?? '').trim();

    if (!umtSource && !umtMedium && !umtCampaign) {
      this.errors.set(['At least one of UTM Source, Medium, or Campaign must be filled.']);
      return;
    }

    if (form.invalid) return;

    this.submitting.set(true);

    const umtParameter = {
      ...(umtSource ? { umtSource } : {}),
      ...(umtMedium ? { umtMedium } : {}),
      ...(umtCampaign ? { umtCampaign } : {}),
    };

    this.utmService.addUtmParameterToUrl(urlId, { umtParameter }).subscribe({
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
